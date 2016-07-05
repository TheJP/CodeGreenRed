using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CardEffects;

public class MoveEffect : CardEffect
{
    public override void Execute()
    {
        Snake.Move();
    }
}
