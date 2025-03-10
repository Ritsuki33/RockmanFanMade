using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateInfo
{
    public GameMainStateParam GameMainStateParam => (GameMainManager.Instance != null) ? GameMainManager.Instance.gameMainStateParam : null;
}
