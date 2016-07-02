using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The "unlimited love" (one of the epic subthemes) powerup grants a length bonus.
/// </summary>
public class UnlimitedLove : Powerup
{
    public override void PickedUp(PickupParameters parameters)
    {
        parameters.PickedUpBy.Grow();
    }
}
