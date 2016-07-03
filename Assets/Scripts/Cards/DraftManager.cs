using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.CardEffects;
using Assets.Scripts.Cards.CardEffects;
using Assets.Scripts.Cards;
using System;
using Assets.Scripts;

public class DraftManager : MonoBehaviour
{
    public List<GameObject> cards;
    public Transform SpawnPositionsParent;
    private Transform[] CardSpawnPositions;
    /// <summary>
    /// after minCards is reached, every player has chosen a card in the current draft
    /// </summary>
    public int minCards = 1;


    private GameObject selected = null;
    private Vector2 dragStartPos; //point in world coordinates where the mouse was originally clicked
    private Vector3 mouseLast = Vector3.zero;
    private CardEffectFactory cardEffectFactory = new CardEffectFactory();
    /// <summary>
    /// time left for the user to chose a card
    /// </summary>
    public float TimeLeft { get; private set; }

    private Queue<BoosterPack> toOpen;
    /// <summary>
    /// while are choosing their cards from the booster or playing they should not be disturbed ; OPEN => we are in control :)
    /// </summary>
    private GameState gamestate;

    private CardEffectParamerters effectParams;
    private DraftResult draftResult;
    

    // Use this for initialization
    void Start()
    {
        addFactories();
        gamestate = GetComponent<GameState>();
        Debug.Assert(gamestate != null);
        gamestate.State = Mode.Open;
        TimeLeft = 4;

        CardSpawnPositions = SpawnPositionsParent.GetComponentsInChildren<Transform>();

        //DebugTestDraft();
    }

    public void StartDraft(List<BoosterPack> packs, CardEffectParamerters cardEffectParams)
    {
        toOpen = new Queue<BoosterPack>(packs);
        this.effectParams = cardEffectParams;
        draftResult = new DraftResult();
    }


    // Update is called once per frame
    void Update()
    {
        if (gamestate.State == Mode.Open)
        {
            if(toOpen.Count > 0)
            {
                //open booster and let them users choose some cards
                var booster = toOpen.Dequeue();
                OpenPackAnimation(booster);
                gamestate.State = Mode.Choosing;
            }
            else
            {
                //we're done with drafting, time to play
                cards.ForEach(c => Destroy(c));
                cards.Clear();
                gamestate.State = Mode.Playing;
            }

        }

        if( gamestate.State == Mode.Choosing)
        {
            //we're in control
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0) { TimeIsUp(); }

            CheckMouseSelection();
        }


        //HearthStoneDragRotationTrollolol();
        DebugExecuteOnKeyPress();
    }

    /// <summary>
    /// user exceeded time limit for chosing a card, chose one at random
    /// </summary>
    public void TimeIsUp()
    {
        ResetTimer();
    }

    private void ResetTimer() { TimeLeft = 4; }

    private void OpenPackAnimation(BoosterPack pack)
    {
        Debug.Assert(CardSpawnPositions.Length >= pack.cardsInBooster.Count);
        //open pack
        for (int i = 0; i < pack.cardsInBooster.Count; i++)
        {
            var card = Instantiate(pack.cardsInBooster[i]);
            card.transform.position = CardSpawnPositions[i].position;
            cards.Add(card);
        }
    }

    private void CheckMouseSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if the user pressed mouse check if he selected a card
            var mouse = Input.mousePosition;
            dragStartPos = Camera.main.ScreenToWorldPoint(mouse);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                selected = cards.Find(c => c.transform == hit.transform);

                if (selected != null)
                {
                    //if he did, highlight it
                    Debug.Log("You selected the " + selected.name); // ensure you picked right object
                    selected.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //if selected, deselect card
            if (selected != null)
            {
                OnDeselectCard();
            }
        }
    }


    private void HearthStoneDragRotationTrollolol()
    {
        if (selected != null && Input.mousePosition != mouseLast) // user is holding mousebutton down and card is selected
        {
            Vector2 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 curPos = Camera.main.ScreenToWorldPoint(curScreenPoint);// + offset;
            selected.transform.position = curPos;

            var dragDir = new Vector2(curPos.x - dragStartPos.x, 0);

            var from = selected.transform.rotation;
            Quaternion targetrotation = Quaternion.LookRotation(dragDir);
            //selected.transform.rotation = Quaternion.RotateTowards(selected.transform.rotation, targetrotation, 50 * Time.deltaTime);


        }
        mouseLast = Input.mousePosition;
    }

    private void selectedCardChosenAnimation()
    {
        //some animation
    }
    private void OnDeselectCard()
    {
        //remove outline
        selected.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        //if mouse pointer is still on the card, user chose it
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform == selected.transform)
            {
                //save effect of selected Card
                var effect = selected.GetComponent<Card>().type.GetEffectType();
                draftResult.chosenCards.Enqueue(cardEffectFactory.create(effect, effectParams));
                selectedCardChosenAnimation();
                //remove it from cached list
                cards.Remove(selected);
                //then destroy it
                Destroy(selected);
                selected = null;
                if (cards.Count <= minCards) { gamestate.State = Mode.Open; }
                ResetTimer();
            }
        }

        selected = null;
    }

    //debugging routines
    private void DebugExecuteOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            draftResult.chosenCards.Dequeue().Execute();
            effectParams.CastingPlayer.Grow();

            effectParams.CastingPlayer.Move();
        }
    }
    private void DebugTestDraft()
    {
        var cardpool = GetComponent<CardPool>();
        StartDraft(cardpool.BasicBoosterBox(), new CardEffectParamerters());
    }

    //initialize new Cards here and in Dictionary below
    private void addFactories()
    {
        //UPDATE these entries and cardEffectDictionary below
        //TODO could be done with relection
        cardEffectFactory.addFactoryMethod<MoveEffect>(MoveEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<DebugEffect>(DebugEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<TurnLeftEffect>(TurnLeftEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<TurnRightEffect>(TurnRightEffect.GetFactory());
    }
}

public enum CardType { MOVE, RIGHT, LEFT, DEBUG, Cheese }
/// <summary>
/// Enables us to select cardtype in unity Editor
/// </summary>
public static class CardTypeExtension
{
    static Dictionary<CardType, Type> cardEffectDictionary = new Dictionary<CardType, Type>()
        {
            {CardType.MOVE,typeof(MoveEffect) },
            {CardType.DEBUG,typeof(DebugEffect) },
            {CardType.LEFT,typeof(TurnLeftEffect)},
            {CardType.RIGHT,typeof(TurnRightEffect) }
        };
    public static Type GetEffectType(this CardType cardType)
    {
        return cardEffectDictionary[cardType];
    }

}
