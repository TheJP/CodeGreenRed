using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.CardEffects
{
    class CardController
    {
        CardEffectFactory cardEffectFactory = new CardEffectFactory();
        public CardController()
        {
            addFactoriers();

            //usage of factories
            var movementEffect = cardEffectFactory.create<MoveEffect>(new CardEffectParamerters());
            movementEffect.Execute();
        }
        void addFactoriers()
        {
            //TODO could be done with relection
            cardEffectFactory.addFactoryMethod(MoveEffect.GetFactory());
        }

    }
}
