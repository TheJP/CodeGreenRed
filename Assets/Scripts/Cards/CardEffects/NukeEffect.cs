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
        private Grid grid;
        private Teams ourTeam;
        public NukeEffect(Grid grid, Teams ourTeam)
        {
            this.grid = grid;
            this.ourTeam = ourTeam;
        }

        public override void Execute()
        {
            foreach(var enemy in grid.Players.Where(p => p.Team != ourTeam))
            {
                enemy.Shrink();
            }
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new NukeEffect(p.Grid, p.CastingSnake.Team));
        }
    }
}
