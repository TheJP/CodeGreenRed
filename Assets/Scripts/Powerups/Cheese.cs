using UnityEngine;
using System.Collections;

/// <summary>Cheese that lets snakes grow. Spawned by the cheese card. ("Cheese everywhere" subtheme)</summary>
public class Cheese : Powerup
{
    public override void PickedUp(PickupParameters parameters)
    {
        parameters.PickedUpBy.Grow();
    }
}
