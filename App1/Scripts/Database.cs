

using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using System.IO;

namespace App1.Scripts
{

    class Database
    {
        private SQLiteConnection con;

        private static Database _instance;

        public static Database Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new Database();
                }

                return _instance;
            }
        }

        private string databaseName = "newDatabase.db";

        private Database()
        {
            if (!File.Exists(databaseName))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(databaseName);
            }

            con = new SQLiteConnection($"data source={databaseName}");
            con.Open();
        }

        public void createDB()
        {
            string createQuery = @"CREATE TABLE IF NOT EXISTS
                                [Mytable](
                                [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                [URL] NVARCHAR(2048),
                                [IsReachable] INTEGER,
                                [ResponseTime] FLOAT
                                )";
      
            using (SQLiteCommand cmd = new SQLiteCommand(con))
            {
                cmd.CommandText = createQuery;
                cmd.ExecuteNonQuery();
                //cmd.CommandText = "INSERT INTO Mytable(URL) VALUES ('TESTURLVALUE')";
                //cmd.ExecuteNonQuery();
            }
        }


        public void insertIntoDB(Block block, bool isReachable)
        {
            {
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = $"INSERT INTO Mytable(URL,IsReachable,ResponseTime) VALUES ('{block.URL}','{Convert.ToInt32(isReachable)}','{block.TimeTaken}')";
                    cmd.ExecuteNonQuery();
                 
                    cmd.CommandText = "SELECT last_insert_rowid()";

                    block.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    
                }
            }
        }
        public List<UrlDataModel> getAllData()
        {

            var output = con.Query<UrlDataModel>("SELECT * FROM Mytable", new DynamicParameters());

            return output.ToList();
        }

        public void deleteById(int id)
        {

            using (SQLiteCommand cmd = new SQLiteCommand(con))
            {
                cmd.CommandText = $"DELETE FROM Mytable WHERE Id = {id}";
                cmd.ExecuteNonQuery();
            }                
        }

        ~Database()
        {
            con.Dispose();
        }
    }
}
