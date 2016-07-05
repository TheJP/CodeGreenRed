using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Cards.CardEffects
{
    class TurnLeftEffect : CardEffect
    {
        private Player castingSnake;

        public TurnLeftEffect(Player castingPlayer)
        {
            this.castingSnake = castingPlayer;
        }

        public override void Execute()
        {
            castingSnake.TurnLeft();
        }

        public override void Initialize(CardEffectParamerters p)
        {
            base.Initialize(p);
            castingSnake = castingPlayer.Snake;
        }
    }
}
