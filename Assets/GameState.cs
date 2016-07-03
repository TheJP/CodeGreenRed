using UnityEngine;
using System.Collections;

/// <summary>
/// Keeps track of whether we are in draftmode(player choosing cards), ready to open mode, playing mode
/// </summary>
public class GameState : MonoBehaviour {
    public Mode State { get; set; }
}
public enum Mode { OPEN, CHOOSING, PLAYING };

