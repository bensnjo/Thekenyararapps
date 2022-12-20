using EBM2x.WPF;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(EBM2xDataIO))]
namespace EBM2x.WPF
{
    public class EBM2xDataIO : ISave
    {
        public bool IsEBM2xDataFolderPath()
        {
            return false;
        }
        public bool ExistsEBM2xDataFolderPath()
        {
            return false;
        }
        public string GetEBM2xDataFolderPath()
        {
            string eBM2xDataFolderPath = null;
            eBM2xDataFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            return eBM2xDataFolderPath;
        }

        public async Task SaveAndView(string filename, string contentType, MemoryStream stream)
        {
            if (!Directory.Exists(@"C:\Temp\"))
            {
                Directory.CreateDirectory(@"C:\Temp\");
            }

            using (FileStream file = new FileStream(@"C:\Temp\" + filename, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }

        public bool IsSdCard()
        {
            return false;
        }

        public async Task ExportData()
        {

        }
    }
}