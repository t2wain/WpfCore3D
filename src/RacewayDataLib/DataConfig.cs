namespace RacewayDataLib
{
    public class DataConfig
    {
        public DataConfig()
        {
            this.DataFolderPath = "C:\\devgit\\Data\\J6327";
            this.RacewayFile = Path.Combine(DataFolderPath, "raceways.csv");
            this.NodeFile = Path.Combine(DataFolderPath, "nodes.csv");
            this.SegSystemFile = Path.Combine(DataFolderPath, "seg_systems.csv");
            this.CableFile = Path.Combine(DataFolderPath, "cables.csv");
            this.RouteFile = Path.Combine(DataFolderPath, "routes.csv");
            this.PreferRouteFile = Path.Combine(DataFolderPath, "prefer_routes.csv");
        }

        public string DataFolderPath { get; set; }
        public string RacewayFile { get; set; }
        public string NodeFile { get; set; } 
        public string SegSystemFile { get; set; } 
        public string CableFile { get; set; } 
        public string RouteFile { get; set; } 
        public string PreferRouteFile { get; set; }  

    }
}
