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
    public PlayerInfo CurrentPlayer { get; private set; }
    public PlayerInfo PreviousPlayer { get; private set; }

    public GameState()
    {
        Players = new List<PlayerInfo>();
        State = Mode.Menu;
    }

    public void ResetState()
    {
        Players = new List<PlayerInfo>();
        PreviousPlayer = null;
        CurrentPlayer = null;
    }

    public void SelectNewCurrentPlayer(PlayerInfo newPlayer)
    {
        PreviousPlayer = CurrentPlayer;
        CurrentPlayer = newPlayer;
    }
}

