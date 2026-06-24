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

    [Header("Center Card")] [SerializeField]
    private CardData centerCard;
    [SerializeField] private Transform centerCardPos;
    
    private void Start()
    {
        CreateDeck();
        ShufflingDeck();
        CenterCard();
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
        // random no generation for basically how much cards we need to shuffle 
        // temp index and then i would be taking a random index and both them would be replaced and then again same 

        int shuffleRatio = Random.Range(21, 46);
        int shuffleIndex=0;
        GameObject temp = new GameObject();
        temp.name = "Temp";
        while (shuffleIndex<shuffleRatio)
        {
            int cardToBeReplaced = Random.Range(27, 51);
            int cardReplaceWithWhom = Random.Range(0, 27);
            CardData tempData = new CardData();
            tempData = deckCardsData[cardToBeReplaced];
            deckCardsData[cardToBeReplaced] = deckCardsData[cardReplaceWithWhom];
            deckCardsData[cardReplaceWithWhom] = tempData;
            shuffleIndex++;
        }
    }

    private void CenterCard()
    {
        int index = Random.Range(0, 52);
        centerCard.Rank = deckCardsData[index].Rank;
        centerCard.Suits = deckCardsData[index].Suits;
        
        foreach (var a in cardsInDeck)
        {
            Card card = a.GetComponent<Card>();
            if (card.cardData.Rank == centerCard.Rank && card.cardData.Suits == centerCard.Suits)
            {
                a.transform.position = centerCardPos.position;
                break;
            }
        }
    }
}
