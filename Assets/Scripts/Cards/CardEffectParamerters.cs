using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CardEffects
{
    public class CardEffectParamerters
    {
        public Player CastingPlayer { get; private set; }
        public Grid Grid { get; private set; }

        public CardEffectParamerters(Player castingPlayer, Grid grid)
        {
            this.CastingPlayer = castingPlayer;
            this.Grid = grid;
        }
    }
}
