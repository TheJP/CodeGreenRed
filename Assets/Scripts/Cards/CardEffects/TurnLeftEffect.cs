using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Cards.CardEffects
{
    class TurnLeftEffect : CardEffect
    {
        private Player castingPlayer;

        public TurnLeftEffect(Player castingPlayer)
        {
            this.castingPlayer = castingPlayer;
        }

        public override void Execute()
        {
            castingPlayer.TurnLeft();
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new TurnLeftEffect(p.CastingSnake));
        }
    }
}
