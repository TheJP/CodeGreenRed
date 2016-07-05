using UnityEngine;
using System.Collections;
using Assets.Scripts.CardEffects;
using Assets.Scripts;

public abstract class CardEffect : Card
{
    protected PlayerInfo CastingPlayer { get; private set; }
    protected Grid Grid { get; private set; }
    protected Player Snake { get { return CastingPlayer.Snake; } }
    protected Teams OurTeam { get { return CastingPlayer.Snake.Team; } }

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
        this.CastingPlayer = p.CastingPlayer;
        this.Grid = p.Grid;
    }
}
