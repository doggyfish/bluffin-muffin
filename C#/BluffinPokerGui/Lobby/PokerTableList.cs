﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PokerProtocol;
using BluffinPokerGui.Game;

namespace BluffinPokerGUI.Lobby
{
    public partial class PokerTableList : UserControl
    {
        private LobbyTCPClient m_Server;
        public PokerTableList()
        {
            InitializeComponent();
        }

        public void setServer(LobbyTCPClient server)
        {
            m_Server = server;
        }

        public int NbTables { get { return datTables.RowCount; } }
        public bool SomethingSelected { get { return datTables.RowCount > 0 && datTables.SelectedRows.Count > 0; } }
        public event EventHandler OnListRefreshed = delegate { };
        public event EventHandler OnSelectionChanged = delegate { };
        public event EventHandler OnChoiceMade = delegate { };
        
        public void RefreshList()
        {
            datTables.Rows.Clear();
            List<TupleTableInfo> lst = m_Server.getListTables();
            for (int i = 0; i < lst.Count; ++i)
            {
                TupleTableInfo info = lst[i];
                datTables.Rows.Add();
                datTables.Rows[i].Cells[0].Value = info.NoPort;
                datTables.Rows[i].Cells[1].Value = info.TableName;
                datTables.Rows[i].Cells[2].Value = info.Limit.ToString();
                datTables.Rows[i].Cells[3].Value = info.BigBlind;
                datTables.Rows[i].Cells[4].Value = info.NbPlayers + "/" + info.NbSeats;
            }
            if (datTables.RowCount > 0 && datTables.SelectedRows.Count > 0)
            {
                datTables.Rows[0].Selected = false;
                datTables.Rows[0].Selected = true;
            }
            OnListRefreshed(this, new EventArgs());
        }
        public void AddTable(bool trainingOnly)
        {
            AddTableForm form = new AddTableForm(m_Server.PlayerName, 1, trainingOnly);
            form.ShowDialog();
            if (form.OK)
            {
                int noPort = m_Server.CreateTable(form.TableName, form.BigBlind, form.NbPlayer, form.WaitingTimeAfterPlayerAction, form.WaitingTimeAfterBoardDealed, form.WaitingTimeAfterPotWon, form.Limit);

                if (noPort != -1)
                {
                    JoinTable(noPort, form.TableName, form.BigBlind);
                    RefreshList();
                }
                else
                {
                    Console.WriteLine("Cannot create table: '" + form.TableName + "'");
                }
            }
        }

        private void LeaveTable(int idGame)
        {
            m_Server.LeaveTable(idGame);
            RefreshList();
        }
        public void LeaveSelected()
        {
            LeaveTable(FindClientId());
        }
        public void JoinSelected()
        {

            if (datTables.RowCount == 0 || datTables.SelectedRows.Count == 0)
            {
                return;
            }
            object o = datTables.SelectedRows[0].Cells[0].Value;
            if (o == null)
                return;
            int noPort = (int)o;
            object o2 = datTables.SelectedRows[0].Cells[1].Value;
            if (o2 == null)
                return;
            string tableName = (string)o2;
            if (FindClient() != null)
                Console.WriteLine("You are already sitting on the table: " + tableName);
            else
            {
                object o3 = datTables.SelectedRows[0].Cells[3].Value;
                if (o3 == null)
                    return;
                int bigBlind = (int)o3;
                if (!JoinTable(noPort, tableName, bigBlind))
                    Console.WriteLine("Table '" + tableName + "' does not exist anymore.");
                RefreshList();

            }
        }
        private bool JoinTable(int p_noPort, String p_tableName, int p_bigBlindAmount)
        {
            AbstractTableForm gui = new TableForm();
            GameClient tcpGame = m_Server.JoinTable(p_noPort, p_tableName, gui);
            gui.FormClosed += delegate
            {
                LeaveTable(p_noPort);
            };
            return true;
        }

        public GameClient FindClient()
        {
            return m_Server.FindClient(FindClientId());
        }

        public int FindClientId()
        {
            if (datTables.RowCount == 0 || datTables.SelectedRows.Count == 0)
            {
                return -1;
            }
            object o = datTables.SelectedRows[0].Cells[0].Value;
            if (o == null)
                return -1;
            return (int)o;
        }

        private void datTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            OnSelectionChanged(this, new EventArgs());
        }

        private void datTables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OnChoiceMade(this, new EventArgs());
        }

        private void datTables_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged(this, new EventArgs());
        }
    }
}