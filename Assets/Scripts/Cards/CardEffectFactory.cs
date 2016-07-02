using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.CardEffects
{
    public delegate T Instantiate<out T>(CardEffectParamerters p) where T : CardEffect;

    class CardEffectFactory
    {
        private Dictionary<Type, Instantiate<CardEffect>> map = new Dictionary<Type, Instantiate<CardEffect>>();

        public void addFactoryMethod<T>(Instantiate<CardEffect> factory) where T : CardEffect
        {
            map.Add(typeof(T), factory);
        }

        public T create<T>(CardEffectParamerters p) where T : CardEffect
        {
            return map[typeof(T)].Invoke(p) as T;
        }

        public CardEffect create(Type t, CardEffectParamerters p)
        {
            if (t.IsSubclassOf(typeof(CardEffect)))
                return map[t].Invoke(p);
            throw new ArgumentException("must supply a subtype of " + typeof(CardEffect));
        }
    }
}
