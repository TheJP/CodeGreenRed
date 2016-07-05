using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class NukeEffect : CardEffect
    {
        private Teams ourTeam;
        public override void Execute()
        {
            foreach(var enemy in grid.Players.Where(p => p.Team != ourTeam))
            {
                enemy.Shrink();
            }
        }

        public override void Initialize(CardEffectParamerters p)
        {
            base.Initialize(p);
            ourTeam = castingPlayer.Snake.Team;
        }
    }
}
