using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class home : Form
    {
        public home()
        {
            InitializeComponent();
            LoadUserProfile(); // Automatically load user profile when the form opens
        }

        // Database connection setup
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        OleDbCommand cmd = new OleDbCommand();
        private int userId;
        // Method to Load User Profile Data
        private void LoadUserProfile()
        {
            try
            {
                con.Open();
                string query = "SELECT TOP 1 [ID #], [Username], [Password], [Email], [Contact #] FROM SII"; // Get the first record
                cmd = new OleDbCommand(query, con);

                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Populate the TextBoxes or Labels with the retrieved data
                    lbl1.Text = reader["ID #"].ToString();
                    lbl2.Text = reader["Email"].ToString();
                    lbl3.Text = reader["Contact #"].ToString();
                    lbl4.Text = reader["Username"].ToString();
                    lbl5.Text = reader["Password"].ToString();
                }
                else
                {
                    MessageBox.Show("No user data found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnvio_Click(object sender, EventArgs e)
        {
            new violation().Show();
            this.Hide();
        }

        private void btnclubs_Click(object sender, EventArgs e)
        {
            new clubs().Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            lb2.BackColor = Color.Transparent;
            pnl2.BackColor = Color.Transparent;
            pnl1.BackColor = SystemColors.GrayText;
            label5.BackColor = SystemColors.GrayText;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void lb4_Click(object sender, EventArgs e)
        {
            new acc().Show();
            this.Hide();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // No implementation needed here unless required for specific behavior.
        }

        private void lb2_Click(object sender, EventArgs e)
        {
            new abt().Show();
            this.Hide();
        }

        private void lb5_Click(object sender, EventArgs e)
        {
            new Records().Show();
            this.Hide();
        }

        private void Logout_Click_1(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to Logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {

                    // Set up the database connection
                    using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb"))
                    {
                        // Open the connection
                        con.Open();

                        // Format DateTime.Now to remove milliseconds
                        string formattedTimeOut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Update the ActivityLog with the logout time
                        string updateLogQuery = "UPDATE ActivityLog SET LogoutTime = @timeOut WHERE UserID = @userId AND LogoutTime IS NULL";
                        OleDbCommand updateCmd = new OleDbCommand(updateLogQuery, con);
                        updateCmd.Parameters.AddWithValue("@timeOut", formattedTimeOut);
                        updateCmd.Parameters.AddWithValue("@userId", userId);

                        // Execute the update query
                        updateCmd.ExecuteNonQuery();
                    }

                    // Optionally, show a message saying logout was successful
                    MessageBox.Show("You have successfully logged out.");

                    // Show the login form again (assuming Form1 is the login form)
                    new Form1().Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    // Handle any errors that may occur during the logout process
                    MessageBox.Show("Error during logout: " + ex.Message);
                }
                new Form1().Show();
                this.Hide();
            }
        }

        private void lb3_Click(object sender, EventArgs e)
        {
            new notifs().Show();
            this.Hide();
        }
    }
}
