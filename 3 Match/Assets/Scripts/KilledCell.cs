using System;
using System.Collections;
using StaticData;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KilledCell : MonoBehaviour
{
    [HideInInspector] public bool isFalling;

    [SerializeField] private float _speed = 16f;
    [SerializeField] private float _gravity = 32f;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    private Vector2 _moveDirection;

    public void Initialize(Sprite sprite, Vector2 startPosition)
    {
        isFalling = true;

        _moveDirection = Vector2.up;
        _moveDirection.x = Random.Range(-1f, 1f);
        _moveDirection *= _speed / 2;

        _image.sprite = sprite;
        _rect.anchoredPosition = startPosition;

        StartCoroutine(WaitForDeath());
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isFalling)
            return;
        _moveDirection.y -= Time.deltaTime * _gravity;
        _moveDirection.x = Mathf.Lerp(_moveDirection.x, 0, Time.deltaTime);
        _rect.anchoredPosition += _moveDirection * (Time.deltaTime * _speed);
        if (_rect.position.x < -Config.PieceSize
            || _rect.position.x > Screen.width * Config.PieceSize
            || _rect.position.y < -Config.PieceSize
            || _rect.position.y > Screen.height * Config.PieceSize)
            isFalling = false;
    }
}