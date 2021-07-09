﻿using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public enum GameState { Start, Pause, Play, GameOver }
public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton instance;
    private Score scoreKeeper;
    private SlowMotion slowMotion;
    private BulletManager bulletManager;
    private DuckSpawner duckSpawner;
    [SerializeField] private GameState gameState;
    [SerializeField] private GameEvent gameStartEvent;

    void Awake()
    {
        instance = this;
        scoreKeeper = GetComponent<Score>();
        slowMotion = GetComponent<SlowMotion>();
        bulletManager = GetComponent<BulletManager>();
    }

    private void Start()
    {
        gameState = GameState.Play;
        gameStartEvent?.Invoke();
    }

    public Score GetScore()
    {
        return scoreKeeper;
    }

    public SlowMotion GetSlowMotion()
    {
        return slowMotion;
    }

    public void Pause()
    {
        instance.gameState = GameState.Pause;
        Debug.Log("Paused Called");
        Time.timeScale = 0;
    }

    public void Resume()
    {
        //Debug.Log("Resume Game");

        instance.gameState = GameState.Play;
        Time.timeScale = 1;
    }

    public bool IsPaused()
    {
        if (gameState == GameState.Pause)
        {
            return true;
        }
        return false;
    }

    public BulletManager GetBulletManager()
    {
        return bulletManager;
    }
}
