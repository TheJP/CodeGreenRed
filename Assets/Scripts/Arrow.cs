using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    public const float RotationSpeed = 180f;
    private const float Epsilon = 0.001f;

    public Sprite greenArrow;
    public Sprite redArrow;

    private float turnTo = 0f;

    public Teams Team
    {
        set
        {
            switch (value)
            {
                case Teams.Green:
                    GetComponent<SpriteRenderer>().sprite = greenArrow;
                    break;
                case Teams.Red:
                    GetComponent<SpriteRenderer>().sprite = redArrow;
                    break;
            }
        }
    }

    public void Turn(float from, float to)
    {
        turnTo = to;
        transform.eulerAngles = new Vector3(0f, 0f, from);
    }

    public void TurnTo(float target)
    {
        turnTo = target;
    }

    void Update()
    {
        var currentAngle = transform.eulerAngles.z;
        var diff = turnTo - currentAngle;
        if(diff > 180) { diff -= 360; }
        if (diff < -180) { diff += 360; }
        if (Mathf.Abs(diff) > Epsilon)
        {
            if (Mathf.Abs(diff) < RotationSpeed * Time.deltaTime) { currentAngle = turnTo; }
            else { currentAngle += Mathf.Sign(diff) * RotationSpeed * Time.deltaTime; }
            transform.eulerAngles = new Vector3(0f, 0f, currentAngle);
        }
    }
}
