using EBM2x.Dependency;
using EBM2x.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace EBM2x.Droid
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}