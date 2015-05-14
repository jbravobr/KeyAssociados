using System;
using Xamarin.Forms;
using TechSocial;
using System.IO;
using TechSocial.iOS;

[assembly: Dependency(typeof(SQLite_IOS))]

namespace TechSocial.iOS
{
    public class SQLite_IOS : ISQLite
    {
        public SQLite_IOS()
        {
        }

        public SQLite.Net.SQLiteConnection GetConnection()
        {
            const string sqliteFilename = "techsocial.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            // Create the connection
            var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var conn = new SQLite.Net.SQLiteConnection(plat, path);
            // Return the database connection
            return conn;
        }
    }
}

