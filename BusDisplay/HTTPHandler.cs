using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using System.Windows.Forms;

namespace BusDisplay
{
    public static class HTTPHandler
    {
        // HTTP Client
        static HttpClient client = new HttpClient();

        // Function to set up HTTP client
        public static void SetHTTPClient()
        {
            client.BaseAddress = new Uri(Configuration.queryURL);
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Function for updating departure info from API call
        public static async Task UpdateDepartures(ListView departureView, DateTime departureTime)
        {
            // Clear old list of departures
            DepartureHandler.departures = new List<Departure>();

            // Get departures from API
            foreach (string busStopID in Configuration.busStopIDs)
            {
                // Form query
                string graphQLquery = "{ stops(ids: \"" + busStopID + "\") { name id stoptimesWithoutPatterns(startTime: " + TimeHandler.ConvertToUTCTimestamp(departureTime) + " numberOfDepartures: " + Configuration.numberOfDeparturesRetrievedPerStop + ") { scheduledDeparture realtimeDeparture departureDelay realtime realtimeState serviceDay headsign realtimeDeparture trip { routeShortName }}}}";
                JObject queryAsJOBject = new JObject();
                queryAsJOBject.Add(new JProperty("query", graphQLquery));

                // Make POST call
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
                request.Content = new StringContent(queryAsJOBject.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request.RequestUri, request.Content);

                // Handle response data to departure objects
                JObject responseAsJobject = JObject.Parse(await response.Content.ReadAsStringAsync());
                JArray stops = (JArray)responseAsJobject["data"]["stops"];

                foreach (var stopItem in stops.Children())
                {
                    if (stopItem["stoptimesWithoutPatterns"].GetValue() != null)
                    {
                        JArray departuresArray = (JArray)stopItem["stoptimesWithoutPatterns"];

                        foreach (var departureItem in departuresArray.Children())
                        {
                            Departure departure = new Departure();
                            departure.headsign = departureItem["headsign"].GetValue().ToString();
                            departure.route = departureItem["trip"]["routeShortName"].GetValue().ToString();
                            departure.departureTime = TimeHandler.ConvertToLocalTimeDateTime((int)departureItem["realtimeDeparture"].GetValue() + (int)departureItem["serviceDay"].GetValue());
                            departure.stopName = stopItem["name"].GetValue().ToString();
                            DepartureHandler.departures.Add(departure);
                        }
                    }
                }
            }
        }
    }
}
