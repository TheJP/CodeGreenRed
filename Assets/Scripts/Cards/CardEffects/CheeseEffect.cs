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
        public override void Execute()
        {
            for (int i = 0; i < AmountOfCheese; ++i) { Grid.SpawnPowerup<Cheese>(); }
        }
    }
}
