using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusDisplay
{
    public static class Configuration
    {
        // API URL
        public const string queryURL = "https://api.digitransit.fi/routing/v1/routers/finland/index/graphql";

        // Numbers of departures retrieved per stop
        public const int numberOfDeparturesRetrievedPerStop = 10;

        // Document path for document containing stop IDs
        public const string configFilePath = @"config.txt";

        // List of bus stop IDs that are followed
        public static List<string> busStopIDs = new List<string>();

        // Function to load bus stop IDs from config file
        public static void LoadBusStopConfig()
        {
            foreach (string line in File.ReadLines(Configuration.configFilePath))
            {
                busStopIDs.Add(line);
            }
        }
    }
}
