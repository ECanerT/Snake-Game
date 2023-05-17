using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoreWindow : MonoBehaviour
{
    private TMP_Text ScoreText;
    private void Awake()
    {
        ScoreText = transform.Find("ScoreText").GetComponent<TMP_Text>();
    }
    private void Update()
    {
        Debug.Log(ScoreText);
        if (ScoreText != null)
        {
        ScoreText.text = GameHandler.GetScore().ToString();
        }
    }
}
