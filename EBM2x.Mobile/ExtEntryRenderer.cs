using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using EBM2x.Droid;
using EBM2x.UI.EntryPanel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AndroidEntry), typeof(ExtEntryRenderer))]
namespace EBM2x.Droid
{
    public class ExtEntryRenderer : EntryRenderer
    {
        public ExtEntryRenderer(Context context) : base(context)
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
            }
        }

        protected void OnElementChangedOld(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (AndroidEntry)Element;
                // creating gradient drawable for the curved background  
                var _gradientBackground = new GradientDrawable();
                _gradientBackground.SetShape(ShapeType.Rectangle);
                _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                // Thickness of the stroke line  
                _gradientBackground.SetStroke(2, global::Android.Graphics.Color.Black);

                // Radius for the curves  
                _gradientBackground.SetCornerRadius(15);

                // set the background of the   
                Control.SetBackground(_gradientBackground);
            }
        }
    }
}
