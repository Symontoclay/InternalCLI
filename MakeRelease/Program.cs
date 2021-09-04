using Deployment.ReleaseTasks.MakeRelease;
using NLog;
using System;

namespace MakeRelease
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Console.WriteLine("This app will release future version. Are you sure?");
            Console.WriteLine("Press 'y' or 'Y' for release or other else key for cancel release.");
            Console.WriteLine("After your choise press enter.");

            var key = Console.ReadLine();

            if(key != "y" && key != "Y")
            {
                Console.WriteLine("Release has been cancelled.");

                return;
            }

            Console.WriteLine("Release has been started.");

            var task = new MakeReleaseReleaseTask();
            task.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}
