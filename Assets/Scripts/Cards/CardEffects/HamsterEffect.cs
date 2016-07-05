using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class HamsterEffect : CardEffect
    {
        public override void Execute()
        {
            var random = new System.Random();
            foreach (var player in grid.Players.Shuffle(random))
            {
                player.Move();
            }
        }
    }
}
