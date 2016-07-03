using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Cards.CardEffects
{
    class TurnRightEffect : CardEffect
    {
        private Player castingPlayer;

        public TurnRightEffect(Player castingPlayer)
        {
            this.castingPlayer = castingPlayer;
        }

        public override void Execute()
        {
            castingPlayer.TurnRight();
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new TurnRightEffect(p.CastingSnake));
        }
    }
}
