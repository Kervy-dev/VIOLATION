using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class acc : Form
    {
        public acc()
        {
            InitializeComponent();
            LoadStudentUsernames(); // Load student usernames from the database when the form opens
        }
        private int userId;
        // Database connection setup
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        OleDbCommand cmd = new OleDbCommand();

        // Method to load student usernames into the DataGridView
        private void LoadStudentUsernames()
        {
            try
            {
                con.Open();

                // Query to select only students from the database, excluding archived users
                string query = "SELECT [ID #], [Username], [Type] FROM BPC WHERE [Type] = 'Student' AND [IsArchived] = False"; // Filter out archived users
                cmd = new OleDbCommand(query, con);

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable); // Fill the DataTable with data from the query

                DGV1.DataSource = dataTable; // Bind the DataTable to the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student usernames: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        // Event handler for the Add button
        private void button1_Click(object sender, EventArgs e)
        {
            using (add addForm = new add())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    string newUsername = addForm.Username;
                    string userType = "Student"; // Default to Student type

                    if (!string.IsNullOrEmpty(newUsername))
                    {
                        try
                        {
                            con.Open();
                            string query = "INSERT INTO BPC ([Username], [Type]) VALUES (@Username, @Type)";
                            cmd = new OleDbCommand(query, con);
                            cmd.Parameters.AddWithValue("@Username", newUsername);
                            cmd.Parameters.AddWithValue("@Type", userType);
                            cmd.ExecuteNonQuery();

                            // Load the updated data into the DataGridView
                            LoadStudentUsernames();
                            MessageBox.Show("Username added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding username: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        // Event handler for the Delete (Archive) button
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (DGV1.SelectedRows.Count > 0)
            {
                // Get the selected row's user ID (or another unique identifier)
                string userID = DGV1.SelectedRows[0].Cells["ID #"].Value.ToString();

                // Confirm the archiving action
                DialogResult result = MessageBox.Show("Are you sure you want to archive this user?", "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Archive the user by setting IsArchived = True
                    try
                    {
                        con.Open();
                        string archiveQuery = "UPDATE BPC SET IsArchived = True WHERE [ID #] = @ID";
                        cmd = new OleDbCommand(archiveQuery, con);
                        cmd.Parameters.AddWithValue("@ID", userID);
                        cmd.ExecuteNonQuery();

                        // Reload the student usernames after archiving
                        LoadStudentUsernames();

                        MessageBox.Show("User has been archived successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error archiving user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to archive.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Navigate to other forms (These remain the same)
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
            new Records().Show();
            this.Hide();
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
