using System;
using System.Data;
using System.Data.OleDb;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Violation_Crud_Admin : Form
    {
        OleDbConnection conn;

        public Violation_Crud_Admin()
        {
            InitializeComponent();
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
            conn.Open();
        }

        // Load violation data into the DataGridView
        public void LoadData()
        {
            string query = "SELECT ID, Student_ID, Offense_ID, Consequences_ID, Date_and_Time FROM [Student Violation Records]";

            using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb"))
            {
                try
                {
                    conn.Open();

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    foreach (DataRow row in dt.Rows)
                    {
                        // Retrieve Offense name using Offenses_ID
                        int offenseId = Convert.ToInt32(row["Offense_ID"]);
                        row["Offense_ID"] = GetOffenseName(offenseId); // Replace ID with name

                        /* Retrieve Consequence name using Consequences_ID IN PROGRESSSSS
                        int consequenceId = Convert.ToInt32(row["Consequences_ID"]);
                        row["Consequences_ID"] = GetConsequenceName(consequenceId); // Replace ID with name
                        */
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }

                finally
                {
                    conn.Close(); // Ensure the connection is closed
                }
            }
        }

        private string GetOffenseName(int offenseId)
        {
            string offenseName = string.Empty;
            string offenseQuery = "SELECT Offenses FROM Offenses WHERE Offenses_ID = @OffenseId";

            try
            {
                using (OleDbCommand cmd = new OleDbCommand(offenseQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@OffenseId", offenseId);
                    object result = cmd.ExecuteScalar();  // Get the Offense name
                    if (result != null)
                    {
                        offenseName = result.ToString();  // Set the name
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving Offense name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return offenseName; // Return the name or empty string if not found
        }

        /* Method to get the Consequence name by its ID IN PROGRESSSSS
        private string GetConsequenceName(int consequenceId)
        {
            string consequenceName = string.Empty;
            string consequenceQuery = "SELECT Consequences FROM Consequences WHERE Consequences_ID = @ConsequenceId";

            try
            {
                using (OleDbCommand cmd = new OleDbCommand(consequenceQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ConsequenceId", consequenceId);
                    object result = cmd.ExecuteScalar();  // Get the Consequence name
                    if (result != null)
                    {
                        consequenceName = result.ToString();  // Set the name
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving Consequence name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return consequenceName; // Return the name or empty string if not found
        }
        */

        private void btnclubs_Click(object sender, EventArgs e)
        {
            new Major_Offenses_Admin().Show();
            this.Hide();
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            new Home_Admin().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Dashboard_Admin().Show();
            this.Hide();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void violation_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            new Statistic_Admin().Show();
            this.Hide();
        }

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            new Major_Offenses_Admin().Show();
            this.Hide();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if a valid cell (Student_ID column) is clicked
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Student_ID"].Index)
            {
                // Retrieve the Student_ID from the selected row
                string studentID = dataGridView1.Rows[e.RowIndex].Cells["Student_ID"].Value.ToString();

                // Call a method to load the violations for the selected student

                LoadViolationsForStudent(studentID);
            }
        }

        private void LoadViolationsForStudent(string studentID)
        {
            try
            {
                // Create a connection to the Access database
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
                {
                    conn.Open(); // Open the connection to the database

                    // Create the OleDbDataAdapter and set its SelectCommand
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Student Violation Records] WHERE [Student_ID] = ?", conn))
                    {
                        // Add the parameter for Student_ID
                        adapter.SelectCommand.Parameters.AddWithValue("?", studentID);

                        // Create a DataTable to hold the violations data
                        DataTable dtViolations = new DataTable();

                        // Fill the DataTable with the data fetched from the database
                        adapter.Fill(dtViolations);

                        // Bind the DataTable to the DataGridView
                        dgvStudentListofViolation.DataSource = dtViolations;

                        dgvStudentListofViolation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading violations: " + ex.Message);
            }
        }


        private void ConfigureDataGridView2()
        {
            // Set appropriate column headers for the second DataGridView
            dgvStudentListofViolation.Columns["Offenses"].HeaderText = "Offense";
            dgvStudentListofViolation.Columns["Date_and_Time"].HeaderText = "Date and Time";
            dgvStudentListofViolation.Columns["Counts"].HeaderText = "Counts";

            // Optionally, you can set the format of the Date_and_Time column
            dgvStudentListofViolation.Columns["Date_and_Time"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
        }


    }

}
