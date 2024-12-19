using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Actlog : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";
        private string currentUserId = "MA2301553"; // Replace with the logged-in user ID dynamically.

        public Actlog()
        {
            InitializeComponent();
            RecordLogin(currentUserId); // Record login when the form is initialized
            DisplayActivityLog(); // Display the activity log on form load
        }

        private void lb5_Click(object sender, EventArgs e)
        {
            new Records().Show();
            this.Hide();
        }

        private void lb3_Click(object sender, EventArgs e)
        {
            new notifs().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void lb2_Click(object sender, EventArgs e)
        {
            new abt().Show();
            this.Hide();
        }

        private void lb4_Click(object sender, EventArgs e)
        {
            new acc().Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            new home().Show();
            this.Hide();
        }

        private void Logout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (OleDbConnection con = new OleDbConnection(connectionString))
                    {
                        con.Open();
                        string formattedTimeOut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Update the ActivityLog with the logout time
                        string updateLogQuery = "UPDATE ActivityLog SET LogoutTime = @timeOut WHERE Username = @userId AND LogoutTime IS NULL";
                        using (OleDbCommand updateCmd = new OleDbCommand(updateLogQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@timeOut", formattedTimeOut);
                            updateCmd.Parameters.AddWithValue("@userId", currentUserId);

                            updateCmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("You have successfully logged out.");

                    new Form1().Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during logout: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        // Method to record login event
        private void RecordLogin(string userId)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "INSERT INTO ActivityLog (StudentID, LoginTime) VALUES (@userId, @loginTime)";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@loginTime", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error recording login: " + ex.Message);
                }
            }
        }

        // Method to display activity log
        private void DisplayActivityLog()
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    con.Open();

                    // Fetch data from ActivityLog
                    string query = "SELECT ID, Username, LoginTime, LogoutTime FROM ActivityLog";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the data to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying activity log: " + ex.Message);
            }
        }

        private void Actlog_Load(object sender, EventArgs e)
        {
            DisplayActivityLog(); // Refresh the activity log when the form loads
        }
    }
}
