﻿using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class WeddingEffect : CardEffect
    {
        public override void Execute()
        {
            Snake.Grow();
            Snake.Move();
        }
    }
}
