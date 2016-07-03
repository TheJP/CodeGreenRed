using UnityEngine;
using System.Collections;
using System;

public class InvisibleWall : Powerup
{
    public GameObject text;
    public override void PickedUp(PickupParameters parameters)
    {
        parameters.PickedUpBy.Shrink();
        parameters.MoveEventArguments.Cancel();
        if(text != null) { text.SetActive(true); }
    }

    public override void Consumed()
    {
        Destroy(gameObject, 2f);
    }
}
