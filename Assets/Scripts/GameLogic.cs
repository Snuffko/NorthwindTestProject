using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(Interface))]
[RequireComponent(typeof(ResultPanel))]
public class GameLogic : MonoBehaviour
{
    [SerializeField] private Deck _deck;
    [SerializeField] private Interface _interface;
    [SerializeField] private ResultPanel _resultPanel;
    [SerializeField] private int _tryCount;
    [SerializeField] private int _playerLevel;
    [SerializeField] private int _minutesBetweenSession;
    [SerializeField] private int _maxCardNumber;

    private List<Card> _cardsToCompare;
    private int _openCardCount;
    private int _currentTryCount;
    private int _numbersFoundCount;
    private int _currentMaxCardNumber;
    private int _level;
    private int _score;
    private bool _isRoundStart;

    public event Action<int> ScoreChange;
    public event Action<int> TryCountChange;
    public event Action<int> LevelChange;
    public event Action<int> CardCountChange;

    private void Start()
    {
        SessionStart();
    }

    public void SessionStart()
    {
        _interface.SessionStart();
        _resultPanel.SessionStart();

        _level = 1;
        _currentMaxCardNumber = _maxCardNumber;
        _cardsToCompare = new List<Card>();
        _currentTryCount = _tryCount;
        _openCardCount = 0;
        _numbersFoundCount = 0;

        StartCoroutine(StartNewRound(0.5f));
    }

    private IEnumerator StartNewRound(float seconds) {
        _isRoundStart = false;
        ScoreChange?.Invoke(_score);
        TryCountChange?.Invoke(_tryCount);
        LevelChange?.Invoke(_level);

        _deck.AddCardCouples(_currentMaxCardNumber);
        CardCountChange?.Invoke(_deck.GetCount());
        _deck.Collect();
        yield return new WaitForSeconds(seconds);
        _deck.Shuffle();
        _deck.Layout();
        yield return new WaitForSeconds(seconds/2);
        _isRoundStart = true;
    }

    public void OnCardClick(Card card)
    {
        if (_openCardCount < 2 && _isRoundStart)
        {
            _openCardCount++;
            card.Open();
            _cardsToCompare.Add(card);
            if (_openCardCount == 2)
            {
                StartCoroutine(CompareOpenCard(_cardsToCompare, 0.75f));
            }
        }
    }

    private IEnumerator CompareOpenCard(List<Card> cardsValues, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (cardsValues[0].GetValue() == cardsValues[1].GetValue())
        {
            _score++;
            _numbersFoundCount++;
            ScoreChange?.Invoke(_score);
            IsAllNumbersFound();
        } 
        else
        {
            _currentTryCount--;
            TryCountChange?.Invoke(_currentTryCount);
            yield return new WaitForSeconds(seconds/2);
            foreach (Card card in cardsValues)
            {
                card.Close();
            }
            IsAllTryesLeft();
        }
        cardsValues.Clear();
        _openCardCount = 0;
    }

    private void IsAllNumbersFound()
    {
        if (_numbersFoundCount >= (_currentMaxCardNumber))
        {
            _level++;
            _numbersFoundCount = 0;
            _currentTryCount = _tryCount + ((_level > 10) ? _level-10 : 0);
            _currentMaxCardNumber++;
            LevelChange?.Invoke(_level);
            StartCoroutine(StartNewRound(1f));
        }
    }

    private void IsAllTryesLeft()
    {
        if (_currentTryCount <= 0)
        {
            _deck.DeleteCards();
            SessionEnd();
        }
    }

    public int GetLevel()
    {
        return _level;
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetCurrency()
    {
        return _score * 15 * _playerLevel;
    }

    public int GetPlayerLevel()
    {
        return _playerLevel;
    } 

    public int GetTimerMinutes()
    {
        return _minutesBetweenSession;
    }

    private void SessionEnd()
    {
        _interface.SessionEnd();
        _resultPanel.SessionEnd();
    }
}
    