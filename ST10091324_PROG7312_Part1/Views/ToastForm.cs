using System;
using System.Drawing;
using System.Windows.Forms;

namespace ST10091324_PROG7312_Part1.Views
{    
    public partial class ToastForm : Form
    {
        int toastX, toastY;

        public ToastForm(string type, string message)
        {
            InitializeComponent();
            // The code to make the toast message appear on top was taken from StackOverflow
            // Author: Jader Dias
            // Link: https://stackoverflow.com/questions/257587/bring-a-window-to-the-front-in-wpf
            this.Activate();
            this.TopMost = true;
            //this.Focus();

            // The switch statement code below was taken from a YouTube video
            // Titled: Tutorial : How to Create a Toast Message. C# | Windows Form
            // Uploaded By: Coding Ideas
            // Link: https://www.youtube.com/watch?v=vLWWShU9gKY
            lblType.Text = type;
            lblMessage.Text = message;
            switch (type)
            {
                case "SUCESS":                   
                    toastBorder.BackColor = Color.FromArgb(57, 155, 53);
                    picIcon.Image = Properties.Resources.check;
                    break;
                case "ERROR":
                    toastBorder.BackColor = Color.FromArgb(227, 50, 45);
                    picIcon.Image = Properties.Resources.cross;
                    break;
                case "INFO":
                    toastBorder.BackColor = Color.FromArgb(18, 136, 191);
                    picIcon.Image = Properties.Resources.information;
                    break;
                case "WARNING":
                    toastBorder.BackColor = Color.FromArgb(247, 171, 35);
                    picIcon.Image = Properties.Resources.warning;
                    break;
            }
        }

        // The code for this method was taken from a YouTube video
        // Titled: Tutorial : How to Create a Toast Message. C# | Windows Form
        // Uploaded By: Coding Ideas
        // Link: https://www.youtube.com/watch?v=vLWWShU9gKY
        private void ToastForm_Load(object sender, EventArgs e)
        {
            DeterminePosition();
        }

        // The code for this method was taken from a YouTube video
        // Titled: Tutorial : How to Create a Toast Message. C# | Windows Form
        // Uploaded By: Coding Ideas
        // Link: https://www.youtube.com/watch?v=vLWWShU9gKY
        private void ToastTimer_Tick(object sender, EventArgs e)
        {
            toastY -= 10;
            this.Location = new Point(toastX, toastY);
            if(toastY <= 1000)
            {
                ToastTimer.Stop();
                ToastHide.Start();
            }
        }
        // The code for this method was taken from a YouTube video
        // Titled: Tutorial : How to Create a Toast Message. C# | Windows Form
        // Uploaded By: Coding Ideas
        // Link: https://www.youtube.com/watch?v=vLWWShU9gKY
        private int y = 100;
        private void ToastHide_Tick(object sender, EventArgs e)
        {
            y--;
            if(y <= 0)
            {
                toastY += 1;
                this.Location = new Point(toastX, toastY += 10);
                if(toastY > 1040)
                {
                    ToastHide.Stop();
                    y = 100;
                    this.Close();
                }
            }
        }

        // The code for this method was taken from a YouTube video
        // Titled: Tutorial : How to Create a Toast Message. C# | Windows Form
        // Uploaded By: Coding Ideas
        // Link: https://www.youtube.com/watch?v=vLWWShU9gKY
        private void DeterminePosition()
        {
            int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            toastX = ScreenWidth - this.Width - 480;
            toastY = ScreenHeight - this.Height - 180;

            this.Location = new Point(toastX, toastY);
        }
    }
}
