using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MoveEventArguments
{
    public bool Canceled { get; private set; }
    public Player Player { get; private set; }
    public Point TargetPosition { get; private set; }
    public void Cancel() { Canceled = true; }

    public MoveEventArguments(Player player, Point targetPosition)
    {
        this.Player = player;
        this.TargetPosition = targetPosition;
        this.Canceled = false;
    }
}
