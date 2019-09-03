

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
            con.Open();
        }
        //public void addColumn()
        //{
        //    using (SQLiteConnection con = new SQLiteConnection("data source=newDatabase.db"))
        //    {
        //        using (SQLiteCommand cmd = new SQLiteCommand(con))
        //        {
        //            con.Open();
        //            //cmd.CommandText = $"ALTER TABLE Mytable ADD COLUMN ResponseTime float";
        //            cmd.CommandText = $"ALTER TABLE Mytable DROP COLUMN ResponseTime;";
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
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
