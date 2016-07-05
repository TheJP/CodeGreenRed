using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class CheeseEffect : CardEffect
    {
        public const int AmountOfCheese = 3;
        public CheeseEffect(Grid grid)
        {
            this.grid = grid;
        }

        public override void Execute()
        {
            for (int i = 0; i < AmountOfCheese; ++i) { grid.SpawnPowerup<Cheese>(); }
        }

        public override void Initialize(CardEffectParamerters effectParams)
        {
            grid = effectParams.Grid;
        }
    }
}
