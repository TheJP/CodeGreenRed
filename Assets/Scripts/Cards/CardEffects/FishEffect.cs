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
        private Teams ourTeam;
        public override void Execute()
        {
            foreach (var friend in grid.Players.Where(p => p.Team == ourTeam))
            {
                friend.Grow();
            }
        }
        public override void Initialize(CardEffectParamerters p)
        {
            this.grid = p.Grid;
            this.ourTeam = p.CastingSnake.Team;
        }
    }
}
