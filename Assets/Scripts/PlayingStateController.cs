using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class PlayingStateController : MonoBehaviour {
    public float WaitForAnimationSeconds = 0.5f;

    //gamestate chache
    private GameState gamestate;
    public DraftResult DraftResult { get; set; }
    private float lastEffectTime;
    private int currentPlayer = 0;

    // Use this for initialization
    void Start () {
        gamestate = GetComponent<GameState>();
    }
    
    // Update is called once per frame
    void Update () {
        if(gamestate.State == Mode.Playing)
        {
            lastEffectTime -= Time.deltaTime;
            if (lastEffectTime < 0) { lastEffectTime = 0; }

            if(DraftResult.chosenCards.Count == 0)
            {
                //there are no more effects to play
                gamestate.State = Mode.FinishedRound;
                currentPlayer = 0;
            }
            else if(lastEffectTime <= 0)
            {
                //play effect
                DraftResult.chosenCards.Dequeue().Execute();
                var snake = NextPlayer().Snake;
                snake.Move();
                //wait a bit for the effect animation
                lastEffectTime = WaitForAnimationSeconds;
            }
        }
    
    }
    private PlayerInfo NextPlayer()
    {
        var info = gamestate.Players[currentPlayer];
        currentPlayer = (currentPlayer + 1) % gamestate.Players.Count;
        return info;
    }
}
