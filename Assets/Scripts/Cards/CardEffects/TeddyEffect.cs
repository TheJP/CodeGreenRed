using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class TeddyEffect : CardEffect
    {
        public const int AmountOfCheese = 3;
        private Grid grid;
        public TeddyEffect(Grid grid)
        {
            this.grid = grid;
        }

        public override void Execute()
        {
            var player = grid.Players.Shuffle(new System.Random()).Where(p => !p.Dead).FirstOrDefault();
            if (player != null) { player.Shrink(); }
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new TeddyEffect(p.Grid));
        }
    }
}
