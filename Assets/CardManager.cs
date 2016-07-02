using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {
    public List<GameObject> cards;
    public GameObject cardPrefab;
    public Transform[] CardSpawnPositions;
   

    GameObject selected = null;
    Vector2 dragStartPos; //point in world coordinates where the mouse was originally clicked
    Vector3 mouseLast = Vector3.zero;

    // Use this for initialization
    void Start () {

        foreach(var t in CardSpawnPositions)
        {
            var card = Instantiate<GameObject>(cardPrefab);
            card.transform.position = t.position;
            cards.Add(card);
        } 
    }
    
    // Update is called once per frame
    void Update () {


        CheckMouseSelection();
        HearthStoneDragRotationTrollolol();
    }

    void CheckMouseSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouse = Input.mousePosition;
            dragStartPos = Camera.main.ScreenToWorldPoint(mouse);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                selected = cards.Find(c => c.transform == hit.transform);

                if(selected != null)
                {
                    Debug.Log("You selected the " + selected.name); // ensure you picked right object
                    selected.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //deselect card
            if (selected != null)
            {
                selected.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                selected = null;
            }
        }
    }
    void HearthStoneDragRotationTrollolol()
    {
        if (selected != null && Input.mousePosition != mouseLast) // user is holding mousebutton down and card is selected
        {
            Vector2 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 curPos = Camera.main.ScreenToWorldPoint(curScreenPoint);// + offset;
            selected.transform.position = curPos;

            var dragDir = new Vector2(curPos.x - dragStartPos.x, 0);

            var from = selected.transform.rotation;
            Quaternion targetrotation = Quaternion.LookRotation(dragDir);
            selected.transform.rotation = Quaternion.RotateTowards(selected.transform.rotation, targetrotation, 50 * Time.deltaTime);


        }
        mouseLast = Input.mousePosition;
    }

    IEnumerable ScaleMe(Transform objTr)
    {
        objTr.localScale *= 1.2f;
        yield return new WaitForSeconds(0.5f);
        objTr.localScale /= 1.2f;
    }
}
