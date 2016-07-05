using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Cards.CardEffects
{
    class DebugEffect : CardEffect
    {
        public override void Execute()
        {
            Debug.Log("called execute on DebugEffect");
        }

        public override void Initialize(CardEffectParamerters effectParams)
        {
            // do nothing
        }
    }
}
