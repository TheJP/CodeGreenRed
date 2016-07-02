using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CardEffects
{
    public struct CardEffectParamerters
    {
        public GameObject caster { get;}
        public GameObject[] players; //all except caster
        public Player castingPlayer { get; }



    }
}
