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
    public int delayAfterPlay = 5;
    bool changing = false;

    // Use this for initialization
    void Start () {
        gamestate = GetComponent<GameState>();
    }
    
    // Update is called once per frame
    void Update () {
        if(gamestate.State == Mode.Playing && !changing)
        {
            lastEffectTime -= Time.deltaTime;
            if (lastEffectTime < 0) { lastEffectTime = 0; }

            if(DraftResult.chosenCards.Count == 0)
            {
                //there are no more effects to play
                currentPlayer = 0;
                Invoke("ChangeToFinishedState", delayAfterPlay);
                changing = true;
            }
            else if(lastEffectTime <= 0)
            {
                //play effect
                var cardeffect = DraftResult.chosenCards.Dequeue();
                cardeffect.Execute();
                Destroy(cardeffect);

                //always move after effect
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

    private void ChangeToFinishedState()
    {
        gamestate.State = Mode.FinishedRound;
        changing = false;
    }
}
