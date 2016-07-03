using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class ObamaEffect : CardEffect
    {
        private Grid grid;
        public ObamaEffect(Grid grid)
        {
            this.grid = grid;
        }

        public override void Execute()
        {
            grid.SpawnRandomPowerup();
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new CheeseEffect(p.Grid));
        }
    }
}
