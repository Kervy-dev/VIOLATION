﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Records : Form
    {
        public Records()
        {
            InitializeComponent();
        }
        private int userId;
        private void lb3_Click(object sender, EventArgs e)
        {
            new notifs().Show();
            this.Hide();
        }

        private void lb2_Click(object sender, EventArgs e)
        {
            new abt().Show();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

       

        private void label1_Click(object sender, EventArgs e)
        {
            new Archive().Show();
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

        private void lbl3_Click(object sender, EventArgs e)
        {
            new Actlog().Show();
            this.Hide();
        }
    }
}
