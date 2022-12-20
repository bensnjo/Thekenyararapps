using Android.App;
using Android.Content;
using Xamarin.Forms;
using Android.Webkit;
using EBM2x.Droid;
using EBM2x.Dependency;

[assembly: Dependency(typeof(DownloadProcessorTablet))]
namespace EBM2x.Droid
{
    public class DownloadProcessorTablet : IDownloadProcessor
    {
        public void DownloadFromUrl(string url)
        {
            string cookie = CookieManager.Instance.GetCookie(url);
            var source = Android.Net.Uri.Parse(url);
            DownloadManager.Request request = new DownloadManager.Request(source);
            request.AddRequestHeader("Cookie", cookie);
            request.AllowScanningByMediaScanner();
            request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            request.SetDestinationInExternalFilesDir(Forms.Context, Android.OS.Environment.DirectoryDownloads, source.LastPathSegment);
            DownloadManager dm = (DownloadManager)Android.App.Application.Context.GetSystemService(Context.DownloadService);
            dm.Enqueue(request);
        }
    }
}