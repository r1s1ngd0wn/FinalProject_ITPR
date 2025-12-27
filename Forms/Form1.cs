using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DACK_ITPROJECT
{
    // Keep Form1 as a simple host in case the project references it from designer
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadSubForm(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            // Use the panel that actually exists on the form (panelCenter from the designer)
            this.panelCenter.Controls.Clear();
            this.panelCenter.Controls.Add(uc);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAddPhone_Click(object sender, EventArgs e)
        {
            // Load the AddNewPhone user control into the center panel for testing
            var uc = new UC_AddNewPhone();
            LoadSubForm(uc);
        }
    }
}
