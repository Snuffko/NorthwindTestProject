using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshPro))]
[RequireComponent(typeof(BoxCollider2D))]
public class Card : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro _cardValueText;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _value;
    [SerializeField] private Vector3 _targetToMove;

    private bool _isRotate = false;
    private bool _isRotating = false;
    private Quaternion _targetQuternion;

    public event Action<Card> CardClick;

    public float Width { get; private set; }
    public float Height { get; private set; }

    private void Awake()
    {
        if (_cardValueText.TryGetComponent<TMPro.TextMeshPro>(out TMPro.TextMeshPro text))
        {
            text.text = _value.ToString();
        }
        if (TryGetComponent<BoxCollider2D>(out BoxCollider2D collider)) 
        {
            Width = collider.size.x; 
            Height = collider.size.y;
        }
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetToMove, _moveSpeed * Time.deltaTime);

        if (!_isRotating)
        {
            _isRotating = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetQuternion, 5f * Time.deltaTime);
            if (transform.rotation.z == 0 || transform.rotation.z == 180)
            {
                _isRotating = false;
            }
        }
    }

    void OnMouseUp()
    {
        if (!_isRotate && !_isRotating)
        {
            CardClick?.Invoke(this);     
        } 
    }

    public Card Create(int value)
    {
        _targetToMove = new Vector3(0, -5f, 0);
        _value = value;
        Card newCard = Instantiate(this, _targetToMove, transform.rotation);    
        return newCard;
    }

    public int GetValue()
    {
        return _value;
    }

    public void Open()
    {
        _targetQuternion = new Quaternion(0f, 0f, 0f, 1f);
        _isRotate = true;
    }


    public void Close()
    {
        _targetQuternion = new Quaternion(0f, 1f, 0f, 0f);
        _isRotate = false;
    }

    public void SetTargetPosition(Vector3 position)
    {
        _targetToMove = position;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
