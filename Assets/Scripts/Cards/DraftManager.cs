﻿using Assets.Scripts;
using Assets.Scripts.CardEffects;
using Assets.Scripts.Cards;
using Assets.Scripts.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameState))]
public class DraftManager : MonoBehaviour
{
    //unity editor dependencies
    public List<Card> cards;
    public Transform SpawnPositionsParent;
    //how long before chosing card at random
    public int thinkingTime = 6;
    //UI text that indicates which is the current player
    public Text playerText;

    //stuff for sharing gamestate infos
    /// <summary>
    /// time left for the user to chose a card
    /// </summary>
    public float TimeLeft { get; private set; }

    private Transform[] CardSpawnPositions;
    private Card selected = null;
    //private Vector2 dragStartPos; //point in world coordinates where the mouse was originally clicked
    private Vector3 mouseLast = Vector3.zero;
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
        gamestate = GetComponent<GameState>();
        Debug.Assert(gamestate != null);
        ResetTimer();
        playerText.GetComponent<Text>().text = "Player : " + (currentPlayer + 1) + " 's turn ";
        CardSpawnPositions = SpawnPositionsParent.GetComponentsInChildren<Transform>();
    }

    public void StartDraft(List<BoosterPack> packs, Grid grid)
    {
        toOpen = new Queue<BoosterPack>(packs);
        this.grid = grid;
        draftResult = new DraftResult();
        currentPlayer = -1; //openpacks will increment and then change player
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
                ResetTimer();
            }
            else
            {
                //we're done with drafting, time to play
                GetComponent<PlayingStateController>().DraftResult = draftResult;
                gamestate.CurrentPlayer.Snake.Select(false);
                gamestate.State = Mode.Playing;
                draftResult = new DraftResult();
            }

        }
        else
        if (gamestate.State == Mode.Choosing)
        {
            //we're in control
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0) { TimeIsUp(); }

            CheckMouseSelection();
            if (gamestate.CurrentPlayer.Snake.Dead)
            {
                NextPlayer();
                ResetTimer();
            }
        }
    }

    /// <summary>
    /// user exceeded time limit for chosing a card, chose one at random
    /// </summary>
    public void TimeIsUp()
    {
        selected = cards.Shuffle(new System.Random()).First();
        //save effect of selected Card
        CreateAndAddChosenCard();
    }

    /// <summary>
    /// if every player has chosen a card, destroy the remaining
    /// </summary>
    private void CheckDraftDone()
    {
        var deadCount = gamestate.Players.FindAll(p => p.Snake.Dead).Count;
        if (cards.Count - deadCount <= cardsPerPack - gamestate.Players.Count)
        {
            //done choosing cards
            cards.ForEach(c => { Destroy(c.gameObject); });
            cards.Clear();
            gamestate.State = Mode.OpenPack;
        }
    }

    private void OnCurrentPlayerChange()
    {
        if (gamestate.PreviousPlayer != null) { gamestate.PreviousPlayer.Snake.Select(false); }
        gamestate.CurrentPlayer.Snake.Select(true);
        playerText.text = "Player : " + (currentPlayer + 1) + " 's turn ";
    }
    private void ResetTimer() { TimeLeft = thinkingTime; }

    private void OpenPackAnimation(BoosterPack pack)
    {
        Debug.Assert(CardSpawnPositions.Length >= pack.cardsInBooster.Count);
        //open pack
        for (int i = 0; i < pack.cardsInBooster.Count; i++)
        {
            var card = Instantiate(pack.cardsInBooster[i]);
            card.gameObject.transform.position = CardSpawnPositions[i].position;
            cards.Add(card);
        }

        NextPlayer();
    }

    private void CheckMouseSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //if the user pressed mouse check if he selected a card
            var mouse = Input.mousePosition;
            //dragStartPos = Camera.main.ScreenToWorldPoint(mouse);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                selected = cards.Find(c => c.gameObject.transform == hit.transform);

                if (selected != null)
                {
                    //if he did, highlight it
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

            //var dragDir = new Vector2(curPos.x - dragStartPos.x, 0);

            //var from = selected.transform.rotation;
            //Quaternion targetrotation = Quaternion.LookRotation(dragDir);
            //selected.transform.rotation = Quaternion.RotateTowards(selected.transform.rotation, targetrotation, 50 * Time.deltaTime);
        }
        mouseLast = Input.mousePosition;
    }

    private void SelectedCardChosenAnimation()
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
                CreateAndAddChosenCard();
            }
        }
        selected = null;
    }

    private void CreateAndAddChosenCard()
    {
        //save effect of selected Card
        var castingPlayer = gamestate.CurrentPlayer;
        var cardeffectParams = new CardEffectParamerters(castingPlayer, grid);
        var effect = selected.GetComponent<CardEffect>();
        selected.gameObject.SetActive(false);
        effect.Initialize(cardeffectParams);
        draftResult.chosenCards.Enqueue(effect);
        SelectedCardChosenAnimation();
        //remove it from cached list
        cards.Remove(selected);
        selected = null;
        NextPlayer();

        CheckDraftDone();
        ResetTimer();
    }

    private void NextPlayer()
    {
        currentPlayer = (currentPlayer + 1) % gamestate.Players.Count;
        gamestate.SelectNewCurrentPlayer(gamestate.Players[currentPlayer]);
        OnCurrentPlayerChange();
        if (gamestate.CurrentPlayer.Snake.Dead)
        {
            NextPlayer();
            ResetTimer();
        }
    }
}
