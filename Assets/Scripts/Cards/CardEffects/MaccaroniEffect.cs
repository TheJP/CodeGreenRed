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
        public override void Execute()
        {
            castingSnake.Grow();
        }
        public override void Initialize(CardEffectParamerters p)
        {
            base.Initialize(p);
            this.castingSnake = castingPlayer.Snake;
        }
    }
}
