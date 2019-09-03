

using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System;
using Windows.Storage;
using Windows.UI.Xaml;

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
        private Database()
        {
            con = new SQLiteConnection("data source=newDatabase.db");
        }

        public void createDB()
        {
            string createQuery = @"CREATE TABLE IF NOT EXISTS
                                [Mytable](
                                [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                [URL] NVARCHAR(2048)
                                )";
           // SQLiteConnection.CreateFile("newDatabase.db");
           // using (SQLiteConnection con = new SQLiteConnection("data source=newDatabase.db"))
           // {
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    con.Open();
                    cmd.CommandText = createQuery;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Mytable(URL) VALUES ('TESTURLVALUE')";
                    cmd.ExecuteNonQuery();
                }
           // }
        }
        public void insertIntoDB(string url,bool isReachable)
        {
            using (SQLiteConnection con = new SQLiteConnection("data source=newDatabase.db"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    con.Open();
                    cmd.CommandText = $"INSERT INTO Mytable(URL,IsReachable) VALUES ('{url}','{Convert.ToInt32(isReachable)}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<UrlDataModel> getAllData()
        {      
            using (SQLiteConnection con = new SQLiteConnection("data source=newDatabase.db"))
            {
                var output = con.Query<UrlDataModel>("SELECT * FROM Mytable", new DynamicParameters());

                return output.ToList();
            }

        }

        ~Database()
        {
            con.Dispose();
        }
    }
}
