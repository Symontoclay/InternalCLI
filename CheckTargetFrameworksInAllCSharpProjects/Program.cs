﻿using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.TargetFrameworks.CheckTargetFrameworksInAllCSharpProjects;
using NLog;

namespace CheckTargetFrameworksInAllCSharpProjects
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DeploymentPipeline.Run(new CheckTargetFrameworksInAllCSharpProjectsDevTask());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Info($"e.ExceptionObject = {e.ExceptionObject}");
        }
    }
}