using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(GameLogic))]
public class Interface : MonoBehaviour
{
    [SerializeField] private GameLogic _gameLogic;
    [SerializeField] private TMP_Text _tryCountValueDisplay;
    [SerializeField] private TMP_Text _scoreValueDisplay;
    [SerializeField] private TMP_Text _levelValueDisplay;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _gameLogic.ScoreChange += OnScoreChange;
        _gameLogic.TryCountChange += OnTryCountChange;
        _gameLogic.LevelChange += OnLevelChange;
    }

    private void OnDisable()
    {
        _gameLogic.ScoreChange -= OnScoreChange;
        _gameLogic.TryCountChange -= OnTryCountChange;
        _gameLogic.LevelChange -= OnLevelChange;
    }

    private void OnScoreChange(int score)
    {
        _scoreValueDisplay.text = score.ToString();
    }

    private void OnTryCountChange(int tryCount)
    {
        _tryCountValueDisplay.text = tryCount.ToString();
    }

    private void OnLevelChange(int level)
    {
        _levelValueDisplay.text = level.ToString();
    }
     
    public void SessionStart()
    {
        gameObject.SetActive(true);
    }

    public void SessionEnd()
    {
        gameObject.SetActive(false);
    }
}
