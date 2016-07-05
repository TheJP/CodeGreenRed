using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(GameState))]
public class PlayingStateController : MonoBehaviour
{
    public float waitForAnimationSeconds = 0.5f;
    public float delayAfterPlay = 5f;

    //gamestate chache
    private GameState gamestate;
    public DraftResult DraftResult { get; set; }
    private float lastEffectTime;
    private bool changing = false;

    void Start()
    {
        gamestate = GetComponent<GameState>();
    }

    void Update()
    {
        if (gamestate.State == Mode.Playing && !changing)
        {
            lastEffectTime -= Time.deltaTime;
            if (lastEffectTime < 0) { lastEffectTime = 0; }

            if (DraftResult.chosenCards.Count == 0)
            {
                //there are no more effects to play
                Invoke("ChangeToFinishedState", delayAfterPlay);
                changing = true;
            }
            else if (lastEffectTime <= 0)
            {
                //play effect
                var cardeffect = DraftResult.chosenCards.Dequeue();
                cardeffect.Execute();
                //always move after effect
                cardeffect.CastingPlayer.Snake.Move();
                Destroy(cardeffect.gameObject);

                //wait a bit for the effect animation
                lastEffectTime = waitForAnimationSeconds;
            }
        }
    }

    private void ChangeToFinishedState()
    {
        gamestate.State = Mode.FinishedRound;
        changing = false;
    }
}
