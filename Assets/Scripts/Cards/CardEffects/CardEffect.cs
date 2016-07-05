using UnityEngine;
using System.Collections;
using Assets.Scripts.CardEffects;
using Assets.Scripts;

public abstract class CardEffect : Card
{
    protected PlayerInfo castingPlayer;
    protected Grid grid;

    /// <summary>
    /// put the cards effect into action
    /// </summary>
    public abstract void Execute();
    /// <summary>
    /// called when cardEffect is chosen during plays
    /// override this method to get access to players, grid etc.
    /// </summary>
    /// <param name="p"></param>
    public virtual void Initialize(CardEffectParamerters p)
    {
        this.castingPlayer = p.CastingPlayer;
        this.grid = p.Grid;
    }
}
