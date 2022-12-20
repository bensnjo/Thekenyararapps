using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace EBM2x.WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public static AppTablet __AppTablet = null;

        public MainWindow()
        {
            InitializeComponent();

            Forms.Init();

            // 2021.01 Mobile
            __AppTablet = new AppTablet(true, true, false);
            LoadApplication(__AppTablet);  //new EBM2x.AppBackoffice()
        }

        private void FormsApplicationPage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                    MessagingCenter.Send<Object, string>(this, "It was entered Number", e.Key.ToString().Replace("D", ""));
                    break;
                case Key.Back:
                    MessagingCenter.Send<Object, string>(this, "It was entered Function", "Backspace");
                    break;
                case Key.Enter:
                    MessagingCenter.Send<Object, string>(this, "It was entered Function", "Enter");
                    break;
                default:
                    MessagingCenter.Send<Object, string>(this, "It was entered Function", e.Key.ToString());
                    break;
            }
        }
    }
}
