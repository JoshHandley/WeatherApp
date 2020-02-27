using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int LocationId { get; set; }

        public string LocationName { get; set; }
    }
}
