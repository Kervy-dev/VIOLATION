using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Event_Lost_Found_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void userID_Click(object sender, EventArgs e)
        {
            userID.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel4.BackColor = SystemColors.Control;
            pass.BackColor = SystemColors.Control;
        }

        private void pass_Click(object sender, EventArgs e)
        {
            pass.BackColor = Color.White;
            panel4.BackColor = Color.White;
            userID.BackColor = SystemColors.Control;
            panel3.BackColor = SystemColors.Control;
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            pass.UseSystemPasswordChar = false;
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            pass.UseSystemPasswordChar = true;
        }

        private void btnlog_Click(object sender, EventArgs e)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\petwu\source\repos\Event&Lost-Found System\bin\Debug\Table1.accdb";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("Select Username, Password From table1 Where Username=@user AND Password=@pass", conn);
                cmd.Parameters.AddWithValue("@user", userID.Text);
                cmd.Parameters.AddWithValue("@pass", pass.Text);
                OleDbDataReader myreader = cmd.ExecuteReader();

                if (myreader.Read())
                {
                    int resultcomp = String.Compare(pass.Text, myreader.GetValue(1).ToString());
                    if (resultcomp == 0)
                    {
                        Lost_Found fr2 = new Lost_Found();
                        fr2.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Error Username or Password");
                    }
                }
                else
                {
                    MessageBox.Show("Error Username or Password");
                }
            }
        }
    }
}
