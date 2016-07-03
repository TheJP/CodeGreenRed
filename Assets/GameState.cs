using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

/// <summary>
/// Keeps track of whether we are in draftmode(player choosing cards), ready to open mode, playing mode
/// </summary>
public class GameState : MonoBehaviour {

    private Mode state = Mode.Menu;
    public Mode State {
        get { return state; }
        set { state = value; }
        }
    public List<PlayerInfo> Players { get; set; }
    

    void Awake()
    {
        Players = new List<PlayerInfo>();
        State = Mode.Menu;
    }
}
public enum Mode { OpenPack, Choosing, Playing, FinishedRound, Menu, AboutToStart };

