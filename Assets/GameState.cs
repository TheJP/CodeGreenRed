using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public enum Mode { OpenPack, Choosing, Playing, FinishedRound, Menu, AboutToStart };

/// <summary>
/// Keeps track of whether we are in draftmode(player choosing cards), ready to open mode, playing mode
/// </summary>
public class GameState : MonoBehaviour
{
    public Mode State { get; set; }
    public List<PlayerInfo> Players { get; private set; }
    public PlayerInfo CurrPlayer { get { return currPlayerField; } set { PrevPlayer = currPlayerField; currPlayerField = value; } }
    private PlayerInfo currPlayerField;
    public PlayerInfo PrevPlayer { get; private set; }

    public GameState()
    {
        Players = new List<PlayerInfo>();
        State = Mode.Menu;
    }

    public void ResetState()
    {
        Players = new List<PlayerInfo>();
    }
}

