using Dapper;
using Npgsql;
using System;
using Wkx;

namespace wkx_dapper_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = $"Host=localhost;Username=postgres;Password=postgres;Database=postgres;Port=5432";
            SqlMapper.AddTypeHandler(new GeometryTypeHandler());
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var geoms = conn.Query<Geometry>("select ST_AsBinary(wkb_geometry) as geometry from buildings_utrecht").AsList();
            foreach(var geom in geoms)
            {
                var polygon = (Polygon)geom;
                var points = polygon.ExteriorRing.Points;
                Console.WriteLine($"number of vertices: {points.Count}");
            }
            conn.Close();
        }
    }
}
