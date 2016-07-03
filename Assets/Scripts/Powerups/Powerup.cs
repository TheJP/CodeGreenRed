using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Powerups are items, which will spawn on free map locations.
/// They give buffs or debuffs (depending on the tpye) to the snake that picks it up.
/// </summary>
public abstract class Powerup : MonoBehaviour
{
    public readonly Vector3 PowerupOffset = new Vector3(0.5f, 0.5f);

    public Point Position { get; set; }
    public abstract void PickedUp(PickupParameters parameters);

    protected virtual void Start()
    {
        transform.localPosition = Position.ToVector() + PowerupOffset;
    }

    public virtual void Consumed()
    {
        Destroy(this.gameObject, 0.3f);
    }
}

public class PickupParameters
{
    public Player PickedUpBy { get { return MoveEventArguments.Player; } }
    public Grid Grid { get; private set; }
    public MoveEventArguments MoveEventArguments { get; private set; }
    public PickupParameters(MoveEventArguments arguments, Grid grid)
    {
        this.MoveEventArguments = arguments;
        this.Grid = grid;
    }
}
