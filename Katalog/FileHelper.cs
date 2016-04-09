using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Katalog
{
    public static class FileHelper
    {
        public static void ZipBackups()
        {
            var desktop = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            var backups = desktop.EnumerateFiles("*.bak", SearchOption.TopDirectoryOnly);
            if(backups.Count()<10) return;
            var packageDir = desktop.CreateSubdirectory("BackupPackage");
            backups.Foreach(x=>x.MoveTo(Path.Combine(packageDir.FullName, x.Name)));
            ZipFile.CreateFromDirectory(packageDir.FullName,
                Path.Combine(desktop.FullName, "Backups" + DateTime.Now.GetBackupFileName(".zip")));
            packageDir.Delete(true);
        }
    }
}