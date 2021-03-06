﻿using Com.Ericmas001.Net.Protocol.JSON;
using BluffinMuffin.Poker.DataTypes.Enums;

namespace BluffinMuffin.Protocol.Commands.Game
{
    public class PlayerTurnEndedCommand : AbstractJsonCommand
    {
        public int PlayerPos { get; set; }
        public int PlayerBet { get; set; }
        public int PlayerMoney { get; set; }
        public int TotalPot { get; set; }
        public GameActionEnum ActionType { get; set; }
        public int ActionAmount { get; set; }
        public PlayerStateEnum State { get; set; }
    }
}
