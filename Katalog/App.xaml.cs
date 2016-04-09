using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Katalog
{
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.UnhandledException += HandleException;
        }
        
        private void HandleException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "ERRORLOG" + DateTime.Now.GetBackupFileName(".txt"));
                var data = $"Error log: {e.Exception.GetType().Name}\r\n" +
                           $"Message\r\n" +
                           $"{e.Exception.Message}\r\n" +
                           $"Inner message\r\n" +
                           $"{e.Exception.InnerException?.Message}\r\n" +
                           $"Inner inner message\r\n" +
                           $"{e.Exception.InnerException?.InnerException?.Message}\r\n" +
                           $"Stacktrace\r\n" +
                           $"{e.Exception.StackTrace}\r\n" +
                           $"Inner stacktrace\r\n" +
                           $"{e.Exception.InnerException?.StackTrace}\r\n" +
                           $"Inner inner stacktrace\r\n" +
                           $"{e.Exception.InnerException?.InnerException?.StackTrace}";
                File.WriteAllText(path,data);
            }
            catch (Exception)
            {
            }
        }
    }
}
