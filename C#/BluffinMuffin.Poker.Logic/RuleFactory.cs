﻿using BluffinMuffin.Poker.DataTypes;
using BluffinMuffin.Poker.DataTypes.Enums;
using System.Collections.Generic;

namespace BluffinMuffin.Poker.Logic
{
    public static class RuleFactory
    {
        public static RuleInfo[] SupportedRules
        {
            get
            {
                //The order here is important! The most important game should be at the top, and so on.
                return new[]
                {
                    new RuleInfo()
                    {
                        Name = "Texas Hold'em",
                        GameType = GameTypeEnum.Holdem,
                        MinPlayers = 2,
                        MaxPlayers = 10,
                        AvailableLimits = new List<LimitTypeEnum>(){LimitTypeEnum.NoLimit/*,LimitTypeEnum.FixedLimit,LimitTypeEnum.PotLimit*/ },
                        DefaultLimit = LimitTypeEnum.NoLimit,
                        AvailableBlinds = new List<BlindTypeEnum>(){BlindTypeEnum.Blinds, BlindTypeEnum.Antes, BlindTypeEnum.None},
                        DefaultBlind = BlindTypeEnum.Blinds,
                        CanConfigWaitingTime = true,
                        AvailableLobbys = new List<LobbyTypeEnum>(){LobbyTypeEnum.Training, LobbyTypeEnum.Career},
                    }
                };
            }
        }
    }
}
