using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BikeController.Core.Models
{
    // TODO Make me into a struct
    public class BikeLocationModel
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; } // 64 base byte data

        public BikeLocationModel(params object[] args) // for use when reading from database
        {
            Id = (int)args[0];
            Latitude = (double)args[1];
            Longitude = (double)args[2];
            Description = (string)args[3];
            Photo = (string)args[4];
        }
    }

    //? Schemas:
    /*
     * CREATE TABLE IF NOT EXISTS pending (
    id SERIAL PRIMARY KEY,
    latitude FLOAT,
    longitude FLOAT,
    description VARCHAR,
    photo VARCHAR
    );

    CREATE TABLE IF NOT EXISTS locations (
    id SERIAL PRIMARY KEY,
    latitude FLOAT,
    longitude FLOAT,
    description VARCHAR,
    photo VARCHAR
    );
    */
}
