using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace EBM2x.WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// https://nsinc.tistory.com/182?category=669772 : [Xamarin.Android] Binding Java Library
    /// //"C:\Users\jcna2\AppData\Local"
    /// 
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();

            Forms.Init();
            // 2021.01 Mobile
            LoadApplication(new AppTablet(true, false, false));  //AppTablet

            // 2021.01 Mobile
            //LoadApplication(new AppPhone(false, true));  //AppPhone
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
