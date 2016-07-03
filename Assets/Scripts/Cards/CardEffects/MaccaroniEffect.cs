using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class MaccaroniEffect : CardEffect
    {
        private Player castingSnake;
        public MaccaroniEffect(Player castingSnake)
        {
            this.castingSnake = castingSnake;
        }

        public override void Execute()
        {
            castingSnake.Grow();
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new MaccaroniEffect(p.CastingSnake));
        }
    }
}
