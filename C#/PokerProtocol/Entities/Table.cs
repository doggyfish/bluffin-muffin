﻿using System;
using System.Collections.Generic;
using System.Text;
using PokerWorld.Game;
using EricUtility;
using PokerProtocol.Entities.Enums;
using PokerWorld.Game.Enums;
using Newtonsoft.Json.Linq;
using PokerWorld.Game.Rules;

namespace PokerProtocol.Entities
{
    public class Table : IComparable<Table>
    {
        public int IdTable { get; set; }
        public int BigBlind { get; set; }
        public int NbPlayers { get; set; }
        public LobbyActionEnum PossibleAction { get; set; }
        public GameRule Rules { get; set; }

        public Table(int idTable, GameRule rules, int nbPlayers, LobbyActionEnum possibleAction)
        {
            IdTable = idTable;
            Rules = rules;
            NbPlayers = nbPlayers;
            PossibleAction = possibleAction;
        }

        public Table()
        {
        }

        public int CompareTo(Table other)
        {
            return IdTable.CompareTo(other.IdTable);
        }
    }
}
