using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    [Header("Card Data")]
    public CardData cardData { get; private set; }
    [SerializeField] private SpriteRenderer cardVisual;
    
    //other
    public static event Action<Card> CardClicked;
    private bool isInteractable = false;
    
    public void CardInitialise(CardData cardinfo, Sprite sr)
    {
        cardData = cardinfo;
        cardVisual.sprite = sr;
    }
    
    public void CardInteraction(bool isHandCard)
    {
        isInteractable = isHandCard;
    }
    
    public void OnMouseDown()
    {
        if(!isInteractable)
            return;
        
        CardClicked.Invoke(this);
    }
}
