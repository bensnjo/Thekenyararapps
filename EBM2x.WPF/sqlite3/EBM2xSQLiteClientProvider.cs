using EBM2x.Dependency;
using EBM2x.UI;
using EBM2x.Utils;
using EBM2x.WPF.sqlite3;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;

[assembly: Xamarin.Forms.Dependency(typeof(EBM2xSQLiteClientProvider))]
namespace EBM2x.WPF.sqlite3
{
    public class EBM2xSQLiteClientProvider : IEBM2xDatabaseProvider
    {
        protected SQLiteConnection sqlConnection = null;
        protected SQLiteTransaction sqlTransaction = null;

        public string GetDatabaseName(string name)
        {
            string directory = "sqlite";
            string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string pathName = Path.Combine(localApplicationData, "EBM2x/" + directory);
            if (!Directory.Exists(pathName))
            {
                Directory.CreateDirectory(pathName);
            }

            string fileName = Path.Combine(pathName, name);

            return fileName;
        }

        public void OpenConnection(string server, string database, string uid, string pwd)
        {
            if (Connected())
            {
                if (sqlConnection != null) return;
                CloseConnection();
            }

            string databasenameStr = GetDatabaseName("RRA_EBM2x.db");
            string strConn = "Data Source=" + databasenameStr + "; Version=3" + ";" + " Password=" + "EBMCLIENT-20170301-GAION" + ";";
            //string strConn = "Data Source=" + databasenameStr + "; Version=3;";

            try
            {
                sqlConnection = new SQLiteConnection(strConn);
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                LogWriter.ErrorLog(ex.ToString());
                sqlConnection = null;

                MessageBox.Show("DB connection failed. Exit the application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                System.Windows.Application.Current.Shutdown();
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            catch (Exception ex)
            {
                LogWriter.ErrorLog(ex.ToString());
            }
            sqlConnection = null;
        }

        public void Reconnect()
        {
            CloseConnection();
            if (UIManager.Instance().IsWindows && UIManager.Instance().IsMySQL)
            {
                string DBServer = UIManager.Instance().PosModel.Environment.EnvPosSetup.MySQLServer;
                string Database = UIManager.Instance().PosModel.Environment.EnvPosSetup.MySQLDatabase;
                string DBUid = UIManager.Instance().PosModel.Environment.EnvPosSetup.MySQLUid;
                string DBPwd = UIManager.Instance().PosModel.Environment.EnvPosSetup.MySQLPwd;
                OpenConnection(DBServer, Database, DBUid, DBPwd);
            }
            else
            {
                OpenConnection("", "", "", "");
            }
        }

        public bool Connected()
        {
            if (sqlConnection != null && (int)sqlConnection.State == (int)ConnectionState.Open) return true;
            return false;
        }

        public IDbCommand GetDbCommand()
        {
            if (!Connected() || sqlConnection == null ) return null;
            
            try
            {
                return sqlConnection.CreateCommand();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void TransactionStart()
        {
            if (!Connected()) return;

            // only begin transaction when there is no transaction
            try
            {
                if (sqlTransaction == null)
                {
                    sqlTransaction = sqlConnection.BeginTransaction();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void TransactionCommit()
        {
            if (!Connected()) return;
            if (sqlTransaction == null) return;
            try
            {
                sqlTransaction.Commit();
                sqlTransaction.Dispose();
                sqlTransaction = null;
            }
            catch (Exception ex)
            {
            }
        }

        public void TransactionRollback()
        {
            if (!Connected()) return;
            if (sqlTransaction == null) return;

            try
            {
                sqlTransaction.Rollback();
                sqlTransaction.Dispose();
                sqlTransaction = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
