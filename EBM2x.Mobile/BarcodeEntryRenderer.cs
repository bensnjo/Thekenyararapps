using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using EBM2x.Droid;
using EBM2x.UI.EntryPanel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BarcodeEntry), typeof(BarcodeEntryRenderer))]
namespace EBM2x.Droid
{
    public class BarcodeEntryRenderer : EntryRenderer
    {
        public BarcodeEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetShape(ShapeType.Rectangle);
                gd.SetStroke(1, global::Android.Graphics.Color.Black);
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                Control.SetBackground(gd);
                Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));

                Control.SetPadding(0, 0, 0, 0);

                var nativeEditText = (global::Android.Widget.EditText)Control;
                nativeEditText.TextSize = 36f;
            }
        }
    }
}
