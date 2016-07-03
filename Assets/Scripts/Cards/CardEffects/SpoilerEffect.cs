﻿using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class SpoilerEffect : CardEffect
    {
        private Player castingSnake;
        public SpoilerEffect(Player castingSnake)
        {
            this.castingSnake = castingSnake;
        }

        public override void Execute()
        {
            castingSnake.Move();
        }

        public static Instantiate<CardEffect> GetFactory()
        {
            return (p => new SpoilerEffect(p.CastingSnake));
        }
    }
}