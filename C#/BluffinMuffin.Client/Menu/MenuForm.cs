﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BluffinMuffin.Client.Menu
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            Hide();
            new TrainingParmsForm(clstServerName.Text, (int)nudServerPort.Value).ShowDialog();
            Show();
        }

        private void btnCareerConnect_Click(object sender, EventArgs e)
        {
            Hide();
            new CareerConnectParmsForm(clstServerName.Text, (int)nudServerPort.Value).ShowDialog();
            Show();
        }

        private void btnCareerRegister_Click(object sender, EventArgs e)
        {
            Hide();
            new CareerRegisterParmsForm(clstServerName.Text, (int)nudServerPort.Value).ShowDialog();
            Show();
        }

        private void MenuForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Process.GetCurrentProcess().Kill();
            // If after that i'm still in memory, cry :)
        }
    }
}
