using System;
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
        public readonly List<GameObject> cardsInBooster;
        public BoosterPack(List<GameObject> cards)
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
        public Queue<CardEffect> chosenCards = new Queue<CardEffect>(30);
    }

    
}
