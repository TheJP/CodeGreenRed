using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CardEffects;

public class MoveEffect : CardEffect {

    private Player castingPlayer;

    public override void Execute()
    {
        castingPlayer.Move();
    }

    public static Instantiate<CardEffect> GetFactory()
    {
        return (p => new MoveEffect(p.CastingSnake));
    }

    public MoveEffect(Player caster)
    {
        this.castingPlayer = caster;
    }

}
