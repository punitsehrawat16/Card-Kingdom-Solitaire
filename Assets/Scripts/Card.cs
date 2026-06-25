using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public CardData cardData { get; private set; }
    [SerializeField] private SpriteRenderer cardVisual;
    public static event Action<Card> CardClicked;
    private bool isInteractable = true;
    public void CardInitialise(CardData cardinfo, Sprite sr)
    {
        cardData = cardinfo;
        cardVisual.sprite = sr;
    }
    public void CardInteraction(bool isCenterCard)
    {
        isInteractable = !isCenterCard;  
    }
    public void OnMouseDown()
    {
        if(!isInteractable)
            return;
        CardClicked.Invoke(this);
    }
}
