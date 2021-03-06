using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AdonisUI.Controls;
using BikeController.Core.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Microsoft.VisualBasic;
using HackUTDBikeQueryApp.Core.Util;

namespace BikeController.Core.ViewModels
{
    public class BikeControllerViewModel : MvxViewModel
    {
        // These Enablers allow us to dynamically enable and disable controls depending on the given conditions
        #region Enablers
        public bool CanQuery => CurrentBike != null;
        #endregion

        // Commands are bound to controls within the .xaml to execute code on use of said controls
        #region Command Properties
        public IMvxCommand AcceptCommand { get; set; }

        public IMvxCommand DenyCommand { get; set; }

        public IMvxCommand QueryCommand { get; set; }
        #endregion

        public Queue<BikeLocationModel> Bikes = new Queue<BikeLocationModel>();

        public BikeLocationModel CurrentBike
        {
            get
            {
                if (Bikes.Count == 0) return null;
                return Bikes.Peek();
            }
        }
        
        public int BikeCount => Bikes.Count;

        public BikeControllerViewModel()
        {
            // Linking Commands
            AcceptCommand = new MvxCommand(Accept);
            DenyCommand = new MvxCommand(Deny);
            QueryCommand = new MvxCommand(Query);

            // an initial query always occurs on opening the program
            Query();
        }

        #region Command Methods
        public void Accept()
        {
            // Dequeues and grabs Id for sql command
            NpgsqlUtil.MovePendingToLocation(Bikes.Dequeue().Id);

            BikeListUpdated();
        }

        public void Deny()
        {
            // Dequeues and grabs Id for sql command
            NpgsqlUtil.RemovePending(Bikes.Dequeue().Id);

            BikeListUpdated();
        }

        public void Query()
        {
            // Updates the Bike list to what's currently in the database
            Bikes = NpgsqlUtil.QueryBikeList();

            BikeListUpdated();
        }
        #endregion

        private void BikeListUpdated()
        {
            MapUtil.UpdateBikeMap?.Invoke();

            RaisePropertyChanged(() => CanQuery);
            RaisePropertyChanged(() => CurrentBike);
            RaisePropertyChanged(() => BikeCount);
        }
    }
}
