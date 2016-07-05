using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Cards.CardEffects
{
    class HamsterEffect : CardEffect
    {
        public override void Execute()
        {
            foreach (var player in Grid.Players.Shuffle(new Random()))
            {
                player.Move();
            }
        }
    }
}
