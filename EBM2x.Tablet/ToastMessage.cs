using Android.Widget;
using EBM2x.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace EBM2x.Droid
{
    public class ToastMessage : IToast
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}