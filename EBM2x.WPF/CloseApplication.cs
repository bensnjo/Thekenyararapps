using EBM2x.Dependency;
using EBM2x.WPF;
using System.Windows;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace EBM2x.WPF
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            Application.Current.Shutdown(99);
        }
    }
}