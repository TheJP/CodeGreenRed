using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int BodyElementsPerTile = 3;
    public const float MovementSpeed = 2f;
    private const float Epsilon = 0.001f;
    public readonly Vector3 PlayerOffset = new Vector3(0.5f, 0.5f);

    public GameObject playerHeadPrefab;
    public GameObject playerBodyPrefab;
    public Sprite[] playerSprites;

    private GameObject head;
    private readonly List<GameObject> body = new List<GameObject>();
    private int grow = 0;
    private readonly Queue<Point> headAnimation = new Queue<Point>();
    private readonly List<Vector3> bodyAnimation = new List<Vector3>();
    private int animationHasToGrow = 0;
    private Vector3 previousHeadAnimation;

    public Directions Direction { get; private set; }
    public Point Position { get; private set; }
    public Queue<Point> BodyPositions { get; private set; }
    public int SnakeLength { get { return BodyPositions.Count + grow; } }

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
        if (grow < 0) { grow = 0; } //Cannot be less than 0
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
        var startMovement = !headAnimation.Any() && amount > 0;
        if (startMovement) { previousHeadAnimation = Position.ToVector() + PlayerOffset; }
        for (int i = 0; i < amount; ++i)
        {
            Position += Direction.Movement();
            BodyPositions.Enqueue(Position);
            headAnimation.Enqueue(Position);
            if (grow > 0) { --grow; ++animationHasToGrow; }
            else { BodyPositions.Dequeue(); }
        }
        if (startMovement) { StartHeadMovement(); }
    }

    private void StartHeadMovement()
    {
        if (animationHasToGrow > 0)
        {
            --animationHasToGrow;
            SpawnBody();
        }
        if (bodyAnimation.Any())
        {
            for (int i = bodyAnimation.Count - 1; i >= BodyElementsPerTile; --i)
            {
                bodyAnimation[i] = bodyAnimation[i - BodyElementsPerTile];
            }
            var distance = ((headAnimation.Peek().ToVector() + PlayerOffset) - head.transform.localPosition) / BodyElementsPerTile;
            for (int i = 0; i < BodyElementsPerTile; ++i)
            {
                bodyAnimation[i] = (headAnimation.Peek().ToVector() + PlayerOffset) - (i + 1) * distance;
            }
        }
    }

    private void SpawnBody()
    {
        for (int i = 0; i < BodyElementsPerTile; ++i)
        {
            var bodyPart = Instantiate(playerBodyPrefab);
            bodyPart.transform.parent = transform;
            bodyPart.transform.localPosition = bodyAnimation.Count > 0 ? bodyAnimation.Last() : previousHeadAnimation;
            //TODO: Generalize
            bodyPart.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
            body.Add(bodyPart);
            bodyAnimation.Add(bodyPart.transform.localPosition);
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
                previousHeadAnimation = head.transform.localPosition;
                if (headAnimation.Any()) { StartHeadMovement(); }
            }
            else
            {
                head.transform.Translate(direction.normalized * Time.deltaTime * MovementSpeed, Space.Self);
            }
        }

        //Body animation
        for(int i = 0; i < body.Count; ++i)
        {
            var direction = bodyAnimation[i] - body[i].transform.localPosition;
            if(direction.magnitude < Time.deltaTime * MovementSpeed)
            {
                body[i].transform.localPosition = bodyAnimation[i];
            }
            else
            {
                body[i].transform.Translate(direction.normalized * Time.deltaTime * MovementSpeed, Space.Self);
            }
        }
    }
}
