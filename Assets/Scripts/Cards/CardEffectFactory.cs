using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.CardEffects
{
    public delegate T Instantiate<out T>(CardEffectParamerters p) where T : CardEffect;

    class CardEffectFactory
    {
        private Dictionary<Type, Instantiate<CardEffect>> map;

        public void addFactoryMethod<T>(Instantiate<T> factory) where T : CardEffect
        {
            map.Add(typeof(T), factory);
        }

        public T create<T>(CardEffectParamerters p) where T : CardEffect
        {
            return map[typeof(T)].Invoke(p) as T;
        }
    }
}
