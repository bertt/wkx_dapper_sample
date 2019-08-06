# wkx_dapper_sample

Sample of reading PostGIS geometries using Micro-ORM Dapper (https://www.nuget.org/packages/Dapper/) and geometry library WKX (https://www.nuget.org/packages/Wkx/).

In this sample a PostGIS database is used. However the code can be adapted to any spatial database that is 
capable of returning geometries in WKB format (like SqlServer or MySql).


# Sample code

```
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
 ```

## Run program

Get a PostGIS database running with a table with some geometries.

- Start database using Docker

```
$ docker run -e POSTGRES_PASSWORD=postgres -it -p 5432:5432 -v postgis:/var/lib/postgresql/data mdillon/postgis
```

- Load sample file buildings_utrecht.geojson using ogr2ogr

```
$ ogr2ogr -f "PostgreSQL" PG:"dbname=postgres user=postgres password=postgres" "./sample_Data/buildings_utrecht.geojson" -nln buildings_utrecht
```

And this program should output the number of vertices per polygon:

```
number of vertices: 6
number of vertices: 10
number of vertices: 14
number of vertices: 21
number of vertices: 8
number of vertices: 13
```

