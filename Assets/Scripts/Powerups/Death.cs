using UnityEngine;
using System.Collections;

public class Death : Powerup
{
    public override void PickedUp(PickupParameters parameters)
    {
        parameters.PickedUpBy.Die();
    }
}
