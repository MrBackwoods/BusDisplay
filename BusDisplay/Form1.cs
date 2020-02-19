using System;
using System.Windows.Forms;

namespace BusDisplay
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Rescale departure columns on windows resize
        private void Form1_Resize(object sender, System.EventArgs e)
        {
            Control control = (Control)sender;

            foreach (ColumnHeader column in departureListView.Columns)
            {
                departureListView.Columns[column.Index].Width = departureListView.Width / 4 - 5;
            }
        }

        // Set up form, HTTP client, conflig and get initial departures
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareForm();
                Configuration.LoadBusStopConfig();
                HTTPHandler.SetHTTPClient();
                DepartureHandler.GetAndSetDepartures(departureListView, dateTimePicker.Value);
            }

            // Show exceptions on list view
            catch (Exception ex)
            {
                departureListView.View = View.Details;
                departureListView.Clear();
                departureListView.Columns.Add("Exception", departureListView.Width);
                departureListView.Items.Add(ex.Message);
            }
        }

        // Set up defaults for list view and date time picker
        public void PrepareForm()
        {
            departureListView.View = View.Details;
            departureListView.Columns.Add("Stop", departureListView.Width / 4 - 5);
            departureListView.Columns.Add("Line", departureListView.Width / 4 - 5);
            departureListView.Columns.Add("Destination", departureListView.Width / 4 - 5);
            departureListView.Columns.Add("Departure", departureListView.Width / 4 - 5);
            dateTimePicker.Value = DateTime.Now;
            departureListView.FullRowSelect = true;
        }

        // Get departures on button press
        private void searchButton_Click_1(object sender, EventArgs e)
        {
            DepartureHandler.GetAndSetDepartures(departureListView, dateTimePicker.Value);
        }
    }
}
