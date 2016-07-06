using UnityEngine;
using System.Collections;
using System;

public class SelectionMenu : MonoBehaviour
{

    public GameObject[] selectedObject;
    public GameObject heartPrefab;
    public GameObject soundOnPrefab;
    public GameObject soundOffPrefab;
    public Animator mainCamera;
    public Animator guiCamera;
    public RuntimeAnimatorController rAC;
    public TextMesh greenScoreText;
    public TextMesh redScoreText;

    public GameObject gridHolder;
    public GameObject gridPrefab;
    public GameState gamestate;
    public GameController gameController;

    private GameObject heartLeft;
    private GameObject heartRight;
    private GameObject heartSecondLeft;
    private GameObject heartSecondRight;
    private GameObject sound = null;

    private int currentPosition = 0;
    private int newPosition = 0;
    private WallLayouts currentMap = WallLayouts.NoWalls;
    private int players = 2;
    private Animator mainCameraAnimator;
    private Animator guiCameraAnimator;

    private bool rg = true;

    private int redScore = 0;
    private int greenScore = 0;
    private bool forward = true;


    //Should be handled in another script

    private Grid grid;

    public void IncrementRedScore()
    {
        redScore++;
        redScoreText.text = ("R:" + redScore);
    }

    public void IncrementGreenScore()
    {
        greenScore++;
        greenScoreText.text = ("G:"+greenScore);
    }

    public void EnableScript() {
        guiCameraAnimator.Play("GuiCameraBackward");
        mainCameraAnimator.Play("myCameraReward");

        Invoke("Enable", 2f);
    }

    private void Enable() {
        gamestate.State = Mode.Menu;
        Destroy(grid.gameObject);
        InizializeMap();
    }

    private void Start()
    {
        InizializeMap();
        spawnHeartsNew();
        mainCameraAnimator = mainCamera;
        guiCameraAnimator = guiCamera;

        foreach (GameObject obj in selectedObject)
        {
            if (obj.name.Contains("Count"))
            {
                obj.GetComponent<TextMesh>().text = ("Player: " + players);
            }
            else if (obj.name.Contains("Map"))
            {
                obj.GetComponent<TextMesh>().text = ("Map: " + currentMap.ToString());

            }
            else if (obj.name.Contains("Sound"))
            {
                //Turn sound on.
                Vector3 mySoundVec = obj.GetComponent<RectTransform>().localPosition;
                sound = (GameObject)Instantiate(AudioListener.pause ? soundOffPrefab : soundOnPrefab, mySoundVec, Quaternion.identity);
                sound.layer = 8;
            }
        }

    }

    private void spawnHeartsNew()
    {

        RectTransform myRecTran = selectedObject[currentPosition].GetComponent<RectTransform>();
        Vector3 myVec = myRecTran.localPosition;
        float myWidth = myRecTran.rect.width;

        heartLeft = (GameObject)Instantiate(heartPrefab, new Vector3(myVec.x + (myWidth / 4) + 5, myVec.y), Quaternion.identity);
        heartRight = (GameObject)Instantiate(heartPrefab, new Vector3(myVec.x - (myWidth / 4) - 5, myVec.y), Quaternion.identity);

        heartLeft.layer = 8;
        heartRight.layer = 8;

        heartLeft.GetComponent<Animator>().runtimeAnimatorController = rAC;
        heartRight.GetComponent<Animator>().runtimeAnimatorController = rAC;

        heartSecondLeft = (GameObject)Instantiate(heartPrefab, new Vector3(myVec.x + (myWidth / 4) + 5 + 3, myVec.y), Quaternion.identity);
        heartSecondRight = (GameObject)Instantiate(heartPrefab, new Vector3(myVec.x - (myWidth / 4) - 5 - 3, myVec.y), Quaternion.identity);

        heartSecondLeft.layer = 8;
        heartSecondRight.layer = 8;

        heartSecondLeft.GetComponent<Animator>().runtimeAnimatorController = rAC;
        heartSecondRight.GetComponent<Animator>().runtimeAnimatorController = rAC;

        DeactivateAdditionalHearts();
    }


    private void DeactivateAdditionalHearts()
    {
        heartSecondLeft.SetActive(false);
        heartSecondRight.SetActive(false);
    }

    private void ActivateAdditionalHearts()
    {
        heartSecondLeft.SetActive(true);
        heartSecondRight.SetActive(true);
    }

    private void MoveHearts()
    {
        RectTransform myRecTran = selectedObject[currentPosition].GetComponent<RectTransform>();
        Vector3 myVec = myRecTran.localPosition;
        float myWidth = myRecTran.rect.width;
        heartLeft.transform.position = (new Vector3(myVec.x - (myWidth / 4) - 5, myVec.y));

        heartRight.transform.position = (new Vector3(myVec.x + (myWidth / 4) + 5, myVec.y));

        heartSecondLeft.transform.position = (new Vector3(myVec.x - (myWidth / 4) - 5 - 3, myVec.y));

        heartSecondRight.transform.position = (new Vector3(myVec.x + (myWidth / 4) + 5 + 3, myVec.y));
    }

    private void TogglePlayers()
    {
        if (players == 2)
        {
            players = 4;
            ActivateAdditionalHearts();
        }
        else
        {
            players = 2;
            DeactivateAdditionalHearts();
        }
        selectedObject[currentPosition].GetComponent<TextMesh>().text = ("Player: " + players);
    }

    private void ToggleSound()
    {

        Quaternion myQuack = sound.transform.rotation;
        Vector3 myVec = sound.transform.position;
        var soundRAC = sound.GetComponent<Animator>().runtimeAnimatorController;
        Destroy(sound);
        AudioListener.pause = !AudioListener.pause;
        sound = (GameObject)Instantiate(AudioListener.pause ? soundOffPrefab : soundOnPrefab, myVec, myQuack);
        sound.GetComponent<Animator>().runtimeAnimatorController = soundRAC;
        sound.layer = 8;
    }

    private void ToggleBlind()
    {

        if (rg)
        {
            selectedObject[currentPosition].GetComponent<TextMesh>().text = "YB";
        }
        else
        {
            selectedObject[currentPosition].GetComponent<TextMesh>().text = "RG";
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (gamestate.State == Mode.Menu)
        {

            if (Input.GetButtonDown("Up"))
            {

                if (currentPosition == 0)
                {
                    newPosition = selectedObject.Length - 1;
                }
                else
                {
                    newPosition--;
                }
            }

            if (Input.GetButtonDown("Down"))
            {
                if (currentPosition == selectedObject.Length - 1)
                {
                    newPosition = 0;
                }
                else
                {
                    newPosition++;
                }
            }

            if (Input.GetButtonDown("Left"))
            {

                //Increase/dec player count change maps
                if (selectedObject[currentPosition].name.Contains("Count"))
                {
                    TogglePlayers();
                }
                else if (selectedObject[currentPosition].name.Contains("Map"))
                {
                    currentMap = currentMap == WallLayouts.NoWalls ? WallLayouts.Corners : currentMap - 1;
                    selectedObject[currentPosition].GetComponent<TextMesh>().text = ("Map: " + currentMap.ToString());

                    //TODO: create Map with options (player and what specific map?)
                    ChangeMapLayout();
                }

                else if (selectedObject[currentPosition].name.Contains("Sound"))
                {
                    ToggleSound();
                }
                else if (selectedObject[currentPosition].name.Contains("Blind"))
                {
                    ToggleBlind();
                }
            }
            if (Input.GetButtonDown("Right") || Input.GetButtonDown("Submit"))
            {
                if (selectedObject[currentPosition].name.Contains("Count"))
                {
                    TogglePlayers();
                }
                else if (selectedObject[currentPosition].name.Contains("Map"))
                {
                    currentMap = currentMap == WallLayouts.Corners ? WallLayouts.NoWalls : currentMap + 1;
                    selectedObject[currentPosition].GetComponent<TextMesh>().text = ("Map: " + currentMap.ToString());

                    //TODO: create Map with options (player and what specific map?)
                    ChangeMapLayout();



                }
                else if (selectedObject[currentPosition].name.Contains("Sound"))
                {
                    ToggleSound();
                }
                else if (selectedObject[currentPosition].name.Contains("Blind"))
                {
                    ToggleBlind();
                }

            }


            if (Input.GetButtonDown("Submit") && selectedObject[currentPosition].name.Contains("Start"))
            {
                //Starting game!:
                gameController.StartGame(players, grid);
                gamestate.State = Mode.AboutToStart;



                mainCameraAnimator.Play("myCameraForward");
                guiCameraAnimator.Play("GuiCameraForward");

                //mainCameraAnimator.Play("myCameraReward");

            }



            if (currentPosition != newPosition)
            {
                currentPosition = newPosition;
                MoveHearts();
            }
        }

    }


    public void ChangeMapLayout()
    {
        grid.SetWallLayout(currentMap.CreateArray(grid.width, grid.height));
    }

    private void InizializeMap()
    {
        grid = Instantiate(gridPrefab).GetComponent<Grid>();
        ChangeMapLayout();
    }


}
