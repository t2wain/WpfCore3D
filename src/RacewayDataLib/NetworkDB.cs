using CsvHelper;
using System.Globalization;

namespace RacewayDataLib
{
    /// <summary>
    /// Read and save raceway data from and to CSV files
    /// </summary>
    public static class NetworkDB
    {
        public class NetworkData
        {
            public IEnumerable<Raceway> Raceways { get; set; } = [];
            public IEnumerable<Cable> Cables { get; set; } = [];
            public IEnumerable<Route> Routes { get; set; } = [];
            public IEnumerable<SegSystem> Systems { get; set; } = [];
            public IEnumerable<Node> Nodes { get; set; } = [];
        }

        /// <summary>
        /// Filter raceway
        /// </summary>
        public static IEnumerable<Raceway> GetTray(this IEnumerable<Raceway> raceways) =>
            raceways.Where(r => r.Type == "TRAY");

        /// <summary>
        /// Filter raceway
        /// </summary>
        public static IEnumerable<Raceway> SelectSystem(this IEnumerable<Raceway> raceways, int sysId) =>
            raceways.Where(r => r.Systems.Contains(sysId));

        #region Read Data from CSV files

        /// <summary>
        /// Load a complete set of raceway data from CSV files
        /// </summary>
        public static NetworkData LoadData(DataConfig files)
        {
            var data = new NetworkData
            { 
                Raceways = ReadRaceways(files.RacewayFile),
                Cables = ReadCables(files.CableFile),
                Routes = ReadRoutes(files.RouteFile),
                Systems = ReadSegSystems(files.SegSystemFile),
                Nodes = ReadNodes(files.NodeFile),
            };

            var dnode = data.Nodes.ToDictionary(n => n.ID);
            foreach (var rw in data.Raceways)
            {
                if (dnode.TryGetValue(rw.FromNodeID, out var n))
                    rw.FromNode = n;
                if (dnode.TryGetValue(rw.ToNodeID, out n))
                    rw.ToNode = n;
            }

            return data;
        }

        public static IEnumerable<Raceway> ReadRaceways(string path)
        {
            TextReader rd = new StreamReader(path);
            var csv = new CsvReader(rd, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<RacewayMap>();
            csv.Context.Configuration.HasHeaderRecord = true;
            return csv.GetRecords<Raceway>().ToList();
        }

        public static IEnumerable<Cable> ReadCables(string path)
        {
            TextReader rd = new StreamReader(path);
            var csv = new CsvReader(rd, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<CableMap>();
            csv.Context.Configuration.HasHeaderRecord = true;
            return csv.GetRecords<Cable>().ToList();
        }

        public static IEnumerable<Route> ReadRoutes(string path)
        {
            TextReader rd = new StreamReader(path);
            var csv = new CsvReader(rd, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<RouteMap>();
            csv.Context.Configuration.HasHeaderRecord = true;
            return csv.GetRecords<Route>().ToList();
        }

        public static IEnumerable<SegSystem> ReadSegSystems(string path)
        {
            TextReader rd = new StreamReader(path);
            var csv = new CsvReader(rd, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<SegSystemMap>();
            csv.Context.Configuration.HasHeaderRecord = true;
            return csv.GetRecords<SegSystem>().ToList();
        }

        public static IEnumerable<Node> ReadNodes(string path)
        {
            TextReader rd = new StreamReader(path);
            var csv = new CsvReader(rd, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<NodeMap>();
            csv.Context.Configuration.HasHeaderRecord = true;
            return csv.GetRecords<Node>().ToList();
        }

        #endregion

        #region Save Data to CSV files

        public static void SaveRaceways(string path, IEnumerable<Raceway> data)
        {
            SaveData(path, data, typeof(RacewayMap));
        }

        public static void SaveCables(string path, IEnumerable<Cable> data)
        {
            SaveData(path, data, typeof(CableMap));
        }

        public static void SaveRoutes(string path, IEnumerable<Route> data)
        {
            SaveData(path, data, typeof(RouteMap));
        }

        public static void SaveSegSystems(string path, IEnumerable<SegSystem> data)
        {
            SaveData(path, data, typeof(SegSystemMap));
        }

        public static void SaveNodes(string path, IEnumerable<Node> data)
        {
            SaveData(path, data, typeof(NodeMap));
        }

        static void SaveData(string path, System.Collections.IEnumerable data, Type classMap)
        {
            TextWriter rd = new StreamWriter(path);
            using (var csv = new CsvWriter(rd, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(classMap);
                csv.Context.Configuration.HasHeaderRecord = true;
                csv.WriteRecords(data);
            }
        }

        #endregion
    }
}
