

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
        private string tableName = "Mytable";

        private Database()
        {
            if (!File.Exists(databaseName))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(databaseName);
            }
            createDB();
        }

        private void createDB()
        {
            using (SQLiteConnection con = new SQLiteConnection($"data source={databaseName}"))
            {
                con.Open();
                string createQuery = $@"CREATE TABLE IF NOT EXISTS
                                    [{tableName}](
                                    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                    [URL] NVARCHAR(2048),
                                    [IsReachable] INTEGER,
                                    [ResponseTime] FLOAT
                                    )";
      
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = createQuery;
                    cmd.ExecuteNonQuery();                
                }
            }

        }


        public void insertIntoDB(Block block, bool isReachable)
        {
            using (SQLiteConnection con = new SQLiteConnection($"data source={databaseName}"))
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = $"INSERT INTO {tableName}(URL,IsReachable,ResponseTime) VALUES ('{block.URL}','{Convert.ToInt32(isReachable)}','{block.TimeTaken}')";
                    cmd.ExecuteNonQuery();
                 
                    cmd.CommandText = "SELECT last_insert_rowid()";

                    block.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    
                }
            }

        }
        public List<UrlDataModel> getAllData()
        {
            using (SQLiteConnection con = new SQLiteConnection($"data source={databaseName}"))
            {
                con.Open();
                var output = con.Query<UrlDataModel>($"SELECT * FROM {tableName}", new DynamicParameters());

                return output.ToList();
            }

        }

        public void deleteById(int id)
        {
            using (SQLiteConnection con = new SQLiteConnection($"data source={databaseName}"))
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    cmd.CommandText = $"DELETE FROM {tableName} WHERE Id = {id}";
                    cmd.ExecuteNonQuery();
                }   
            }

        }

    }
}
