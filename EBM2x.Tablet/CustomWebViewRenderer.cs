using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Webkit;
using EBM2x.Droid;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(CustomWebViewRenderer))]
namespace EBM2x.Droid
{
    public class CustomWebViewRenderer : ViewRenderer<Xamarin.Forms.WebView, global::Android.Webkit.WebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var webView = new global::Android.Webkit.WebView(this.Context);
                webView.SetWebViewClient(new WebViewClient());
                webView.SetWebChromeClient(new WebChromeClient());
                webView.SetDownloadListener(new CustomDownloadListener());
                this.SetNativeControl(webView);
                var source = e.NewElement.Source as UrlWebViewSource;
                if (source != null)
                {
                    webView.LoadUrl(source.Url);
                }
            }
        }
    }

    public class CustomDownloadListener : Java.Lang.Object, IDownloadListener
    {
        public void OnDownloadStart(string url, string userAgent, string contentDisposition, string mimetype, long contentLength)
        {
            DownloadManager.Request request = new DownloadManager.Request(Android.Net.Uri.Parse(url));
            request.AllowScanningByMediaScanner();
            request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            request.SetDestinationInExternalFilesDir(Forms.Context, Android.OS.Environment.DirectoryDownloads, "download");
            DownloadManager dm = (DownloadManager)Android.App.Application.Context.GetSystemService(Android.App.Application.DownloadService);
            dm.Enqueue(request);
        }
    }
}