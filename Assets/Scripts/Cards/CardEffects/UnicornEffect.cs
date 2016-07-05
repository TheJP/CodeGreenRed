using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class UnicornEffect : CardEffect
    {
        private Player castingSnake;
        public UnicornEffect(Player castingSnake)
        {
            this.castingSnake = castingSnake;
        }

        public override void Execute()
        {
            castingSnake.Move(2);
        }
    }
}
