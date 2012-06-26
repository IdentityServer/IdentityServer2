/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Thinktecture.IdentityServer.Repositories.WindowsAzure;
using System.Linq;

namespace Thinktecture.IdentityServer.Web
{
    public class EntryPoint : RoleEntryPoint
    {
        private const string DiagnosticStorage       = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";
        private const string StorageConnectionString = "StorageConnectionString";

        public override bool OnStart()
        {
            try
            {
                RoleEnvironment.Changing += RoleEnvironment_Changing;
                ConfigureDiagnosticMonitoring();

                return base.OnStart();
            }
            catch (Exception ex)
            {
                WriteExceptionToBlobStorage(ex);
                return false;
            }
        }

        void RoleEnvironment_Changing(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(c => c is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }

        // adjust these settings to your needs
        private void ConfigureDiagnosticMonitoring()
        {
            var standardTransfer = TimeSpan.FromMinutes(60);
            var pollInterval = TimeSpan.FromMinutes(10);

            // retrieve standard config
            var config = DiagnosticMonitor.GetDefaultInitialConfiguration();

            // check for config changes
            config.ConfigurationChangePollInterval = pollInterval;

            // traces transfer
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            config.Logs.ScheduledTransferPeriod = standardTransfer;

            // infrastructure traces
            config.DiagnosticInfrastructureLogs.ScheduledTransferLogLevelFilter = LogLevel.Error;
            config.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = standardTransfer;

            // automatic transfer of application event log
            config.WindowsEventLog.DataSources.Add("Application!*");
            config.WindowsEventLog.DataSources.Add("System!*");
            config.WindowsEventLog.ScheduledTransferLogLevelFilter = LogLevel.Warning;
            config.WindowsEventLog.ScheduledTransferPeriod = standardTransfer;

            // automatic transfer of CPU load
            var procTime = new PerformanceCounterConfiguration
            {
                CounterSpecifier = @"\Processor(*)\% Processor Time",
                SampleRate = TimeSpan.FromMinutes(1)
            };

            config.PerformanceCounters.DataSources.Add(procTime);
            config.PerformanceCounters.ScheduledTransferPeriod = standardTransfer;

            // send config to monitor
            DiagnosticMonitor.Start(DiagnosticStorage, config);
        }

        private void WriteExceptionToBlobStorage(Exception ex)
        {
            var storageAccount = CloudStorageAccount.Parse(
                RoleEnvironment.GetConfigurationSettingValue(DiagnosticStorage));

            var container = storageAccount.CreateCloudBlobClient().GetContainerReference("exceptions");
            container.CreateIfNotExist();

            var blob = container.GetBlobReference(string.Format("exception-{0}-{1}.log", RoleEnvironment.CurrentRoleInstance.Id, DateTime.UtcNow.Ticks));
            blob.UploadText(ex.ToString());
        }
    }
}