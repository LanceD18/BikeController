using System;
using System.Collections.Generic;
using System.Text;
using BikeController.Core.ViewModels;
using MvvmCross.ViewModels;

namespace BikeController.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<BikeControllerViewModel>();
        }
    }
}
