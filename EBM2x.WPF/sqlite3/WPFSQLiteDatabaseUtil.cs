using EBM2x.WPF.sqlite3;
using System.IO;
using System.Reflection;

[assembly: Xamarin.Forms.Dependency(typeof(WPFSQLiteDatabaseUtil))]
namespace EBM2x.WPF.sqlite3
{
    public class WPFSQLiteDatabaseUtil : ISQLiteDatabaseUtil
    {
        public Stream GetSQLiteDatabaseStream()
        {
            string imageName = "RRA_EBM2x.db";
            var type = typeof(WPFSQLiteDatabaseUtil).GetTypeInfo();
            var assembly = type.Assembly;
            return assembly.GetManifestResourceStream($"EBM2x.WPF.sqlite3.{imageName}");
        }

        public Stream GetSQLiteDatabaseZipStream()
        {
            string imageName = "RRA_EBM2x.zip";
            var type = typeof(WPFSQLiteDatabaseUtil).GetTypeInfo();
            var assembly = type.Assembly;
            return assembly.GetManifestResourceStream($"EBM2x.WPF.sqlite3.{imageName}");
        }
        
    }
}
