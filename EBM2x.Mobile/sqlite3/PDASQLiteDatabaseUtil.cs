﻿using EBM2x.Droid.sqlite3;
using System.IO;
using System.Reflection;

[assembly: Xamarin.Forms.Dependency(typeof(PDASQLiteDatabaseUtil))]
namespace EBM2x.Droid.sqlite3
{
    public class PDASQLiteDatabaseUtil : ISQLiteDatabaseUtil
    {
        public Stream GetSQLiteDatabaseStream()
        {
            string imageName = "RRA_EBM2x.db";
            var type = typeof(PDASQLiteDatabaseUtil).GetTypeInfo();
            var assembly = type.Assembly;
            return assembly.GetManifestResourceStream($"EBM2x.Droid.sqlite3.{imageName}");
        }

        public Stream GetSQLiteDatabaseZipStream()
        {
            string imageName = "RRA_EBM2x.zip";
            var type = typeof(PDASQLiteDatabaseUtil).GetTypeInfo();
            var assembly = type.Assembly;
            return assembly.GetManifestResourceStream($"EBM2x.Droid.sqlite3.{imageName}");
        }
    }
}
