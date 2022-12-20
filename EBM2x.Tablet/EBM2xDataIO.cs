using Android.Widget;
using EBM2x.Droid;
using Java.IO;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(EBM2xDataIO))]
namespace EBM2x.Droid
{
    public class EBM2xDataIO : ISave
    {
        public bool IsEBM2xDataFolderPath()
        {
            return true;
        }

        public bool ExistsEBM2xDataFolderPath()
        {
            string eBM2xDataFolderPath = null;
            try
            {    
                eBM2xDataFolderPath = Android.OS.Environment.GetExternalStoragePublicDirectory("EBM2xData").AbsolutePath;
                if (Directory.Exists(eBM2xDataFolderPath)) return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public string GetEBM2xDataFolderPath()
        {
            string eBM2xDataFolderPath = null;
            try
            {
                eBM2xDataFolderPath = Android.OS.Environment.GetExternalStoragePublicDirectory("EBM2xData").AbsolutePath;
            }
            catch (Exception ex)
            {
            }
            return eBM2xDataFolderPath;
        }

        public async Task SaveAndView(string fileName, String contentType, MemoryStream stream)
        {
            string root = GetEBM2xDataFolderPath();

            Java.IO.File file = new Java.IO.File(root, fileName);
            if (file.Exists()) file.Delete();

            //Write the stream into the file
            FileOutputStream outs = new FileOutputStream(file);
            outs.Write(stream.ToArray());

            outs.Flush();
            outs.Close();
        }
        public bool IsSdCard()
        {
            Java.IO.File fi = new Java.IO.File("storage/");
            var lst = fi.ListFiles();
            foreach(var devName in lst)
            {
                if(devName.Name != "self" && devName.Name != "emulated" && devName.Name != "enc_emulated")
                {
                    return true; 
                }
            }
            return false;
        }

        public string GetSdCardPath()
        {
            Java.IO.File fi = new Java.IO.File("storage/");
            var lst = fi.ListFiles();
            foreach (var devName in lst)
            {
                if (devName.Name != "self" && devName.Name != "emulated" && devName.Name != "enc_emulated")
                {
                    return devName.Path;
                }
            }
            return "";
        }
        public void ExportDataToSdCard(string rootPath) // Folder
        {
            string targetDirectory = null;
            try
            {
                // 접근 권한이 Internal로 한정된다. 'Android/data/com.companyname.ebm2x'
                targetDirectory = Path.Combine(rootPath, "Android/data/com.companyname.ebm2x/EBM2xBackup");
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                
                if (Directory.Exists(targetDirectory))
                {
                    string localApplicationData = GetEBM2xDataFolderPath();
                    string sourceDirectory = Path.Combine(localApplicationData, "EBM2x");

                    try
                    {
                        DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                        DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

                        CopyFilesRecursively(diSource, diTarget);
                    }
                    catch (Exception e)
                    {
                        string aaa = e.Message;
                    }
                }
            }
            catch (Exception e)
            {
                string aa = e.Message;
            }
        }

        public async Task ExportData() // Folder
        {
            bool sdCard = IsSdCard();
            if (sdCard)
            {
                string rootPath = GetSdCardPath();
                ExportDataToSdCard(rootPath);
            }
        }

        public void ExportAppData() // Folder
        {
            string targetDirectory = null;
            //Get the root path in android device.
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                targetDirectory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Android/data/com.companyname.ebm2x/files");

                //// 마이크로 SD 저장 위치 : '/storage/sdcard1'
                //// 내부 저장소(UT) 위치: '/storage/sdcard0'
                //// '/sdcard' 및 '/storage/emulated/0' 도 내부 저장소(UT)를 가리 킵니다.
                //// 그러나 이것들은 '/storage/sdcard0' 대한 심볼릭 링크입니다.
                // targetDirectory = "/storage/sdcard1/ebm2x";
            }
            else
            {
                return;
            }

            string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string sourceDirectory = Path.Combine(localApplicationData, "EBM2x");


            try
            {
                DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

                CopyFilesRecursively(diSource, diTarget);
            }
            catch (Exception e)
            {
                string aaa = e.Message;
            }
        }

        public void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                if (dir.Name.Length > 2 && dir.Name.Substring(0,2).Equals("20"))
                {
                    continue;
                }

                string targetSub = Path.Combine(target.FullName, dir.Name);
                if (!Directory.Exists(targetSub))
                {
                    Directory.CreateDirectory(targetSub);
                }

                DirectoryInfo diSource = new DirectoryInfo(targetSub);
                CopyFilesRecursively(dir, diSource);
            }
            foreach (FileInfo file in source.GetFiles())
            {
                string targetFile = Path.Combine(target.FullName, file.Name);
                if (System.IO.File.Exists(file.FullName))
                {
                    System.IO.File.Copy(file.FullName, targetFile);
                }
            }
        }

        public void ExportBackupData(string targetDirectory)
        {
            string localApplicationData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            string sourceDirectory = Path.Combine(localApplicationData, "EBM2x");

            try
            {
                string targetSub = Path.Combine(targetDirectory, "EBM2x");
                if (!Directory.Exists(targetSub))
                {
                    Directory.CreateDirectory(targetSub);
                }

                DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo diTarget = new DirectoryInfo(targetSub);

                BackupFilesRecursively(diSource, diTarget);
            }
            catch (Exception e)
            {
                string aaa = e.Message;
            }
        }

        public void BackupFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                if (dir.Name.Length >= 3 && dir.Name.Substring(0, 3).Equals("202")) continue;

                Android.Widget.Toast.MakeText(Android.App.Application.Context, "proceeding", ToastLength.Short).Show();

                string targetSub = Path.Combine(target.FullName, dir.Name);
                if (!Directory.Exists(targetSub))
                {
                    Directory.CreateDirectory(targetSub);
                }

                DirectoryInfo diSource = new DirectoryInfo(targetSub);
                BackupFilesRecursively(dir, diSource);
            }
            foreach (FileInfo file in source.GetFiles())
            {
                string targetFile = Path.Combine(target.FullName, file.Name);
                if (System.IO.File.Exists(file.FullName))
                {
                    System.IO.File.Copy(file.FullName, targetFile);
                }
            }
        }

    }
}