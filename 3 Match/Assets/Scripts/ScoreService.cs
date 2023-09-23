using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreService : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    private int _score;

    private void Start()
    {
        _score = 0;
        _scoreText.text = "0";
    }

    public void AddScore(int score)
    {
        _score += score;
        _scoreText.text = _score.ToString();
        if (DOTween.IsTweening(_scoreText.transform))
            DOTween.Kill(_scoreText.transform);
        DOTween.Sequence()
            .Append(_scoreText.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f))
            .Append(_scoreText.transform.DOScale(Vector3.one, 0.3f));
    }
}