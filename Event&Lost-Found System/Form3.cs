using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Form3 : Form
    {

        private Panel currentPanel; // The current visible panel
        private Panel nextPanel;    // The panel being transitioned to
        private Timer animationTimer; // Timer for handling the animation
        private int animationSpeed = 20; // Speed of animation
        private bool slideIn; // Determines direction of slide (true = slide in, false = slide out)

        public string TextBoxValue { get; set; }  // Property to store the entered text

        public Form3()
        {
            InitializeComponent();

            // Initialize animation timer
            animationTimer = new Timer();
            animationTimer.Interval = 10; // Set timer interval for smooth animation
            animationTimer.Tick += AnimationTimer_Tick; // Attach tick event
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Ensure pnl1 is visible when the form loads
            pna1.Visible = true;
            pnl2.Visible = false;
            pna3.Visible = false;
            

            // Position the panels off-screen initially
            pna1.Left = 25; // pnl1 stays on screen
            pnl2.Left = this.Width; // pnl2 starts off-screen to the right
            pna3.Left = this.Width; // pnl3 starts off-screen to the right
           
        }

        private void btnN_Click_1(object sender, EventArgs e)
        {
            // Start slide animation to go to the next panel
            if (pna1.Visible)
            {
                StartSlideAnimation(pna1, pnl2, true); // Slide from pna1 to pnl2
            }
            else if (pnl2.Visible)
            {
                StartSlideAnimation(pnl2, pna3, true); // Slide from pnl2 to pna3
            }
            else if (pna3.Visible)
            {
                StartSlideAnimation(pna3, pna1, true); // Slide from pna3 to pna1
            }
        }

        private void btnP_Click_1(object sender, EventArgs e)
        {
            // Start slide animation to go to the previous panel
            if (pna1.Visible)
            {
                StartSlideAnimation(pna1, pna3, false); // Slide from pna1 to pna3
            }
            else if (pnl2.Visible)
            {
                StartSlideAnimation(pnl2, pna1, false); // Slide from pnl2 to pna1
            }
            else if (pna3.Visible)
            {
                StartSlideAnimation(pna3, pnl2, false); // Slide from pna3 to pnl2
            }
        }

        private void StartSlideAnimation(Panel fromPanel, Panel toPanel, bool isSlidingIn)
        {
            // Debugging: Check the panels being transitioned
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

        private void btnok_Click(object sender, EventArgs e)
        {
            // Capture the entered text
            TextBoxValue = textBox1.Text;

            // Check if the text is not empty
            if (!string.IsNullOrEmpty(TextBoxValue))
            {
                try
                {
                    // Open a connection to the database
                    OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");

                    // Insert the text into the Task table
                    string query = "INSERT INTO Task ([Task]) VALUES (@Task)"; // Assuming 'TaskText' is the correct column name
                    OleDbCommand cmd = new OleDbCommand(query, con);

                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@Task", TextBoxValue);

                    // Open the connection and execute the query
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Text captured saved! Now you can view it on the User form.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving text to database: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter some text in the textbox.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnclubs_Click(object sender, EventArgs e)
        {
            new clubs().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new home().Show();
            this.Hide();
        }

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            new clubs().Show();
            this.Hide();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            new StatisticAdmin().Show();
            this.Hide();
        }
    }
}
