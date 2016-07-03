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
        private Grid grid;
        public HamsterEffect(Grid grid)
        {
            this.grid = grid;
        }

        public override void Execute()
        {
            var random = new System.Random();
            foreach (var player in Shuffle(grid.Players, random))
            {
                player.Move();
            }
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new HamsterEffect(p.Grid));
        }

        private static IEnumerable<T> Shuffle<T>(IEnumerable<T> source, System.Random rng)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }
}
