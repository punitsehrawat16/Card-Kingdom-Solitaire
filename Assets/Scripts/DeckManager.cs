using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    
    [Header("Cards Visuals")]
    [SerializeField] private List<Sprite> spriteCards = new List<Sprite>(); 
    
    
    [Header("Cards Data")] 
    [SerializeField] private  List<CardData> deckCardsData = new List<CardData>();
    [SerializeField] private List<GameObject> cardsInDeck = new List<GameObject>();
    
    
    [Header("Cards Relevant")]
    [SerializeField] private GameObject cardHolder;
    [SerializeField] private GameObject cardPrefab;
    
    
    private void Start()
    {
        deckCardsData.Clear();
        cardsInDeck.Clear();
        
        
    }

    public void CreateAndShuffleDeck()
    {
        CreateDeck();
        ShufflingDeck();
    }
    private void CreateDeck()
    {
        int cardIndex = 0; // for sprites 
        foreach (Suits s in System.Enum.GetValues(typeof(Suits)))
        {
            foreach (Rank r in System.Enum.GetValues(typeof(Rank)))
            {
                CardData cardData = new CardData();
                cardData.Suits = s;
                cardData.Rank = r;
                deckCardsData.Add(cardData);
                
                string str = cardData.Suits.ToString() +" "+ cardData.Rank.ToString();
                
                GameObject card = Instantiate(cardPrefab,cardHolder.transform);
                Sprite sr = spriteCards[cardIndex];
                Card _card = card.GetComponent<Card>();
                if(_card != null)
                    _card.CardInitialise(cardData,sr);
                card.name = str;
                cardIndex++;
                cardsInDeck.Add(card);
            }
        }
    }
    
    private void ShufflingDeck()
    {
        for (int i = deckCardsData.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            CardData temp = deckCardsData[i];
            deckCardsData[i] = deckCardsData[randomIndex];
            deckCardsData[randomIndex] = temp;

            GameObject tempObj = cardsInDeck[i];
            cardsInDeck[i] = cardsInDeck[randomIndex];
            cardsInDeck[randomIndex] = tempObj;
        }
        Debug.Log(" Top Card After Shuffle: "+deckCardsData[0].Rank+" "+deckCardsData[0].Suits);

    }

    public Card DrawTopCard(Transform pos)
    {
        //CardData cardData = deckCardsData[0];
        var card = cardsInDeck[0].GetComponent<Card>();
        deckCardsData.RemoveAt(0);
       
        cardsInDeck[0].transform.position = pos.position;
        cardsInDeck.RemoveAt(0);

        return card;
    }
}
