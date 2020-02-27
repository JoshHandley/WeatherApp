using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp
{
    public interface IDBInterface
    {
        SQLiteConnection CreateConnection();
    }
}
