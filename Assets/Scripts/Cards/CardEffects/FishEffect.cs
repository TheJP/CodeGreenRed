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
        private Grid grid;
        private Teams ourTeam;
        public FishEffect(Grid grid, Teams ourTeam)
        {
            this.grid = grid;
            this.ourTeam = ourTeam;
        }

        public override void Execute()
        {
            foreach(var friend in grid.Players.Where(p => p.Team == ourTeam))
            {
                friend.Grow();
            }
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new FishEffect(p.Grid, p.CastingSnake.Team));
        }
    }
}
