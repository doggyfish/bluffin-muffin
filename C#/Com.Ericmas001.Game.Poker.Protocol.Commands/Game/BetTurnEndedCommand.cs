﻿using System.Collections.Generic;
using Com.Ericmas001.Net.Protocol.JSON;
using Com.Ericmas001.Game.Poker.DataTypes.Enums;

namespace Com.Ericmas001.Game.Poker.Protocol.Commands.Game
{
    public class BetTurnEndedCommand : AbstractJsonCommand
    {
        public RoundTypeEnum Round { get; set; }
        public List<int> PotsAmounts { get; set; }
    }
}
