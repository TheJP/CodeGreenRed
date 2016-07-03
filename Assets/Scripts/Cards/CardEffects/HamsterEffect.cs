using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class HamsterEffect : CardEffect
    {
        private Grid grid;
        public HamsterEffect(Grid grid)
        {
            this.grid = grid;
        }

        public override void Execute()
        {
            var random = new System.Random();
            foreach (var player in grid.Players.Shuffle(random))
            {
                player.Move();
            }
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new HamsterEffect(p.Grid));
        }
    }
}
