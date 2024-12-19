using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Home_User : Form
    {
        private Panel currentPanel; // The current visible panel
        private Panel nextPanel;    // The panel being transitioned to
        private Timer animationTimer; // Timer for handling the animation
        private int animationSpeed = 20; // Speed of animation
        private bool slideIn; // Determines direction of slide (true = slide in, false = slide out)

        public Home_User()
        {
            InitializeComponent();

            // Initialize the animation Timer
            animationTimer = new Timer();
            animationTimer.Interval = 10; // Timer tick interval in milliseconds
            animationTimer.Tick += AnimationTimer_Tick; // Attach event for timer ticks
        }
        private int userId;

        // Method to add text to the ListBox
        public void AddToListBox(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                listbox1.Items.Add($"Username: {text}"); // Add the text with "Username:" prefix
            }
        }

        private void btnv_Click(object sender, EventArgs e)
        {
            new Violation_Form_User().Show();
            this.Hide();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnof_Click(object sender, EventArgs e)
        {
            new Major_Offenses_User().Show();
            this.Hide();
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            new Home_User().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard_User profileForm = new Dashboard_User(userId);
            profileForm.Show(); // Show the profile form
            this.Hide();        // Hide the login form
        }

        // Display the captured text (e.g., username)
        public void ShowCapturedText(string capturedText)
        {
            if (!string.IsNullOrEmpty(capturedText))
            {
                AddToListBox(capturedText); // Add the text to ListBox with "Username:" prefix
            }
        }

        private void user_Load(object sender, EventArgs e)
        {
            // Ensure pnl1 is visible when the form loads
            pnl1.Visible = true;
            pnl2.Visible = false;
            pnl3.Visible = false;
            pnl4.Visible = false;

            // Position the panels off-screen initially
            pnl1.Left = 25; // pnl1 stays on screen
            pnl2.Left = this.Width; // pnl2 starts off-screen to the right
            pnl3.Left = this.Width; // pnl3 starts off-screen to the right
            pnl4.Left = this.Width; // pnl4 starts off-screen to the right

            try
            {
                // Establish a connection to the database
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");

                string query = "SELECT [Task] FROM Task"; // Query to fetch tasks
                OleDbCommand cmd = new OleDbCommand(query, con);

                con.Open();

                OleDbDataReader reader = cmd.ExecuteReader();
                listbox1.Items.Clear();

                while (reader.Read())
                {
                    string taskText = reader["Task"].ToString();
                    listbox1.Items.Add($"Task: {taskText}"); // Add tasks to the ListBox
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tasks: " + ex.Message);
            }
        }

        // Timer tick event for slide animation
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int targetPosition = 25;

            if (slideIn)
            {
                // Sliding in
                nextPanel.Left -= animationSpeed; // Move the next panel toward the center
                currentPanel.Left -= animationSpeed; // Move the current panel toward the left

                // Stop animation when the next panel reaches position 0
                if (nextPanel.Left <= targetPosition)
                {
                    animationTimer.Stop();
                    nextPanel.Left = targetPosition; // Correct the position of the next panel
                    currentPanel.Left = -this.Width; // Move current panel completely out of view
                    currentPanel.Visible = false; // Hide the current panel
                }
            }
            else
            {
                // Sliding out
                nextPanel.Left += animationSpeed; // Move the next panel toward the center
                currentPanel.Left += animationSpeed; // Move the current panel toward the right

                // Stop animation when the current panel is out of view
                if (currentPanel.Left >= this.Width)
                {
                    animationTimer.Stop();
                    currentPanel.Left = this.Width; // Ensure the current panel is off-screen
                    nextPanel.Left = targetPosition; // Position the next panel at its target position
                    currentPanel.Visible = false; // Hide the current panel
                }
            }
        }

        // Start panel transition with animation
        private void StartSlideAnimation(Panel fromPanel, Panel toPanel, bool isSlidingIn)
        {
            currentPanel = fromPanel;
            nextPanel = toPanel;
            slideIn = isSlidingIn;

            // Set initial positions for sliding
            if (slideIn)
            {
                nextPanel.Left = this.Width; // Next panel starts off-screen to the right
            }
            else
            {
                nextPanel.Left = -this.Width; // Next panel starts off-screen to the left
            }

            nextPanel.Visible = true; // Make the next panel visible
            animationTimer.Start(); // Start the timer to trigger animation
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (pnl1.Visible)
            {
                StartSlideAnimation(pnl1, pnl2, true); // Slide from pnl1 to pnl2
            }
            else if (pnl2.Visible)
            {
                StartSlideAnimation(pnl2, pnl3, true); // Slide from pnl2 to pnl3
            }
            else if (pnl3.Visible)
            {
                StartSlideAnimation(pnl3, pnl4, true); // Slide from pnl3 to pnl4
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (pnl4.Visible)
            {
                StartSlideAnimation(pnl4, pnl3, false); // Slide from pnl4 to pnl3
            }
            else if (pnl3.Visible)
            {
                StartSlideAnimation(pnl3, pnl2, false); // Slide from pnl3 to pnl2
            }
            else if (pnl2.Visible)
            {
                StartSlideAnimation(pnl2, pnl1, false); // Slide from pnl2 to pnl1
            }
        }

        // Show a specific panel without animation
        private void ShowPanel(Panel panelToShow)
        {
            pnl1.Visible = false;
            pnl2.Visible = false;
            pnl3.Visible = false;
            pnl4.Visible = false;

            panelToShow.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Statistic_User().Show();
            this.Hide();
        }
    }
}
