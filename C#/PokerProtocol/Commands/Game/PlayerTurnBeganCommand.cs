﻿using System.Text;
using EricUtility;
using EricUtility.Networking.Commands;

namespace PokerProtocol.Commands.Game
{
    public class PlayerTurnBeganCommand : AbstractJsonCommand
    {
        public static string COMMAND_NAME = "gamePLAYER_TURN_BEGAN";

        public int PlayerPos { get; set; }
        public int LastPlayerNoSeat { get; set; }
        public int MinimumRaise { get; set; }
    }
}
