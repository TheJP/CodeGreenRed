using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{

    public class PlayerInfo
    {
        public uint PlayerID { get; private set; }
        public Player Snake{ get; set; }

        public PlayerInfo(uint number)
        {
            PlayerID = number;
            Snake = null;
        }
    }
}
