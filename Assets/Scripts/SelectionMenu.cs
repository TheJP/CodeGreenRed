using UnityEngine;
using System.Collections;
using System;

public class SelectionMenu : MonoBehaviour
{

    public GameObject[] selectedObject;
    public GameObject heart;
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject mainCamera;
    public GameObject guiCamera;
    public RuntimeAnimatorController rAC;
    public GameObject GreenScoreObject;
    public GameObject RedScoreObject;

    public GameObject gridHolder;
    public GameObject gridPrefab;
    public GameState gamestate;
    public GameController gameController;

    private GameObject heartLeft;
    private GameObject heartRight;
    private GameObject heartSecondLeft;
    private GameObject heartSecondRight;
    private GameObject sound;



    private AudioListener myAudioListener;
    private int currentPosition = 0;
    private int newPosition = 0;
    private WallLayouts currentMap = WallLayouts.NoWalls;
    private int players = 2;
    private Animator mainCameraAnimator;
    private Animator guiCameraAnimator;

    private bool muted = false;
    private bool rg = true;

    private int redScore = 0;
    private int greenScore = 0;
    private bool forward = true;


    //Should be handled in another script

    private Grid grid;

    public void incRedScore()
    {
        redScore++;
        RedScoreObject.GetComponent<TextMesh>().text = ("R:" + redScore);
    }

    public void incGreenScore()
    {
        greenScore++;
        GreenScoreObject.GetComponent<TextMesh>().text = ("G:"+greenScore);
    }

    public void enableScript() {
        guiCameraAnimator.Play("GuiCameraReward");
        mainCameraAnimator.Play("myCameraReward");

        StartCoroutine(waitUntilEnable());

        Destroy(grid.gameObject);
        InizializeMap();
    }

    IEnumerator waitUntilEnable() {
        yield return new WaitForSeconds(2);
        gamestate.State = Mode.Menu;
    }

    void Start()
    {
        InizializeMap();
        spawnHeartsNew();
        mainCameraAnimator = mainCamera.GetComponent<Animator>();

        guiCameraAnimator = guiCamera.GetComponent<Animator>();




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
                sound = (GameObject)Instantiate(soundOn, mySoundVec, Quaternion.LookRotation(new Vector3(0, 1)));

                sound.layer = 8;
                myAudioListener = (AudioListener)mainCamera.GetComponent<AudioListener>();

            }

        }

    }


    void spawnHeartsNew()
    {

        RectTransform myRecTran = selectedObject[currentPosition].GetComponent<RectTransform>();
        Vector3 myVec = myRecTran.localPosition;
        float myWidth = myRecTran.rect.width;

        heartLeft = (GameObject)Instantiate(heart, new Vector3(myVec.x + (myWidth / 4) + 5, myVec.y), Quaternion.identity);
        heartRight = (GameObject)Instantiate(heart, new Vector3(myVec.x - (myWidth / 4) - 5, myVec.y), Quaternion.identity);

        heartLeft.layer = 8;
        heartRight.layer = 8;

        heartLeft.GetComponent<Animator>().runtimeAnimatorController = rAC;
        heartRight.GetComponent<Animator>().runtimeAnimatorController = rAC;

        heartSecondLeft = (GameObject)Instantiate(heart, new Vector3(myVec.x + (myWidth / 4) + 5 + 3, myVec.y), Quaternion.identity);
        heartSecondRight = (GameObject)Instantiate(heart, new Vector3(myVec.x - (myWidth / 4) - 5 - 3, myVec.y), Quaternion.identity);

        heartSecondLeft.layer = 8;
        heartSecondRight.layer = 8;

        heartSecondLeft.GetComponent<Animator>().runtimeAnimatorController = rAC;
        heartSecondRight.GetComponent<Animator>().runtimeAnimatorController = rAC;

        deactivateAdditionalHearts();
    }


    void deactivateAdditionalHearts()
    {
        heartSecondLeft.SetActive(false);
        heartSecondRight.SetActive(false);
    }

    void activateAdditionalHearts()
    {
        heartSecondLeft.SetActive(true);
        heartSecondRight.SetActive(true);
    }

    void moveHearts()
    {
        RectTransform myRecTran = selectedObject[currentPosition].GetComponent<RectTransform>();
        Vector3 myVec = myRecTran.localPosition;
        float myWidth = myRecTran.rect.width;
        heartLeft.transform.position = (new Vector3(myVec.x - (myWidth / 4) - 5, myVec.y));

        heartRight.transform.position = (new Vector3(myVec.x + (myWidth / 4) + 5, myVec.y));

        heartSecondLeft.transform.position = (new Vector3(myVec.x - (myWidth / 4) - 5 - 3, myVec.y));

        heartSecondRight.transform.position = (new Vector3(myVec.x + (myWidth / 4) + 5 + 3, myVec.y));
    }

    void togglePlayers()
    {
        if (players == 2)
        {
            players = 4;
            activateAdditionalHearts();
        }
        else
        {
            players = 2;
            deactivateAdditionalHearts();
        }
        selectedObject[currentPosition].GetComponent<TextMesh>().text = ("Player: " + players);
    }

    void toggleSound()
    {

        Quaternion myQuack = sound.GetComponent<Transform>().rotation;
        Vector3 myVec = sound.GetComponent<Transform>().position;
        var soundRAC = sound.GetComponent<Animator>().runtimeAnimatorController;
        Destroy(sound);
        if (muted)
        {
            muted = false;
            //Turn on sound

            myAudioListener.enabled = true;

            sound = (GameObject)Instantiate(soundOn, myVec, myQuack);
            sound.GetComponent<Animator>().runtimeAnimatorController = soundRAC;

            sound.layer = 8;
        }
        else
        {
            muted = true;
            //Turn off sound

            myAudioListener.enabled = false;

            sound = (GameObject)Instantiate(soundOff, myVec, myQuack);
            sound.GetComponent<Animator>().runtimeAnimatorController = soundRAC;
            sound.layer = 8;

        }
    }

    void toggleBlind()
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

            if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
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

            if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
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

            if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
            {

                //Increase/dec player count change maps
                if (selectedObject[currentPosition].name.Contains("Count"))
                {
                    togglePlayers();
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
                    toggleSound();
                }
                else if (selectedObject[currentPosition].name.Contains("Blind"))
                {
                    toggleBlind();
                }
            }
            if (Input.GetKeyDown("d") || Input.GetKeyDown("right") || Input.GetKeyDown("return"))
            {
                if (selectedObject[currentPosition].name.Contains("Count"))
                {
                    togglePlayers();
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
                    toggleSound();
                }
                else if (selectedObject[currentPosition].name.Contains("Blind"))
                {
                    toggleBlind();
                }

            }


            if (Input.GetKeyDown("return") && selectedObject[currentPosition].name.Contains("Start"))
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
                moveHearts();
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
