using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameMainState
{
    None,
    Playing,
    Event,
}

public interface IGameMainStateParam
{
    event Action<GameMainState> OnChangeGameMainState;

    void OnRefresh();
}

public class GameMainStateParam : IGameMainStateParam
{
    private GameMainState m_gameMainState = GameMainState.None;
    public event Action<GameMainState> OnChangeGameMainState = default;

    public void ChangeStatus(GameMainState gameMainState)
    {
        m_gameMainState = gameMainState;
        OnChangeGameMainState?.Invoke(m_gameMainState);
    }

    public void OnRefresh()
    {
        OnChangeGameMainState?.Invoke(m_gameMainState);
    }
}
