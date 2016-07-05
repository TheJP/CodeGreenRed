using Assets.Scripts;
using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(GameState))]
public class GameController : MonoBehaviour
{

    private GameState gamestate;
    public Canvas playerInfoCanvas;
    public SelectionMenu menu;
    public float waitSecondsUntilMainMenu = 2f; //on finished game

    public Grid Grid { get; private set; }
    private DraftManager draftManager;
    private CardPool cardPoolManager;
    public bool GameOver { get; set; }

    void Start()
    {
        gamestate = GetComponent<GameState>();
        Debug.Assert(gamestate != null);
    }

    public void StartGame(int nPlayers, Grid grid)
    {
        GameOver = false;
        gamestate.ResetState();
        this.Grid = grid;
        //let's do this here for now
        for (uint i = 1; i <= nPlayers; i++)
        {
            gamestate.Players.Add(new PlayerInfo(i));
        }

        //initialize grid and add Player references to playerinfo
        gamestate.Players[0].Snake = Grid.AddPlayer(new Point(7, 2), Directions.North, Teams.Red);
        gamestate.SelectNewCurrentPlayer(gamestate.Players[0]);
        gamestate.Players[1].Snake = Grid.AddPlayer(new Point(7, 12), Directions.South, Teams.Green);
        if (nPlayers == 4)
        {
            gamestate.Players[2].Snake = Grid.AddPlayer(new Point(2, 7), Directions.East, Teams.Red);
            gamestate.Players[3].Snake = Grid.AddPlayer(new Point(12, 7), Directions.West, Teams.Green);
        }

        cardPoolManager = GetComponent<CardPool>();
        draftManager = GetComponent<DraftManager>();

        Invoke("StartRound", 5); //wait seconds before starting draft
    }

    void Update()
    {
        if (gamestate.State == Mode.FinishedRound && GameOver == false)
        {
            //if round is over and at least one snake has died
            var greenTeamDead = gamestate.Players.Where(p => p.Snake.Team == Teams.Green).All(p => p.Snake.Dead);
            var redTeamDead = gamestate.Players.Where(p => p.Snake.Team == Teams.Red).All(p => p.Snake.Dead);
            GameOver = greenTeamDead || redTeamDead;
            if (GameOver)
            {
                //go to mainmenu screen
                playerInfoCanvas.gameObject.SetActive(false);
                if (greenTeamDead) { menu.IncrementRedScore(); }
                else { menu.IncrementGreenScore(); } //red team dead
                Invoke("BackToMainMenu", waitSecondsUntilMainMenu);
            }
            else { StartRound(); }
        }
    }

    private void StartRound()
    {
        draftManager.StartDraft(cardPoolManager.FillBoosterBox(), Grid);
        gamestate.State = Mode.OpenPack;
        playerInfoCanvas.gameObject.SetActive(true);
    }

    private void BackToMainMenu()
    {
        menu.EnableScript();
    }
}
