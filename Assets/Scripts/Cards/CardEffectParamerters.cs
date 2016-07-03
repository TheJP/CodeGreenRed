using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CardEffects
{
    public class CardEffectParamerters
    {
        public Player CastingSnake { get; private set; }
        public Grid Grid { get; private set; }
        public PlayerInfo CastingPlayer { get; private set; }

        public CardEffectParamerters(PlayerInfo castingPlayer, Grid grid)
        {
            this.CastingSnake = castingPlayer.Snake;
            this.Grid = grid;
            this.CastingPlayer = castingPlayer;
        }
    }
}
