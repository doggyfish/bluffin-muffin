﻿using BluffinMuffin.Poker.DataTypes.Enums;
using Com.Ericmas001.Util;
using System;

namespace BluffinMuffin.Poker.DataTypes.EventHandling
{
    public class PokerGameObserver
    {
        private readonly IPokerGame m_Game;

        public event EventHandler EverythingEnded = delegate { };
        public event EventHandler GameBlindNeeded = delegate { };
        public event EventHandler GameEnded = delegate { };
        public event EventHandler GameGenerallyUpdated = delegate { };
        public event EventHandler<RoundEventArgs> GameBettingRoundStarted = delegate { };
        public event EventHandler<RoundEventArgs> GameBettingRoundEnded = delegate { };
        public event EventHandler<PlayerInfoEventArgs> PlayerJoined = delegate { };
        public event EventHandler<PlayerInfoEventArgs> PlayerMoneyChanged = delegate { };
        public event EventHandler<PlayerInfoEventArgs> PlayerHoleCardsChanged = delegate { };
        public event EventHandler<PlayerInfoEventArgs> PlayerLeft = delegate { };
        public event EventHandler<SeatEventArgs> SeatUpdated = delegate { };
        public event EventHandler<HistoricPlayerInfoEventArgs> PlayerActionNeeded = delegate { };
        public event EventHandler<PotWonEventArgs> PlayerWonPot = delegate { };
        public event EventHandler<PlayerActionEventArgs> PlayerActionTaken = delegate { };
        public event IntHandler SitInResponseReceived = delegate { };
        public event BooleanHandler SitOutResponseReceived = delegate { };

        public PokerGameObserver(IPokerGame game)
        {
            m_Game = game;
        }

        public void RaiseEverythingEnded()
        {
            EverythingEnded(m_Game, new EventArgs());
        }
        public void RaiseGameBlindNeeded()
        {
            GameBlindNeeded(m_Game, new EventArgs());
        }
        public void RaiseGameEnded()
        {
            GameEnded(m_Game, new EventArgs());
        }
        public void RaiseGameGenerallyUpdated()
        {
            GameGenerallyUpdated(m_Game, new EventArgs());
        }
        public void RaiseGameBettingRoundStarted(RoundTypeEnum r)
        {
            GameBettingRoundStarted(m_Game, new RoundEventArgs(r));
        }
        public void RaiseGameBettingRoundEnded(RoundTypeEnum r)
        {
            GameBettingRoundEnded(m_Game, new RoundEventArgs(r));
        }
        public void RaisePlayerJoined(PlayerInfo p)
        {
            PlayerJoined(m_Game, new PlayerInfoEventArgs(p));
        }
        public void RaisePlayerMoneyChanged(PlayerInfo p)
        {
            PlayerMoneyChanged(m_Game, new PlayerInfoEventArgs(p));
        }
        public void RaisePlayerHoleCardsChanged(PlayerInfo p)
        {
            PlayerHoleCardsChanged(m_Game, new PlayerInfoEventArgs(p));
        }
        public void RaisePlayerLeft(PlayerInfo p)
        {
            PlayerLeft(m_Game, new PlayerInfoEventArgs(p));
        }
        public void RaiseSeatUpdated(SeatInfo s)
        {
            SeatUpdated(m_Game, new SeatEventArgs(s));
        }
        public void RaisePlayerActionNeeded(PlayerInfo p, PlayerInfo l)
        {
            PlayerActionNeeded(m_Game, new HistoricPlayerInfoEventArgs(p, l));
        }
        public void RaisePlayerWonPot(PlayerInfo p, int id, int amntWon)
        {
            PlayerWonPot(m_Game, new PotWonEventArgs(p, id, amntWon));
        }
        public void RaisePlayerActionTaken(PlayerInfo p, GameActionEnum action, int amnt)
        {
            PlayerActionTaken(m_Game, new PlayerActionEventArgs(p, action, amnt));
        }
        public void RaiseSitInResponseReceived(int seat)
        {
            SitInResponseReceived(seat);
        }
        public void RaiseSitOutResponseReceived(bool success)
        {
            SitOutResponseReceived(success);
        }
    }
}
