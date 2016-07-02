using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CardEffects
{
    public struct CardEffectParamerters
    {
        public GameObject Caster { get; set; }
        public GameObject[] players; //all except caster
        public Player CastingPlayer { get; set; }



    }
}
