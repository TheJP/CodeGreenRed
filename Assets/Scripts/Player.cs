using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int BodyElementsPerTile = 3;

    public GameObject playerHeadPrefab;
    public GameObject playerBodyPrefab;

    public Sprite[] playerSprites;
    private GameObject head;
    private Stack<GameObject> body = new Stack<GameObject>();
    private int grow = 0;

    public Point Position { get; private set; }
    public Queue<Point> BodyPositions { get; private set; }
    public int SnakeLength { get { return BodyPositions.Count; } }

    public Player() { BodyPositions = new Queue<Point>(); }

    /// <summary>Sets the starting position for this snake. Has to be called, bevore the GameObject is started.</summary>
    /// <param name="position"></param>
    public void SetInitialPosition(Point position)
    {
        this.Position = position;
        BodyPositions.Enqueue(position);
    }

    /// <summary>Grows the snake by the given amount.</summary>
    /// <param name="amount">Amount that the snake will grow meassured in tiles.</param>
    public void Grow(int amount = 1)
    {
        grow += amount;
        if(grow < 0) { grow = 0; } //Cannot be less than 0
    }

    /// <summary>Makes the snake shorter by the given amount.</summary>
    /// <param name="amount">Amount that the snake will shrink meassured in tiles.</param>
    public void Shrink(int amount)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>Makes the snake move by the given amount.</summary>
    /// <param name="amount">Amount that the snake will move meassured in tiles.</param>
    public void Move(int amount)
    {

    }

    void Start()
    {
        head = Instantiate(playerHeadPrefab);
        head.transform.position = Position.ToVector();
        head.transform.parent = transform;
    }

    void Update()
    {

    }
}
