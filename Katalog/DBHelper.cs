using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Settings = Katalog.Properties.Settings;

namespace Katalog
{
    public static class DBHelper
    {
        private static event PercentCompleteEventHandler PercentDone;
        private static event ServerMessageEventHandler Completed;

        public static void Backup()
        {
            var name = "KatalogDatabase";
            var dir = new FileInfo(Application.ResourceAssembly.Location).Directory.FullName;
            var p = Settings.Default;
            var conn = new ServerConnection(new SqlConnection(p.KatalogDatabaseConnectionString));
            var srv = new Server(conn);
            try
            {

                var backup = new Backup
                {
                    Action = BackupActionType.Database,
                    Initialize = true,
                    Database = Path.Combine(dir, name+".mdf").ToUpper(),
                    ExpirationDate = DateTime.Now.AddDays(14),
                    BackupSetName = name,
                    BackupSetDescription = "Katalog full backup"
                };
                backup.Devices.AddDevice(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        DateTime.Now.GetBackupFileName()), DeviceType.File);
                backup.PercentComplete += PercentDone;
                backup.Complete += Completed;
                backup.SqlBackup(srv);
            }
            catch
            {

            }
            finally
            {
                srv.ConnectionContext.Disconnect();
            }
            //db.ExecuteSqlCommand($"BACKUP DATABASE KatalogDatabase TO DISK='{path}' WITH FORMAT, MEDIANAME='DbBackups', MEDIADESCRIPTION='Media set for KatalogDatabase database';");
            
        }
        public static void Restore(string source, string dbname, string path)
        {
            var name = "KatalogDatabase";
            var dir = new FileInfo(Application.ResourceAssembly.Location).Directory.FullName;
            var p = Settings.Default;
            var conn = new ServerConnection(new SqlConnection(p.KatalogDatabaseConnectionString));
            var srv = new Server(conn);
            srv.BackupDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                var backup = new Backup
                {
                    Action = BackupActionType.Database,
                    Initialize = true,
                    Database = Path.Combine(dir, name + ".mdf").ToUpper(),
                    ExpirationDate = DateTime.Now.AddDays(14),
                    BackupSetName = name,
                    BackupSetDescription = "Katalog full backup"
                };
                backup.Devices.AddDevice(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "ROLLBACK" + DateTime.Now.GetBackupFileName()), DeviceType.File);
                backup.PercentComplete += PercentDone;
                backup.Complete += Completed;

                backup.SqlBackup(srv);

            }
            catch
            {
            }
            try
            {
                var res = new Restore();
                res.Action = RestoreActionType.Database;
                res.Devices.AddDevice(path, DeviceType.File);

                //var fileList = res.ReadFileList(srv);
                //var DataFile = new RelocateFile(fileList.Rows[0][0].ToString(), conn);
                //var LogFile = new RelocateFile(fileList.Rows[1][0].ToString(), Path.Combine(dir, name+"_log.ldf"));
                //var MDF = fileList.Rows[0][1].ToString();
                //var LDF = fileList.Rows[1][1].ToString();

                ////DataFile.PhysicalFileName = srv.Databases[name].FileGroups[0].Files[0].FileName;
                
                ////LogFile.PhysicalFileName = srv.Databases[name].LogFiles[0].FileName;

                //res.RelocateFiles.Add(DataFile);
                //res.RelocateFiles.Add(LogFile);

                res.Database = name;
                res.NoRecovery = false;
                res.ReplaceDatabase = true;
                res.SqlRestore(srv);


                //db.ExecuteSqlCommand($"BACKUP DATABASE KatalogDatabase TO DISK='{path}' WITH FORMAT, MEDIANAME='DbBackups', MEDIADESCRIPTION='Media set for KatalogDatabase database';");

            }
            catch(Exception ex)
            {
            }
            conn.Disconnect();
        }
    }
}