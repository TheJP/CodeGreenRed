using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>Determines, how many body elements are created per tile.</summary>
    public const int BodyElementsPerTile = 3;
    /// <summary>Animation movement speed.</summary>
    public const float MovementSpeed = 2f;
    public const int StartingLength = 3;
    /// <summary>Coordinate offset for head and body sprites.</summary>
    public readonly Vector3 PlayerOffset = new Vector3(0.5f, 0.5f);
    public readonly Vector3 ArrowScale = new Vector3(0.5f, 0.5f, 0.5f);
    public readonly Vector3 HeadScale = new Vector3(0.7f, 0.7f, 0.7f);
    public readonly Vector3 BodyScale = new Vector3(0.5f, 0.5f, 0.5f);

    public GameObject arrowPrefab;
    public GameObject playerHeadPrefab;
    public GameObject playerBodyPrefab;
    public GameObject explodeParticlesPrefab;
    public Material greenMaterial;
    public Material redMaterial;
    public Sprite[] playerSprites;

    public event Action<MoveEventArguments> BeforeMove;
    public event Action<Player> AfterDeath;

    //Cached GameObjects for arrow, head and body
    private GameObject arrow = null;
    private GameObject head;
    private readonly List<GameObject> body = new List<GameObject>();
    /// <summary>Determines, how many body elements have to be grown on the next move.</summary>
    private int grow = StartingLength;
    //Animation states of the snake
    private readonly Queue<Point> headAnimation = new Queue<Point>();
    private readonly List<Vector3> bodyAnimation = new List<Vector3>();
    private int animationHasToGrow = 0;
    private Vector3 previousHeadAnimation;
    private Teams team;

    /// <summary>The direction, in which the snake is currently heading.</summary>
    public Directions Direction { get; private set; }
    /// <summary>The position of the snake head, which is relevant for the game logic. (The animation may not be that far yet.)</summary>
    public Point Position { get; private set; }
    /// <summary>The position of all body elements of the snake, which are relevant for the game logic.</summary>
    public Queue<Point> BodyPositions { get; private set; }
    /// <summary>Count of all body parts of the snakes, which already exist or will be grown on the next moves.</summary>
    public int SnakeLength { get { return BodyPositions.Count + grow; } }
    public Teams Team
    {
        get { return team; }
        set
        {
            team = value;
            if(arrow != null) { arrow.GetComponent<Arrow>().Team = value; }
        }
    }
    /// <summary>Grid, in which the snake moves. Has to be set, before the snake starts.</summary>
    public Grid Grid { get; set; }
    /// <summary>Determines, if the player is dead.</summary>
    public bool Dead { get; private set; }

    public Player() { BodyPositions = new Queue<Point>(); }

    /// <summary>Sets the starting position for this snake. Has to be called, before the GameObject is started.</summary>
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
        if (Dead) { return; }
        if (grow > 0) { --grow; }
        else if (BodyPositions.Count <= 1) { Die(); }
        else
        {
            BodyPositions.Dequeue();
            if (animationHasToGrow > 0) { --animationHasToGrow; }
            else
            {
                for (int i = 0; i < BodyElementsPerTile; ++i)
                {
                    var bodyPart = body[body.Count - 1];
                    //Create explosion
                    var explosion = (GameObject)Instantiate(explodeParticlesPrefab, bodyPart.transform.position, Quaternion.identity);
                    explosion.transform.parent = bodyPart.transform;
                    explosion.GetComponent<ParticleSystem>().startColor = Team == Teams.Green ? greenMaterial.color : redMaterial.color;
                    //Destroy body
                    bodyPart.GetComponent<SpriteRenderer>().sprite = null;
                    Destroy(bodyPart, 1f);
                    body.RemoveAt(body.Count - 1);
                    bodyAnimation.RemoveAt(bodyAnimation.Count - 1);
                }
            }
        }
    }

    /// <summary>Makes the snake move by the given amount.</summary>
    /// <param name="amount">Amount that the snake will move meassured in tiles.</param>
    public void Move(int amount = 1)
    {
        if (Dead) { return; }
        var startMovement = !headAnimation.Any() && amount > 0;
        var previousHeadAnimation = Position.ToVector() + PlayerOffset;
        var canceled = 0;
        for (int i = 0; i < amount; ++i)
        {
            var targetPosition = Position + Direction.Movement();
            //Wrap around the edges of the grid
            if (targetPosition.X < 0) { targetPosition = new Point(targetPosition.X + Grid.width, targetPosition.Y); }
            if (targetPosition.X >= Grid.width) { targetPosition = new Point(targetPosition.X - Grid.width, targetPosition.Y); }
            if (targetPosition.Y < 0) { targetPosition = new Point(targetPosition.X, targetPosition.Y + Grid.height); }
            if (targetPosition.Y >= Grid.height) { targetPosition = new Point(targetPosition.X, targetPosition.Y - Grid.height); }
            //Trigger movement event
            if (BeforeMove != null)
            {
                var arguments = new MoveEventArguments(this, targetPosition);
                BeforeMove(arguments);
                if (Dead) { return; }
                if (arguments.Canceled) { ++canceled; continue; }
            }
            //Move player
            Position = targetPosition;
            BodyPositions.Enqueue(Position);
            headAnimation.Enqueue(Position);
            if (grow > 0) { --grow; ++animationHasToGrow; }
            else { BodyPositions.Dequeue(); }
        }
        if (startMovement && canceled < amount)
        {
            this.previousHeadAnimation = previousHeadAnimation;
            StartHeadMovement();
        }
    }

    /// <summary>Called whenever the snake head starts to move away from the center of a tile. Only relevant for the snake animation / visualisation.</summary>
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
            var distance = ((headAnimation.Peek().ToVector() + PlayerOffset) - head.transform.localPosition);
            var distribution = distance.normalized / BodyElementsPerTile;
            if(distance.magnitude > 2) { distribution = -distribution; } //Change direction for warpping around edges
            for (int i = 0; i < BodyElementsPerTile; ++i)
            {
                bodyAnimation[i] = (headAnimation.Peek().ToVector() + PlayerOffset) - (i + 1) * distribution;
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
            bodyPart.transform.localScale = BodyScale; //Local scaling is not set correctly by unity when spawning (fixed with this)
            bodyPart.GetComponent<SpriteRenderer>().material = Team == Teams.Green ? greenMaterial : redMaterial;
            //TODO: Generalize
            bodyPart.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
            body.Add(bodyPart);
            bodyAnimation.Add(bodyPart.transform.localPosition);
        }
    }

    public void TurnLeft()
    {
        if (Dead) { return; }
        Direction = Direction.TurnLeft();
        arrow.GetComponent<Arrow>().TurnTo(Direction.Angle());
    }

    public void TurnRight()
    {
        if (Dead) { return; }
        Direction = Direction.TurnRight();
        arrow.GetComponent<Arrow>().TurnTo(Direction.Angle());
    }

    /// <summary>Kills this snake.</summary>
    public void Die()
    {
        Dead = true;
        Destroy(head, 0.2f);
        arrow = null;
        head = null;
        foreach(var bodyPart in body.ToList()) { Destroy(bodyPart, 0.2f); }
        body.Clear();
        headAnimation.Clear();
        bodyAnimation.Clear();
        grow = 0;
        animationHasToGrow = 0;
        BodyPositions.Clear();
        if(AfterDeath != null) { AfterDeath(this); }
    }

    /// <summary>Controls, if this snake is selected or not.</summary>
    /// <param name="value"></param>
    public void Select(bool value)
    {
        foreach (var particle in body.Concat(new[] { head }).Select(b => b.GetComponent<ParticleSystem>()))
        {
            if (value) { particle.Play(); }
            else { particle.Stop(); }
        }
    }

    void Start()
    {
        //Spawn head
        head = Instantiate(playerHeadPrefab);
        head.transform.parent = transform;
        head.transform.localPosition = Position.ToVector() + PlayerOffset;
        head.transform.localScale = HeadScale; //Local scaling is not set correctly by unity when spawning (fixed with this)
        head.GetComponent<SpriteRenderer>().material = Team == Teams.Green ? greenMaterial : redMaterial;
        //Spawn arrow
        arrow = Instantiate(arrowPrefab);
        arrow.transform.parent = head.transform;
        arrow.transform.localPosition = new Vector3(0f, 0f, -1f);
        arrow.GetComponent<Arrow>().Turn(Direction.Angle(), Direction.Angle());
        arrow.GetComponent<Arrow>().Team = Team;
        //TODO: Generalize
        head.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
    }

    /// <summary>Apply a position update on the given transform in the given direction. This does in clude border wrapping.</summary>
    /// <param name="transform"></param>
    /// <param name="direction"></param>
    private void UpdatePosition(Transform transform, Vector3 direction, Vector3 target)
    {
        //Wrap around the edges of the grid
        if (direction.magnitude > 2f)
        {
            if (target.x + 2f < transform.localPosition.x) { transform.Translate(-Grid.width, 0f, 0f, Space.Self); }
            if (target.x - 2f > transform.localPosition.x) { transform.Translate(Grid.width, 0f, 0f, Space.Self); }
            if (target.y + 2f < transform.localPosition.y) { transform.Translate(0f, -Grid.height, 0f, Space.Self); }
            if (target.y - 2f > transform.localPosition.y) { transform.Translate(0f, Grid.height, 0f, Space.Self); }
        }
        transform.Translate(direction.normalized * Time.deltaTime * MovementSpeed, Space.Self);
    }

    void Update()
    {
        //Head animation
        if (headAnimation.Any() && head != null)
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
                UpdatePosition(head.transform, direction, headAnimation.Peek().ToVector());
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
                UpdatePosition(body[i].transform, direction, bodyAnimation[i]);
            }
        }
    }
}
