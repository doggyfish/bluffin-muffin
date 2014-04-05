﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using EricUtility;
using EricUtility.Collections;
using EricUtility.Networking;
using EricUtility.Networking.Commands;
using Com.Ericmas001.Game.Poker.Protocol.Commands.Lobby;
using PokerWorld.Game;
using Com.Ericmas001.Game.Poker.Protocol.Commands.Lobby.Training;
using Com.Ericmas001.Game.Poker.Protocol.Commands.Lobby.Career;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Web;
using Com.Ericmas001.Game.Poker.Protocol.Commands;
using PokerWorld.Game.Rules;
using Com.Ericmas001.Game.Poker.Protocol.Commands.Entities;
using PokerWorld.Game.Enums;
using System.Runtime.Serialization.Formatters;

namespace Com.Ericmas001.Game.Poker.Protocol.Client
{
    public delegate void DisconnectDelegate();
    public class LobbyTCPClient : TCPCommunicator
    {
        #region Fields
        protected string m_PlayerName;
        protected string m_ServerAddress;
        protected int m_ServerPort;
        protected Dictionary<int, GameClient> m_Clients = new Dictionary<int, GameClient>();
        protected BlockingQueue<string> m_Incoming = new BlockingQueue<string>();
        #endregion Fields

        #region Events
        public event DisconnectDelegate ServerLost = delegate{};
        #endregion Events

        #region Properties
        public string PlayerName { get { return m_PlayerName; } }
        public string ServerAddress { get { return m_ServerAddress; } }
        public int ServerPort { get { return m_ServerPort; } }
        #endregion Properties

        #region Ctors & Init
        public LobbyTCPClient(string serverAddress, int serverPort)
            : base()
        {
            m_ServerAddress = serverAddress;
            m_ServerPort = serverPort;
        }
        #endregion Ctors & Init

        #region GameClient Event Handler
        protected void client_SendedSomething(object sender, KeyEventArgs<string> e)
        {
            Send(new GameCommand() { TableID = ((GameClient)sender).NoPort, EncodedCommand = e.Key });
        }

        #endregion GameClient Event Handler

        #region Public Methods
        public bool Connect()
        {
            return base.Connect(m_ServerAddress, m_ServerPort);
        }

        public void LeaveTable(int idGame)
        {
            if (m_Clients.ContainsKey(idGame))
            {
                GameClient client = m_Clients[idGame];

                m_Clients.Remove(idGame);

                if (client != null)
                    client.Disconnect();
            }
        }

        public override void OnReceiveCrashed(Exception e)
        {
            if (e is IOException)
            {
                LogManager.Log(LogLevel.Error, "LobbyTCPClient.OnReceiveCrashed", "Lobby lost connection with server");
                Disconnect();
            }
            else
                base.OnReceiveCrashed(e);
        }

        public override void OnSendCrashed(Exception e)
        {
            if (e is IOException)
            {
                LogManager.Log(LogLevel.Error, "LobbyTCPClient.OnReceiveCrashed", "Lobby lost connection with server");
                Disconnect();
            }
            else
                base.OnSendCrashed(e);
        }

        public void Send(StreamWriter writer, AbstractCommand command)
        {
            writer.WriteLine(command.Encode());
        }

        public void Send(AbstractCommand command)
        {
            string encode = command.Encode();
            base.Send(encode);
        }

        public void Disconnect()
        {
            foreach (GameClient client in m_Clients.Values)
                client.Disconnect();

            m_Clients.Clear();

            if (IsConnected)
            {
                Send(new DisconnectCommand());
                Close();
            }
        }

        public GameClient FindClient(int noPort)
        {
            if (m_Clients.ContainsKey(noPort))
                return m_Clients[noPort];

            return null;
        }

        public GameClient JoinTable(int p_noPort, string p_tableName, IPokerViewer gui)
        {
            int noSeat = GetJoinedSeat(p_noPort, m_PlayerName);
            if (noSeat == -1)
            {
                LogManager.Log(LogLevel.MessageLow, "LobbyTCPClient.JoinTable", "Cannot sit at this table: {0}", p_tableName);
                return null;
            }

            GameClient client = new GameClient(noSeat, m_PlayerName, p_noPort);
            client.SendedSomething += new EventHandler<KeyEventArgs<string>>(client_SendedSomething);

            if (gui != null)
            {
                gui.SetGame(client, client.NoSeat);
                gui.Start();
            }

            client.Start();

            m_Clients.Add(p_noPort, client);

            return client;
        }

        public int CreateTable(GameRule gr)
        {
            Send(new CreateTableCommand() { GameRules = gr });

            return WaitAndReceive<CreateTableResponse>().IdTable;
        }

        #endregion Public Methods

        #region Protected Methods
        protected JObject WaitAndReceive(string expected)
        {
            string s;
            string commandName;

            JObject jObj;

            do
            {
                s = m_Incoming.Dequeue();
                jObj = JsonConvert.DeserializeObject<dynamic>(s, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple });
                commandName = (string)jObj["CommandName"];
            }
            while (s != null && commandName != expected);

            return jObj;
        }
        protected T WaitAndReceive<T>() where T : AbstractCommand
        {
            string expected = (string)typeof(T).GetField(AbstractCommand.CommandNameField, (BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public)).GetValue(null);
            string s;
            string commandName;

            JObject jObj;

            do
            {
                s = m_Incoming.Dequeue();
                jObj = JsonConvert.DeserializeObject<dynamic>(s);
                commandName = (string)jObj["CommandName"];
            }
            while (s != null && commandName != expected);

            return JsonConvert.DeserializeObject<T>(s, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple });
        }

        protected string Receive(StreamReader reader)
        {
            string line;
            try
            {
                line = reader.ReadLine();
                LogManager.Log(LogLevel.MessageLow, "LobbyTCPClient.Receive", "{0} RECV [{1}]", m_PlayerName, line);
            }
            catch
            {
                return null;
            }
            return line;
        }

        protected virtual int GetJoinedSeat(int p_noPort, string player)
        {
            Send(new JoinTableCommand()
            {
                TableID = p_noPort,
                PlayerName = player,
            });

            return WaitAndReceive<JoinTableResponse>().NoSeat;
        }

        public List<RuleInfo> GetSupportedRules()
        {
            SupportedRulesCommand cmd = new SupportedRulesCommand();
            Send(cmd);

            return WaitAndReceive<SupportedRulesResponse>().Rules;
        }

        protected override void Run()
        {
            while (IsConnected)
            {
                LogManager.Log(LogLevel.MessageVeryLow, "LobbyTCPClient.Run", "{0} IS WAITING", m_PlayerName);

                string line = Receive();
                if (line == null)
                {
                    ServerLost();
                    return;
                }

                LogManager.Log(LogLevel.MessageLow, "LobbyTCPClient.Run", "{0} RECV [{1}]", m_PlayerName, line);

                JObject jObj = JsonConvert.DeserializeObject<dynamic>(line);
                String cmdName = (string)jObj["CommandName"];

                if (cmdName == GameCommand.COMMAND_NAME)
                {
                    GameCommand c = JsonConvert.DeserializeObject<GameCommand>(line, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple });
                    int count = 0;

                    //Be patient
                    while (!m_Clients.ContainsKey(c.TableID) && (count++ < 5))
                        Thread.Sleep(100);

                    if (m_Clients.ContainsKey(c.TableID))
                        m_Clients[c.TableID].Incoming(c.DecodedCommand);
                }
                else
                    m_Incoming.Enqueue(line);
            }
        }

        public List<TableInfo> ListTables(params LobbyTypeEnum[] lobbyTypes)
        {
            Send(new ListTableCommand() { LobbyTypes = lobbyTypes });

            return WaitAndReceive<ListTableResponse>().Tables;
        }

        #endregion Protected Methods
    }
}