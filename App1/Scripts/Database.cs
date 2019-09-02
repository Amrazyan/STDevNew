

using System.Data.SQLite;
using Windows.Storage;
using Windows.UI.Xaml;

namespace App1.Scripts
{

    class Database
    {
        //public SQLiteConnection connection = new SQLiteConnection("ms-appx:///HereIam.db");
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
        
        public void createDB()
        {
            string createQuery = @"CREATE TABLE IF NOT EXISTS
                                [Mytable](
                                [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                [URL] NVARCHAR(2048)
                                )";
           // SQLiteConnection.CreateFile("newDatabase.db");
            using (SQLiteConnection con = new SQLiteConnection("data source=newDatabase.db"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    con.Open();
                    cmd.CommandText = createQuery;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Mytable(URL) VALUES ('TESTURLVALUE')";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void insertIntoDB(string url)
        {
            using (SQLiteConnection con = new SQLiteConnection("data source=newDatabase.db"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(con))
                {
                    con.Open();
                    cmd.CommandText = $"INSERT INTO Mytable(URL) VALUES ('{url}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
