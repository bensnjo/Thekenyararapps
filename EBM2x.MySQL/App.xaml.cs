using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EBM2x.WPF
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        Mutex _mutex = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            string mutexName = "EBM2x.WPF";
            bool isCreatedNew = false;
            try
            {
                _mutex = new Mutex(true, mutexName, out isCreatedNew);
                if (isCreatedNew)
                {
                    base.OnStartup(e);
                }
                else
                {
                    MessageBox.Show("Application already started.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + "Application Existing...", "Exception thrown");
                Application.Current.Shutdown();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            EBM2x.WPF.MainWindow.__AppTablet.StopListening();

            MessageBox.Show("Application Exit.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
