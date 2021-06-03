using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace loggingsample
{
    public static class LoggingHelper
    {
        private static async Task<StorageFolder> GetLogsFolder(string logFolder)
        {
            var storageFolder = ApplicationData.Current.LocalCacheFolder;
            var logsFolder = await storageFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists);
            var netLogsFolder = await logsFolder.CreateFolderAsync(logFolder, CreationCollisionOption.OpenIfExists);
            return netLogsFolder;
        }

        public static async Task InitializeNLogAsync(string logName, string logFolder = "Uwp")
        {
            var logsFolder = await GetLogsFolder(logFolder);
            var logsFilePath = Path.Combine(logsFolder.Path, $"{logName}.log");
            var logsFilePathRolling = Path.Combine(logsFolder.Path, $"{logName}.{{#}}.log");

            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = logsFilePath,
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveDateFormat = "yyyyMMdd",
                ArchiveFileKind = FilePathKind.Absolute,
                ArchiveFileName = logsFilePathRolling,
                MaxArchiveDays = 7,
                Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=message}"
            };


            config.AddRuleForAllLevels(logfile);

            NLog.LogManager.Configuration = config;
        }
    }

}
