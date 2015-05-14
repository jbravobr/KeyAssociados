using System;
using SQLite.Net;

namespace TechSocial
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}

