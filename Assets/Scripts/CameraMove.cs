using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameLogic))]
[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameLogic _gameLogic;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed;

    private float _nowCameraSize;
    private float _targetCameraSize;

    private void Start()
    {
        _nowCameraSize = _camera.orthographicSize;
        _targetCameraSize = _camera.orthographicSize;
    }


    private void Update()
    {
        if (_nowCameraSize < _targetCameraSize)
        {
            Debug.Log(_nowCameraSize);
            _nowCameraSize += Time.deltaTime * _speed;
            _camera.orthographicSize = _nowCameraSize;
        }
    }


    private void OnEnable()
    {
        _gameLogic.CardCountChange += OnCardCountChange;
    }

    private void OnDisable()
    {
        _gameLogic.CardCountChange -= OnCardCountChange;
    }

    private void OnCardCountChange(int cardCount)
    {   
        int cardIntRow = cardCount / 3;
        if (cardIntRow > 7)
        {
            _targetCameraSize += 1f * (cardIntRow - 7);
        }
    }

}
