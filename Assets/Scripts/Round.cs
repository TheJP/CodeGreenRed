﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// In a Draft every player chooses a Card from a given pool of cards
    /// </summary>
    public class BoosterPack
    {
        public readonly List<CardEffect> cardsInBooster;
        public BoosterPack(List<CardEffect> cards)
        {
            this.cardsInBooster = cards;
        }
    }

    /// <summary>
    /// Every Round may consists of multiple drafts
    /// After a Round is over the CardEffects are executed
    /// </summary>
    public class DraftResult
    {
        public readonly Queue<CardEffect> chosenCards = new Queue<CardEffect>();
    }

    
}
