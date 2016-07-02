using UnityEngine;
using System.Collections;

public class SelectionMenu : MonoBehaviour {

    public GameObject[] selectedObject;
    public GameObject heart;
    public GameObject soundOn;
    public GameObject soundOff;
    public RuntimeAnimatorController rAC;

    public string[] maps;

    private GameObject heartLeft;
    private GameObject heartRight;
    private GameObject heartSecondLeft;
    private GameObject heartSecondRight;
    private GameObject sound;
    private int currentPosition = 0;
    private int newPosition = 0;
    private int currentMap = 0;
    private int players = 2;

    private bool muted = false;
    private bool rg = true;

    void Start() {
        spawnHeartsNew();

        foreach (GameObject obj in selectedObject){
            if (obj.name.Contains("Count")) {
                    obj.GetComponent<TextMesh>().text = ("Player: " + players);
            }
            else if(obj.name.Contains("Map")){
                obj.GetComponent<TextMesh>().text = ("Map: " + maps[currentMap]);
            }
            else if(obj.name.Contains("Sound")){
                //Turn sound on.
                Vector3 mySoundVec = obj.GetComponent<RectTransform>().localPosition;
                sound = (GameObject)Instantiate(soundOn, mySoundVec, Quaternion.LookRotation(new Vector3(0, 1)));

                sound.layer = 8;


            }
            else if(obj.name.Contains("Blind")){
                //TODO implement this.
            }
            
        }
        
    }


    void spawnHeartsNew() {

        RectTransform myRecTran = selectedObject[currentPosition].GetComponent<RectTransform>();
        Vector3 myVec = myRecTran.localPosition;
        float myWidth = myRecTran.rect.width;

        heartLeft = (GameObject)Instantiate(heart, new Vector3(myVec.x + (myWidth / 4)+5, myVec.y), Quaternion.identity);
        heartRight = (GameObject)Instantiate(heart, new Vector3(myVec.x - (myWidth / 4)-5, myVec.y), Quaternion.identity);

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


    void deactivateAdditionalHearts() {
        heartSecondLeft.SetActive(false);
        heartSecondRight.SetActive(false);
    }

    void activateAdditionalHearts() {
        heartSecondLeft.SetActive(true);
        heartSecondRight.SetActive(true);
    }

    void moveHearts() {
        RectTransform myRecTran = selectedObject[currentPosition].GetComponent<RectTransform>();
        Vector3 myVec = myRecTran.localPosition;
        float myWidth = myRecTran.rect.width;
        heartLeft.transform.position = (new Vector3(myVec.x - (myWidth / 4) - 5, myVec.y));

        heartRight.transform.position = (new Vector3(myVec.x + (myWidth / 4) + 5, myVec.y));

        heartSecondLeft.transform.position = (new Vector3(myVec.x - (myWidth / 4) - 5 -3, myVec.y));

        heartSecondRight.transform.position = (new Vector3(myVec.x + (myWidth / 4) + 5 + 3, myVec.y));
    }

    void togglePlayers()
    {
        if (players == 2)
        {
            players = 4;
            activateAdditionalHearts();
        }
        else {
            players = 2;
            deactivateAdditionalHearts();
        }
        selectedObject[currentPosition].GetComponent<TextMesh>().text = ("Player: " + players);
    }

    void toggleSound() {

        Quaternion myQuack = sound.GetComponent<Transform>().rotation;
        Vector3 myVec = sound.GetComponent<Transform>().position;
        var soundRAC = sound.GetComponent<Animator>().runtimeAnimatorController;
        Destroy(sound);
        if (muted)
        {
            muted = false;
            //Turn on sound
            sound = (GameObject)Instantiate(soundOn, myVec, myQuack);
            sound.GetComponent<Animator>().runtimeAnimatorController = soundRAC;

            sound.layer = 8;
        }
        else {
            muted = true;
            //Turn off sound
            sound = (GameObject)Instantiate(soundOff, myVec, myQuack);
            sound.GetComponent<Animator>().runtimeAnimatorController = soundRAC;
            sound.layer = 8;

        }
    }

    void toggleBlind() {

        if (rg)
        {

            selectedObject[currentPosition].GetComponent<TextMesh>().text = "YB";
        }
        else {
             
        selectedObject[currentPosition].GetComponent<TextMesh>().text = "RG";
        }

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("up")||Input.GetKeyDown("w")) {
            if (currentPosition == 0)
            {
                newPosition = selectedObject.Length - 1;
            }
            else {
                newPosition--;
            }
        }

        if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
        {
            if (currentPosition == selectedObject.Length - 1)
            {
                newPosition = 0;
            }
            else {
                newPosition++;
            }
        }

        if (Input.GetKeyDown("a") || Input.GetKeyDown("left")) {
            //Increase/dec player count change maps
            if (selectedObject[currentPosition].name.Contains("Count")) {
                togglePlayers();
            }
            else if (selectedObject[currentPosition].name.Contains("Map")) { 
                if (currentMap == 0) {
                    currentMap = maps.Length - 1;
                }
                else { currentMap--; }
                selectedObject[currentPosition].GetComponent<TextMesh>().text = ("Map: " + maps[currentMap]);
                }

            else if (selectedObject[currentPosition].name.Contains("Sound")){
                toggleSound();
            }
            else if (selectedObject[currentPosition].name.Contains("Blind")) {
                toggleBlind();                
            }
        }
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right") || Input.GetKeyDown("return")) {
            if (selectedObject[currentPosition].name.Contains("Count"))
            {
                togglePlayers();
            }
            else if (selectedObject[currentPosition].name.Contains("Map"))
            {
                if (currentMap == maps.Length-1)
                {
                    currentMap = 0;
                }
                else { currentMap++; }
                selectedObject[currentPosition].GetComponent<TextMesh>().text = ("Map: " + maps[currentMap]);
                

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

        if (Input.GetKeyDown("return") && selectedObject[currentPosition].name.Contains("Start")) {
            //Todo -> go to game scene!
            Debug.Log("start found");
        }

        if (currentPosition != newPosition) {
            currentPosition = newPosition;
            moveHearts();
        }


    }

}
