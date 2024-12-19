using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Archive : Form
    {
        // Database connection setup
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        OleDbCommand cmd = new OleDbCommand();
        private int userId;
        public Archive()
        {
            InitializeComponent();
            LoadArchivedUsers(); // Load archived users when the form opens
        }

        // Method to load archived users into the DataGridView
        private void LoadArchivedUsers()
        {
            try
            {
                con.Open();

                // Query to select archived users from the database
                string query = "SELECT [ID #], [Username], [Type] FROM BPC WHERE [IsArchived] = True";
                cmd = new OleDbCommand(query, con);

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable); // Fill the DataTable with data from the query

                dgvArchive.DataSource = dataTable; // Bind the DataTable to the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading archived users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        // Event handler for the Restore button
        private void btnRestore_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvArchive.SelectedRows.Count > 0)
            {
                // Get the selected row's user ID (or another unique identifier)
                string userID = dgvArchive.SelectedRows[0].Cells["ID #"].Value.ToString();

                // Restore the user by setting IsArchived = false
                try
                {
                    con.Open();
                    string restoreQuery = "UPDATE BPC SET IsArchived = False WHERE [ID #] = @ID";
                    cmd = new OleDbCommand(restoreQuery, con);
                    cmd.Parameters.AddWithValue("@ID", userID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("User restored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload the archived users after restoration
                    LoadArchivedUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error restoring user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Please select a user to restore.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Event handler for the Permanently Delete button
        private void btnPermanentlyDelete_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvArchive.SelectedRows.Count > 0)
            {
                // Get the selected row's user ID (or another unique identifier)
                string userID = dgvArchive.SelectedRows[0].Cells["ID #"].Value.ToString();

                // Confirm the permanent deletion
                DialogResult result = MessageBox.Show("Are you sure you want to permanently delete this user?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Permanently delete the user from the database
                    try
                    {
                        con.Open();
                        string deleteQuery = "DELETE FROM BPC WHERE [ID #] = @ID";
                        cmd = new OleDbCommand(deleteQuery, con);
                        cmd.Parameters.AddWithValue("@ID", userID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User permanently deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload the archived users after deletion
                        LoadArchivedUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error permanently deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // Other navigation buttons for different forms
        private void lb5_Click(object sender, EventArgs e)
        {
            new Records().Show();
            this.Hide();
        }

        private void lb2_Click(object sender, EventArgs e)
        {
            new abt().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void lb3_Click(object sender, EventArgs e)
        {
            new notifs().Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            new home().Show();
            this.Hide();
        }

        private void lb4_Click(object sender, EventArgs e)
        {
            new acc().Show();
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
    }
}
