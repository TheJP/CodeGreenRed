using Assets.Scripts.Cards.CardEffects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardType type;
    public GameObject highlightBorder;

    public void Highlight(bool value)
    {
        highlightBorder.SetActive(value);
    }
}
