using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xamarin.Forms;

namespace EBM2x.Droid
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/eTIMS_logo", Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected static AppTablet __AppTablet = null;
        
        //@PWB  20200721 <=권한 체크 
        List<string> checkPermissions = new List<string>();     //권한 검사 항목 
        List<string> permissions = new List<string>();          //권한 요청 항목 
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //@PWB  20200721 <=필요권한 list
            checkPermissions.Add(Manifest.Permission.ReadExternalStorage);
            checkPermissions.Add(Manifest.Permission.WriteExternalStorage);
            checkPermissions.Add(Manifest.Permission.SendSms);

            //@PWB  20200721 <= 권한 확인 및 요청
            foreach (var checkPermission in checkPermissions)
            {
                if (ContextCompat.CheckSelfPermission(this, checkPermission) != (int)Permission.Granted)
                {
                    // Permission 설정이 않된 경우 
                    permissions.Add(checkPermission);
                }
            }
            
            //@PWB  20200721 <=권한 중복 요청설정시 오류 발생 
            if (permissions.Count > 0)
            {
                ActivityCompat.RequestPermissions(this, permissions.ToArray(), 1);
                Thread.Sleep(5000);

                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permission");
                alert.SetMessage("Read/Write External Storage");
                alert.SetPositiveButton("OK", (senderAlert, args) => {
                    try
                    {
                        EBM2xDataIO eBM2xDataIO = new EBM2xDataIO();
                        string targetDirectory = eBM2xDataIO.GetEBM2xDataFolderPath();
                        if (!Directory.Exists(targetDirectory))
                        {
                            Directory.CreateDirectory(targetDirectory);
                            eBM2xDataIO.ExportBackupData(targetDirectory);
                        }
                    }
                    catch (Exception ex)
                    {
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }

                    if (__AppTablet == null)
                    {
                        // 2021.01 Mobile
                        __AppTablet = new AppTablet(false, false, false);
                    }

                    LoadApplication(__AppTablet);
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                try
                {
                    EBM2xDataIO eBM2xDataIO = new EBM2xDataIO();
                    string targetDirectory = eBM2xDataIO.GetEBM2xDataFolderPath();
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                        eBM2xDataIO.ExportBackupData(targetDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                }

                if (__AppTablet == null)
                {
                    // 2021.01 Mobile
                    __AppTablet = new AppTablet(false, false, false);
                }

                LoadApplication(__AppTablet);

            }
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            switch (keyCode)
            {
                case Keycode.Num0:
                case Keycode.Num1:
                case Keycode.Num2:
                case Keycode.Num3:
                case Keycode.Num4:
                case Keycode.Num5:
                case Keycode.Num6:
                case Keycode.Num7:
                case Keycode.Num8:
                case Keycode.Num9:
                    MessagingCenter.Send<Object, string>(this, "It was entered Number", keyCode.ToString().Replace("Num", ""));
                    return true;
                case Keycode.Enter:
                    MessagingCenter.Send<Object, string>(this, "It was entered Function", "Enter");
                    return true;
                default:
                    //MessagingCenter.Send<Object, string>(this, "It was entered Function", keyCode.ToString());
                    return true;
            }

            // return base.OnKeyDown(keyCode, e);
        }
    }
}