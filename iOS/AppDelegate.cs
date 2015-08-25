using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using XLabs.Ioc;
using XLabs.Platform.Services.IO;
using Xamarin;
using System.Net;

namespace TechSocial.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) =>
            {
                if (cert != null)
                    System.Diagnostics.Debug.WriteLine(cert);
                return true;
            };
            
            var resolverContainer = new SimpleContainer();
            resolverContainer.Register<IFileAccess>(t => new FIleAccess()); // maybe just this line
            Resolver.SetResolver(resolverContainer.GetResolver());

            global::Xamarin.Forms.Forms.Init();
            Insights.Initialize("40003b61dcd00a7e797505eadf0c2fc2f584475c");

            App.ScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

