using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int BodyElementsPerTile = 3;
    public const float MovementSpeed = 1f;
    public readonly Vector3 PlayerOffset = new Vector3(0.5f, 0.5f);

    public GameObject playerHeadPrefab;
    public GameObject playerBodyPrefab;
    public Sprite[] playerSprites;

    private GameObject head;
    private readonly Stack<GameObject> body = new Stack<GameObject>();
    private int grow = 0;
    private readonly Queue<Point> headAnimation = new Queue<Point>();

    public Directions Direction { get; private set; }
    public Point Position { get; private set; }
    public Queue<Point> BodyPositions { get; private set; }
    public int SnakeLength { get { return BodyPositions.Count; } }

    public Player() { BodyPositions = new Queue<Point>(); }

    /// <summary>Sets the starting position for this snake. Has to be called, bevore the GameObject is started.</summary>
    /// <param name="position"></param>
    public void SetInitialPosition(Point position, Directions direction = Directions.North)
    {
        this.Position = position;
        BodyPositions.Enqueue(position);
        this.Direction = direction;
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
    public void Shrink(int amount = 1)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>Makes the snake move by the given amount.</summary>
    /// <param name="amount">Amount that the snake will move meassured in tiles.</param>
    public void Move(int amount = 1)
    {
        for(int i = 0; i < amount; ++i)
        {
            Position += Direction.Movement();
            BodyPositions.Enqueue(Position);
            headAnimation.Enqueue(Position);
            if(grow > 0) { --grow; } //TODO: Spawn body??
            else { BodyPositions.Dequeue(); }
        }
    }

    public void TurnLeft() { Direction = Direction.TurnLeft(); }
    public void TurnRight() { Direction = Direction.TurnRight(); }

    void Start()
    {
        head = Instantiate(playerHeadPrefab);
        head.transform.parent = transform;
        head.transform.localPosition = Position.ToVector() + PlayerOffset;
        //TODO: Generalize
        head.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
    }

    void Update()
    {
        //Head animation
        if (headAnimation.Any())
        {
            var direction = (headAnimation.Peek().ToVector() + PlayerOffset) - head.transform.localPosition;
            if (direction.magnitude < Time.deltaTime * MovementSpeed)
            {
                head.transform.localPosition = headAnimation.Dequeue().ToVector() + PlayerOffset;
            }
            else
            {
                head.transform.Translate(direction.normalized * Time.deltaTime * MovementSpeed, Space.Self);
            }
        }
    }
}
