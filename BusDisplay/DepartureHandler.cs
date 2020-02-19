using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BusDisplay
{
    public static class DepartureHandler
    {
        // List of departure information to be queried using buss stop IDs
        public static List<Departure> departures = new List<Departure>();

        // Function to get and set departures
        public static async void GetAndSetDepartures(ListView departureView, DateTime departureTime)
        {
            await HTTPHandler.UpdateDepartures(departureView, departureTime);
            departures = departures.OrderBy(o => o.departureTime).ToList();
            UpdateDepartureListView(departureView, departures);
        }

        // Update departure list view
        public static void UpdateDepartureListView(ListView listView, List<Departure> departures)
        {
            if (GridInvokeRequired(listView, () => UpdateDepartureListView(listView, departures))) return;

            listView.Items.Clear();

            bool color = false;

            foreach (Departure departure in departures)
            {
                string[] row = new string[4] { departure.stopName, departure.route, departure.headsign, departure.departureTime.ToShortTimeString() };
                ListViewItem item = new ListViewItem(row);

                if (color)
                {
                    color = false;
                }
                else
                {
                    item.BackColor = System.Drawing.Color.LightBlue;
                    color = true;
                }

                listView.Items.Add(item);
            }
        }

        // Update label (invoke required)
        public static bool GridInvokeRequired(ListView c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;
            return true;
        }
    }
}
