using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class violation : Form
    {
        OleDbConnection conn;

        public violation()
        {
            InitializeComponent();
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        }

        // Load violation data into the DataGridView
        public void LoadData()
        {
            string query = "SELECT StudentID, Violation FROM Violation";

            using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb"))
            {
                try
                {
                    conn.Open();

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void btnclubs_Click(object sender, EventArgs e)
        {
            new clubs().Show();
            this.Hide();
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new home().Show();
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
            new StatisticAdmin().Show();
            this.Hide();
        }

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            new clubs().Show();
            this.Hide();
        }
    }
}
