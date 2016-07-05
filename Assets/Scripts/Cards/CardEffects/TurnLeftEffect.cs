using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Cards.CardEffects
{
    class TurnLeftEffect : CardEffect
    {
        public override void Execute()
        {
            Snake.TurnLeft();
        }
    }
}
