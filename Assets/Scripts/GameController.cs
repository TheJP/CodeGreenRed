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

    private Grid grid;
    private DraftManager cardManager;
    private List<PlayerInfo> players = new List<PlayerInfo>();

    void Start()
    {
        gamestate = GetComponent<GameState>();
        Debug.Assert(gamestate != null);

        //let's do this here for now
        int nPlayers = 4;
        for(uint i = 1; i <= nPlayers; i++)
        {
            players.Add(new PlayerInfo(i));
        }

        //initialize grid and add Player references to playerinfo
        grid = Instantiate(gridPrefab).GetComponent<Grid>();
        grid.SetWallLayout(WallLayouts.Border.CreateArray(grid.width, grid.height));
        players[0].Snake = grid.AddPlayer(new Point(5, 5), Directions.North, Teams.Red);
        players[1].Snake = grid.AddPlayer(new Point(10, 5), Directions.North, Teams.Red);
        players[2].Snake = grid.AddPlayer(new Point(5, 10), Directions.South, Teams.Green);
        players[3].Snake = grid.AddPlayer(new Point(10, 10), Directions.South, Teams.Green);


        var effectParams = new CardEffectParamerters();
        effectParams.CastingPlayer = players[0].Snake;
        
        var boosters = GetComponent<CardPool>().BasicBoosterBox();
        var draftManager = GetComponent<DraftManager>();
        draftManager.StartDraft(boosters, effectParams);

    }

    void Update()
    {
    }



}
