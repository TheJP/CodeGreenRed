using Assets.Scripts;
using Assets.Scripts.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private GameState gamestate;

    public GameObject gridPrefab;

    public Grid Grid { get; private set; }
    private DraftManager draftManager;
    private CardPool cardPoolManager;
    private CardEffectParamerters effectParams;
    public bool GameOver { get; set; }

    void Start()
    {
        gamestate = GetComponent<GameState>();
        Debug.Assert(gamestate != null);

        //let's do this here for now
        int nPlayers = 4;
        for(uint i = 1; i <= nPlayers; i++)
        {
            gamestate.Players.Add(new PlayerInfo(i));
        }

        //initialize grid and add Player references to playerinfo
        Grid = Instantiate(gridPrefab).GetComponent<Grid>();
        Grid.SetWallLayout(WallLayouts.Border.CreateArray(Grid.width, Grid.height));
        gamestate.Players[0].Snake = Grid.AddPlayer(new Point(5, 5), Directions.North, Teams.Red);
        gamestate.Players[1].Snake = Grid.AddPlayer(new Point(10, 5), Directions.North, Teams.Red);
        gamestate.Players[2].Snake = Grid.AddPlayer(new Point(5, 10), Directions.South, Teams.Green);
        gamestate.Players[3].Snake = Grid.AddPlayer(new Point(10, 10), Directions.South, Teams.Green);

        cardPoolManager = GetComponent<CardPool>();
        draftManager = GetComponent<DraftManager>();

        StartRound();
    }

    void Update()
    {
        if(gamestate.State == Mode.FinishedRound)
        {
            if (GameOver)
            {
                //go to mainmenu screen
            } else { StartRound(); }
        }
    }

    private void StartRound()
    {
        draftManager.StartDraft(cardPoolManager.FillBoosterBox(), Grid);
        gamestate.State = Mode.OpenPack;
    }



}
