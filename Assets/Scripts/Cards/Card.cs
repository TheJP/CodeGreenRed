using Assets.Scripts.Cards.CardEffects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject highlightBorder;

    public void Highlight(bool value)
    {
        highlightBorder.SetActive(value);
    }
}
