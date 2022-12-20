using EBM2x.WPF;
using Xamarin.Forms.Platform.WPF;
using SKFormsView = SkiaSharp.Views.Forms.SKCanvasView;
using SKNativeView = SkiaSharp.Views.WPF.SKElement;

[assembly: ExportRenderer(typeof(SKFormsView), typeof(SKCanvasViewRenderer))]
namespace EBM2x.WPF
{
    public class SKCanvasViewRenderer : SKCanvasViewRendererBase<SKFormsView, SKNativeView>
    {
    }
}