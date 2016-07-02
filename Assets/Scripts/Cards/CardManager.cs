using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.CardEffects;
using Assets.Scripts.Cards.CardEffects;
using Assets.Scripts.Cards;
using System;

public class CardManager : MonoBehaviour
{
    public List<GameObject> cards;
    public GameObject cardPrefab;
    public Transform[] CardSpawnPositions;
    public Grid grid;


    private GameObject selected = null;
    private Vector2 dragStartPos; //point in world coordinates where the mouse was originally clicked
    private Vector3 mouseLast = Vector3.zero;
    private CardEffectFactory cardEffectFactory = new CardEffectFactory();

    private Queue<CardEffect> effects = new Queue<CardEffect>();
    private PlayerInfo[] players;
    private PlayerInfo currentPlayer;



    // Use this for initialization
    void Start()
    {

        addFactories();

        foreach (var t in CardSpawnPositions)
        {
            var card = Instantiate<GameObject>(cardPrefab);
            card.transform.position = t.position;
            cards.Add(card);
        }
    }

    // Update is called once per frame
    void Update()
    {


        CheckMouseSelection();
        HearthStoneDragRotationTrollolol();
        TakeActionForSelected();

        TestExecuteOnKeyPress();
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

                if (selected != null)
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
    void TakeActionForSelected()
    {
        if (selected != null)
        {
            //get cardEffect from Selected
            var effect = selected.GetComponent<Card>().type.GetEffectType();
            Debug.Log("type " + effect);

            var parameters = new CardEffectParamerters();
            
            effects.Enqueue(cardEffectFactory.create(effect,parameters));
        }
    }

    void TestExecuteOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            effects.Dequeue().Execute();
        }
    }

    struct PlayerInfo
    {
        uint playerNumber;
    }

    private void addFactories()
    {
        //UPDATE these entries and cardEffectDictionary below
        //TODO could be done with relection
        cardEffectFactory.addFactoryMethod<MoveEffect>(MoveEffect.GetFactory());
        cardEffectFactory.addFactoryMethod<DebugEffect>(DebugEffect.GetFactory());
    }
}

public enum CardType { MOVE, RIGHT, LEFT, DEBUG }
/// <summary>
/// Enables us to select cardtype in unity Editor
/// </summary>
public static class CardTypeExtension
{
    static Dictionary<CardType, Type> cardEffectDictionary = new Dictionary<CardType, Type>()
        {
            {CardType.MOVE,typeof(MoveEffect) },
            {CardType.DEBUG,typeof(DebugEffect) }
        };
    public static Type GetEffectType(this CardType cardType)
    {
        return cardEffectDictionary[cardType];
    }

}
