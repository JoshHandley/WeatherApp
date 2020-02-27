using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Location>().Wait();
        }

        public Task<List<Location>> GetPeopleAsync()
        {
            return _database.Table<Location>().ToListAsync();
        }

        public Task<int> SaveLocationAsync(Location location)
        {
            return _database.InsertAsync(location);
        }

        public Task<int> DeleteLocationAsync(Location location)
        {
            return _database.DeleteAsync(location);
        }
    }
}
