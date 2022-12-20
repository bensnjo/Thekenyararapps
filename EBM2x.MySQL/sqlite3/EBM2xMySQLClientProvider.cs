using EBM2x.Dependency;
using EBM2x.UI;
using EBM2x.Utils;
using EBM2x.WPF.sqlite3;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows;

[assembly: Xamarin.Forms.Dependency(typeof(EBM2xMySQLClientProvider))]
namespace EBM2x.WPF.sqlite3
{
    public class EBM2xMySQLClientProvider : IEBM2xDatabaseProvider
    {
        protected MySqlConnection sqlConnection = null;
        protected MySqlTransaction sqlTransaction = null;

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
            //string server = "localhost";
            //string database = "ebm2xclient";
            //string uid = "root";
            //string pwd = "skwocjf";

            //string strConn = "Database=test;Data Source=localhost;User Id=root;Password=skwocjf";
            //string strConn = "server=127.0.0.1; uid=root; pwd=skwocjf; database=test;";
            string strConn = "server=" + server + ";database=" + database + ";uid=" + uid + ";pwd=" + pwd + ";";

            try
            {
                sqlConnection = new MySqlConnection(strConn);
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
