using UnityEngine;
using TMPro;

[RequireComponent(typeof(GameLogic))]
public class ResultPanel : MonoBehaviour
{
    [SerializeField] private GameLogic _gameLogic;
    [SerializeField] private TMP_Text _playerLevelValueDisplay;
    [SerializeField] private TMP_Text _scoreValueDisplay;
    [SerializeField] private TMP_Text _levelValueDisplay;
    [SerializeField] private TMP_Text _currencyValueDisplay;
    [SerializeField] private TMP_Text _timerValueDisplay;
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private GameObject _startNewSessionButton;

    private bool _isTimerEnabled;
    private float _timer;

    private void Start()
    {
        _isTimerEnabled = false;
        _timer = 0f;
        gameObject.SetActive(false);
    }

    private void Update()
    { 
        if (_isTimerEnabled)
        {           
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                float timerRound = Mathf.Round(_timer);
                float seconds = timerRound % 60;               
                float minutes = Mathf.Floor(timerRound / 60);
                _timerValueDisplay.text = minutes.ToString() + ':' + (seconds < 10 ? "0" : "") + seconds.ToString();
            } 
            else
            {
                _timerPanel.SetActive(false);
                _startNewSessionButton.SetActive(true);
            }
        }
    }

    public void OnStartNewSessionButtonClick()
    {
        _gameLogic.SessionStart();
    }

    public void SessionStart()
    {
        gameObject.SetActive(false);
        _timer = 0;
        _isTimerEnabled = false;
        _startNewSessionButton.SetActive(false);
    }

    public void SessionEnd()
    {
        gameObject.SetActive(true);

        _levelValueDisplay.text = _gameLogic.GetLevel().ToString();
        _scoreValueDisplay.text = _gameLogic.GetScore().ToString();
        _playerLevelValueDisplay.text = _gameLogic.GetPlayerLevel().ToString();    
        _currencyValueDisplay.text = _gameLogic.GetCurrency().ToString();

        _timer = _gameLogic.GetTimerMinutes() * 60;
        _isTimerEnabled = true;
        _timerPanel.SetActive(true);
    }
}
