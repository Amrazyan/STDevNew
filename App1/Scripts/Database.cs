

using System.Data.SQLite;
using Windows.Storage;
using Windows.UI.Xaml;

namespace App1.Scripts
{

    class Database
    {
        //public SQLiteConnection connection = new SQLiteConnection("ms-appx:///HereIam.db");
        
        public Database()
        {
            
        }

        public void createDB()
        {
            string createQuery = @"CREATE TABLE IF NOT EXISTS
                                [Mytable](
                                [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                [URL] NVARCHAR(2048)
                                )";
            SQLiteConnection.CreateFile("newDatabase.db");
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
    }
}
