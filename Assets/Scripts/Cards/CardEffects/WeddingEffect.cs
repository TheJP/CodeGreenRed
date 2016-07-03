using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class WeddingEffect : CardEffect
    {
        private Player castingSnake;
        public WeddingEffect(Player castingSnake)
        {
            this.castingSnake = castingSnake;
        }

        public override void Execute()
        {
            castingSnake.Grow();
            castingSnake.Move();
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new MaccaroniEffect(p.CastingSnake));
        }
    }
}
