using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Event_Lost_Found_System
{
    public partial class StatisticAdmin : Form
    {
        public StatisticAdmin()
        {
            InitializeComponent();

            // Set up chart title for chart1 (use the correct chart control name)
            chart1.Titles.Add("Most Common Violation in BPC");
            chart1.Titles[0].Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold);

            // Set title alignment to upper-middle
            chart1.Titles[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top; // Title docked at the top
            chart1.Titles[0].Alignment = ContentAlignment.MiddleCenter; // Align title to center

            // Adjust title position to move slightly to the left
            chart1.Titles[0].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(10, 0, 70, 5);
            // 15% Left (move to the left slightly), 0% Top, 70% Width, 10% Height

            // Ensure the chart series "S1" exists
            if (chart1.Series.IndexOf("S1") == -1)
            {
                chart1.Series.Add("S1");
                chart1.Series["S1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar; // Set chart type (adjust as needed)
            }

            // Load data from the Access database into the chart
            LoadViolationData();
        }

        private void LoadViolationData()
        {
            // Connection string for your Access database
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\petwu\source\repos\Event&Lost-Found System\bin\Debug\Monitoring.accdb;";

            // SQL query to fetch data from the Violation table
            string query = "SELECT [violation], [counts] FROM [Violation]";

            try
            {
                // Create a DataTable to hold the fetched data
                DataTable dataTable = new DataTable();

                // Fetch data from the database
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        connection.Open();
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }

                // Clear existing data from the chart series
                chart1.Series["S1"].Points.Clear();

                // Populate the chart with data
                foreach (DataRow row in dataTable.Rows)
                {
                    string category = row["violation"].ToString(); // Assuming 'violation' is the category column
                    double value = Convert.ToDouble(row["counts"]); // Assuming 'counts' is the numeric column
                    chart1.Series["S1"].Points.AddXY(category, value);
                }

                // Refresh chart to update the display
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                // Show error message if there's an issue
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void btnvio_Click(object sender, EventArgs e)
        {
            new violation().Show();
            this.Hide();
        }

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            new offense().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new StatisticAdmin().Show();
            this.Hide();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new home().Show();
            this.Hide();
        }
    }
}
