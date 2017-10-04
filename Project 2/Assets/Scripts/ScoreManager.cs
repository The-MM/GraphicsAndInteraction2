﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private int scoreNum = 0;

    public const int RECTANGLE_SCORE = 100;
    public const int TRIANGLE_SCORE = 200;
    public const int CIRCLE_SCORE = 300;

    public static ScoreManager instance = null;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start ()
    {
        Text score = this.GetComponent<Text>();
        score.text = "Score: " + scoreNum.ToString();
    }

    public void DestroyedBrick(GameManager.BrickTypes type)
    {
        // Increments score based on brick type
        int scoreIncrement = 0;
        switch(type)
        {
            case GameManager.BrickTypes.RECTANGLE:
                scoreIncrement = RECTANGLE_SCORE;
                break;
            case GameManager.BrickTypes.TRIANGLE:
                scoreIncrement = TRIANGLE_SCORE;
                break;
            case GameManager.BrickTypes.CIRCLE:
                scoreIncrement = CIRCLE_SCORE;
                break;
        }

        // Updates text
        scoreNum += scoreIncrement;
        Text score = this.GetComponent<Text>();
        score.text = "Score: " + scoreNum.ToString();
    }
}