using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public CardData cardData { get; private set; }
    [SerializeField] private SpriteRenderer cardVisual;

    public void CardInitialise(CardData cardinfo, Sprite sr)
    {
        cardData = cardinfo;
        cardVisual.sprite = sr;
    }
}
