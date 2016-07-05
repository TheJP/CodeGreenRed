using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class FishEffect : CardEffect
    {
        public override void Execute()
        {
            foreach (var friend in Grid.Players.Where(p => p.Team == OurTeam))
            {
                friend.Grow();
            }
        }
    }
}
