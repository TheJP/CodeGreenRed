using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {
    public List<GameObject> cards;
    GameObject selected = null;
    Vector2 dragStartPos; //point in world coordinates where the mouse was originally clicked

    // Use this for initialization
    void Start () {
    
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


                Debug.Log("You selected the " + selected.name); // ensure you picked right object
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //deselect card
            selected = null;
        }
    }
    void HearthStoneDragRotationTrollolol()
    {
        if (selected != null) // user is holding mousebutton down and card is selected
        {
            Vector2 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 curPos = Camera.main.ScreenToWorldPoint(curScreenPoint);// + offset;
            selected.transform.position = curPos;

            var dragDir = new Vector2(curPos.x - dragStartPos.x, 0);

            var from = selected.transform.rotation;
            var to = new Quaternion(dragDir.x, dragDir.y, 1, 1);

            Quaternion targetrotation = Quaternion.LookRotation(dragDir);
            selected.transform.rotation = Quaternion.RotateTowards(selected.transform.rotation, targetrotation, 50 * Time.deltaTime);


        }
    }

    IEnumerable ScaleMe(Transform objTr)
    {
        objTr.localScale *= 1.2f;
        yield return new WaitForSeconds(0.5f);
        objTr.localScale /= 1.2f;
    }
}
