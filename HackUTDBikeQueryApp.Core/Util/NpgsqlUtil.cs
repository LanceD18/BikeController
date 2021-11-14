using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Diagnostics;
using BikeController.Core.Models;

namespace HackUTDBikeQueryApp.Core.Util
{
    public static class NpgsqlUtil
    {
        private static readonly string CONNECTION_URI_STRING = "postgres://bikeracks:somethingclever@10.8.0.1:5432/bikeracks";

        public static Queue<BikeLocationModel> QueryBikeList()
        {
            Queue<BikeLocationModel> bikes = new Queue<BikeLocationModel>();
            using (NpgsqlConnection con = new NpgsqlConnection(ParseUri(CONNECTION_URI_STRING)))
            {
                con.Open();

                string sql = "SELECT * FROM pending";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                {
                    List<object[]> results = new List<object[]>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var values = new object[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                values[i] = reader[i];
                            }
                            results.Add(values);
                        }
                    }

                    foreach (object[] values in results)
                    {
                        bikes.Enqueue(new BikeLocationModel(values));
                    }
                }
            }

            return bikes;
        }

        /// <summary>
        /// Converts the given Uri into Connection String format
        /// </summary>
        /// <returns></returns>
        private static string ParseUri(string connectionString)
        {
            Uri databaseUri = new Uri(connectionString);
            string[] userInfo = databaseUri.UserInfo.Split(':');

            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };

            return builder.ToString();
        }

        /// <summary>
        /// Moves the currently pending bike item to an accepted location
        /// </summary>
        public static void MovePendingToLocation(int pendingId)
        {
            AddPendingToLocation(pendingId);
            RemovePending(pendingId);
        }

        private static void AddPendingToLocation(int pendingId)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(ParseUri(CONNECTION_URI_STRING)))
            {
                con.Open();

                string sql = "INSERT INTO locations SELECT * FROM pending where Id = " + pendingId;

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Removes the currently pending bike item, used to move and deny items
        /// </summary>
        public static void RemovePending(int pendingId)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(ParseUri(CONNECTION_URI_STRING)))
            {
                con.Open();

                string sql = "DELETE FROM pending WHERE Id = " + pendingId;

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.ExecuteScalar();
                }
            }
        }
    }
}
