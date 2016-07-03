﻿﻿using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.CardEffects;
using Assets.Scripts.Cards.CardEffects;
using Assets.Scripts.Cards;
using System;
using Assets.Scripts;
using UnityEngine.UI;

public class DraftManager : MonoBehaviour
{
    //unity editor dependencies
    public List<GameObject> cards;
    public Transform SpawnPositionsParent;
    //how long before chosing card at random
    public int thinkingTime = 6;
    private Transform[] CardSpawnPositions;

    public GameObject playerText;


    //stuff for sharing gamestate infos
    /// <summary>
    /// time left for the user to chose a card
    /// </summary>
    public float TimeLeft { get; private set; }

    private GameObject selected = null;
    private Vector2 dragStartPos; //point in world coordinates where the mouse was originally clicked
    private Vector3 mouseLast = Vector3.zero;
    private CardEffectFactory cardEffectFactory = new CardEffectFactory();
    private int currentPlayer = 0;
    /// <summary>
    /// boosterpacks which must be opened during this draft round, if 0 => switch to playing mode
    /// </summary>
    private Queue<BoosterPack> toOpen;
    /// <summary>
    /// while are choosing their cards from the booster or playing they should not be disturbed ; OPEN => we are in control :)
    /// </summary>
    private GameState gamestate;
    private Grid grid;
    private DraftResult draftResult;
    /// <summary>
    /// after minCards is reached, every player has chosen a card in the current draft
    /// </summary>
    private const int cardsPerPack = 5;


    // Use this for initialization
    void Start()
    {
        addFactories();
        gamestate = GetComponent<GameState>();
        Debug.Assert(gamestate != null);
        ResetTimer();
        playerText.GetComponent<Text>().text = "Player : " + (currentPlayer + 1) + " 's turn ";
        CardSpawnPositions = SpawnPositionsParent.GetComponentsInChildren<Transform>();

        //DebugTestDraft();
    }

    public void StartDraft(List<BoosterPack> packs, Grid grid)
    {
        toOpen = new Queue<BoosterPack>(packs);
        this.grid = grid;
        draftResult = new DraftResult();
    }


    // Update is called once per frame
    void Update()
    {
        if (gamestate.State == Mode.OpenPack)
        {
            if (toOpen.Count > 0)
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
                GetComponent<PlayingStateController>().DraftResult = draftResult;
                draftResult = new DraftResult();
            }

        }

        if (gamestate.State == Mode.Choosing)
        {
            //we're in control
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0) { TimeIsUp(); }

            CheckMouseSelection();
            //DebugExecuteOnKeyPress();
        }
        //HearthStoneDragRotationTrollolol();
        
    }

    /// <summary>
    /// user exceeded time limit for chosing a card, chose one at random
    /// </summary>
    public void TimeIsUp()
    {
        var selected = cards[0];

        //save effect of selected Card
        var castingPlayer = NextPlayer();
        var cardeffectParams = new CardEffectParamerters(castingPlayer, grid);
        var effect = selected.GetComponent<Card>().type.GetEffectType();
        draftResult.chosenCards.Enqueue(cardEffectFactory.create(effect, cardeffectParams));
        selectedCardChosenAnimation();
        //remove it from cached list
        cards.Remove(selected);
        //then destroy it
        Destroy(selected);
        selected = null;
        CheckDraftDone();
        ResetTimer();
    }

    /// <summary>
    /// if every player has chosen a card, destroy the remaining
    /// </summary>
    private void CheckDraftDone()
    {
        if (cards.Count <= cardsPerPack - gamestate.Players.Count)
        {
            //done choosing cards
            cards.ForEach(c => Destroy(c));
            cards.Clear();
            gamestate.State = Mode.OpenPack;
        }
    }

    private void ResetTimer() { TimeLeft = thinkingTime; }

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
                    selected.GetComponent<Card>().Highlight(true);

                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //if selected, deselect card and add it to chosencards if necessary
            if (selected != null)
            {
                OnDeselectCard();

                playerText.GetComponent<Text>().text = "Player : " + (currentPlayer + 1) + " 's turn ";
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
        selected.GetComponent<Card>().Highlight(false);

        //if mouse pointer is still on the card, user chose it
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform == selected.transform)
            {
                //save effect of selected Card
                var castingPlayer = NextPlayer();
                var cardeffectParams = new CardEffectParamerters(castingPlayer, grid);
                var effect = selected.GetComponent<Card>().type.GetEffectType();
                draftResult.chosenCards.Enqueue(cardEffectFactory.create(effect, cardeffectParams));
                selectedCardChosenAnimation();
                //remove it from cached list
                cards.Remove(selected);
                //then destroy it
                Destroy(selected);
                selected = null;
                CheckDraftDone();
                ResetTimer();
            }
        }

        selected = null;
    }

    private PlayerInfo NextPlayer()
    {
        var info = gamestate.Players[currentPlayer];
        currentPlayer = (currentPlayer + 1) % gamestate.Players.Count;
        return info;
    }

    //debugging routines
    private void DebugExecuteOnKeyPress()
    {
        Debug.Log(gamestate.Players.Count);
        var player = gamestate.Players[currentPlayer];
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ChosenCards.Dequeue().Execute();
            player.Snake.Grow();
            player.Snake.Move();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {   
            //switch player
            NextPlayer();

        }
    }
    //private void DebugTestDraft()
    //{
    //    var cardpool = GetComponent<CardPool>();
    //    StartDraft(cardpool.BasicBoosterBox(), new CardEffectParamerters());
    //}

    //initialize new Cards here and in Dictionary below
    private void addFactories()
    {
        //UPDATE these entries and cardEffectDictionary below
        //TODO could be done with relection
        cardEffectFactory.addFactoryMethod<MoveEffect>(MoveEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<DebugEffect>(DebugEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<TurnLeftEffect>(TurnLeftEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<TurnRightEffect>(TurnRightEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<CheeseEffect>(CheeseEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<ObamaEffect>(ObamaEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<SpoilerEffect>(SpoilerEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<HamsterEffect>(HamsterEffect.GetFactory());
    }
}

public enum CardType { Move, Right, Left, Debug, Cheese, Obama, Spoiler, Hamster }
/// <summary>
/// Enables us to select cardtype in unity Editor
/// </summary>
public static class CardTypeExtension
{
    static Dictionary<CardType, Type> cardEffectDictionary = new Dictionary<CardType, Type>()
    {
        { CardType.Move, typeof(MoveEffect) },
        { CardType.Debug, typeof(DebugEffect) },
        { CardType.Left, typeof(TurnLeftEffect) },
        { CardType.Right, typeof(TurnRightEffect) },
        { CardType.Cheese, typeof(CheeseEffect) },
        { CardType.Obama, typeof(ObamaEffect) },
        { CardType.Spoiler, typeof(SpoilerEffect) },
        { CardType.Hamster, typeof(HamsterEffect) }
    };
    public static Type GetEffectType(this CardType cardType)
    {
        return cardEffectDictionary[cardType];
    }

}
