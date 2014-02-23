﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BluffinPokerClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Process.GetCurrentProcess().Kill();
            // If after that i'm still in memory, cry :)
        }

        private void btnStartTraining_Click(object sender, EventArgs e)
        {
            Hide();
            SplashTrainingConnect cf = new SplashTrainingConnect(txtPlayerName.Text, clstServerName.Text, (int)nudServerPort.Value);
            if (cf.ShowDialog() == DialogResult.OK)
                new LobbyTrainingForm(cf.Server).Show();
            else
                Show();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            Hide();
            SplashCareerConnect cf = new SplashCareerConnect(clstServerName.Text, (int)nudServerPort.Value, txtUsername.Text, txtPassword.Text);
            if (cf.ShowDialog() == DialogResult.OK)
            {
                txtPassword.Text = "";
                new LobbyCareerForm(cf.Server).Show();
            }
            else
                Show();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //TODO: RICK: Validate Password & Email

            Hide();
            SplashCareerRegister cf = new SplashCareerRegister(clstServerName.Text, (int)nudServerPort.Value, txtUser.Text, txtPassword1.Text, txtEmail1.Text, txtDisplayName.Text);
            if (cf.ShowDialog() == DialogResult.OK)
            {
                txtUser.Text = "";
                txtPassword1.Text = "";
                txtPassword2.Text = "";
                txtEmail1.Text = "";
                txtEmail2.Text = "";
                txtDisplayName.Text = "";
                new LobbyCareerForm(cf.Server).Show();
            }
            else
                Show();
        }
    }
}
