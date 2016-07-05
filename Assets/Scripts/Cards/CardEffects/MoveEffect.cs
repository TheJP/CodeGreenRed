using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CardEffects;

public class MoveEffect : CardEffect {

    private Player castingSnake;

    public override void Execute()
    {
        castingSnake.Move();
    }

    public override void Initialize(CardEffectParamerters p)
    {
        base.Initialize(p);
        this.castingSnake = base.castingPlayer.Snake;
    }

}
