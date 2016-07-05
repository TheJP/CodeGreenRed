using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Cards.CardEffects
{
    class TeddyEffect : CardEffect
    {
        public const int AmountOfCheese = 3;
        public override void Execute()
        {
            var player = Grid.Players
                .Where(p => !p.Dead)
                .Shuffle(new Random())
                .FirstOrDefault();
            if (player != null) { player.Shrink(); }
        }
    }
}
