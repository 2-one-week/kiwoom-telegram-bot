using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Kiwoom.DB
{
    
    class SqliteDB
    {
        private string dbName = "userdata.sqlite";
        public SQLiteConnection conn = null;

        public void ConnectAndInitDB()
        {
            FileInfo fileInfo = new FileInfo(Application.StartupPath + @"\" + dbName);
            try
            {
                if (!fileInfo.Exists)
                {
                    SQLiteConnection.CreateFile(Application.StartupPath + @"\" + dbName);
                }

                conn = new SQLiteConnection("Data Source=" + Application.StartupPath + @"\" + dbName + ";Version=3;");
                conn.Open();

                //create & init db
                string createTelegramApi = "CREATE TABLE IF NOT EXISTS stock (id INTEGER, api TEXT); ";
  
                SQLiteCommand cmd = new SQLiteCommand(createTelegramApi, conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 조회 도중 오류가 발생하였습니다.\n" + ex.Message);
                return;
            }
        }

        public void readDataFromDB()
        {

        }

        public void insertDataToDB(long code)
        {

        }

        public void deleteDataFromDB(long code) 
        {

        }
    }
}
