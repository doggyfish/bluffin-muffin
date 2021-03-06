﻿using BluffinMuffin.Poker.DataTypes.EventHandling;

namespace BluffinMuffin.Poker.DataTypes
{
    public interface IPokerGame
    {
        PokerGameObserver Observer { get; }

        TableInfo Table { get; }

        bool PlayMoney(PlayerInfo p, int amnt);
        bool LeaveGame(PlayerInfo p);
        int SitIn(PlayerInfo p, int noSeat = -1, int moneyAmount = 1500);
        bool SitOut(PlayerInfo p);

        string Encode { get; }

        bool IsPlaying { get; }
    }
}
