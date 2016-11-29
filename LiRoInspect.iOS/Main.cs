using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Foundation;
using UIKit;
using System.Diagnostics;

namespace LiRoInspect.iOS
{
    public class Application
    {
		
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
				AppDomain.CurrentDomain.UnhandledException+= AppDomain_CurrentDomain_UnhandledException;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in Main method due to " + ex.Message);
            }
        }

        static void AppDomain_CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
        {
			var ex = (Exception)e.ExceptionObject;
			Debug.WriteLine("Exception Occured in AppDomain_CurrentDomain_UnhandledException method due to " + ex.Message);
        }
    }
}