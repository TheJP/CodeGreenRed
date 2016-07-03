using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

/// <summary>
/// Keeps track of whether we are in draftmode(player choosing cards), ready to open mode, playing mode
/// </summary>
public class GameState : MonoBehaviour {
    public Mode State;
    public List<PlayerInfo> Players { get; set; }
    

    void Awake()
    {
        Players = new List<PlayerInfo>();
        State = Mode.FinishedRound;
    }
}
public enum Mode { OpenPack, Choosing, Playing, FinishedRound };

