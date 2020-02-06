using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Card))]
[RequireComponent(typeof(GameLogic))]
public class Deck : MonoBehaviour
{
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private GameLogic _gameLogic;

    private List<Card> _deck;

    private void Awake()
    {
        _deck = new List<Card>();
    } 

    public void AddCardCouples(int newMaxCardNumber)
    {
        for (int i = _deck.Count / 2; i < newMaxCardNumber; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Card card = _cardPrefab.Create(i+1);
                _deck.Add(card);
                card.CardClick += _gameLogic.OnCardClick;
            }
        }  
    }

    public void Collect()
    {      
        float x = 0.00f;
        float y = -5.00f;
        foreach (var card in _deck)
        {
            card.Close();
            Vector3 Position = new Vector3(x, y, 0);
            card.SetTargetPosition(Position);
            x += 0.025f;
            y += 0.025f;
        }
    }

    public void Shuffle()
    {
        for (int i = _deck.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);      
            var temp = _deck[j];
            _deck[j] = _deck[i];
            _deck[i] = temp;
        }    
    }

    public void Layout()
    {
        int cardInRow = _deck.Count / 3;
        float CardWidth = _deck[0].Width;
        float CardHeight = _deck[0].Height;
        float startPositionX = -(CardWidth * cardInRow / 2f) - 0.5f;
        float startPositionY = -0.5f;
        int PositionYModificator = 1;
        int i = 0;
        foreach (Card card in _deck)
        {
            if (i > cardInRow)
            {
                PositionYModificator--;
                i = 0;
            }
            card.SetTargetPosition(new Vector3(startPositionX + ((CardWidth + 0.1f) * i), startPositionY + ((CardHeight + 0.1f) * PositionYModificator), 0));
            i++;
        }
    }

    public void DeleteCards()
    {
        foreach (Card card in _deck)
        {
            card.CardClick -= _gameLogic.OnCardClick;
            card.Delete();
        }
        _deck.Clear();
    }

    public int GetCount()
    {
        return _deck.Count;
    }
}
