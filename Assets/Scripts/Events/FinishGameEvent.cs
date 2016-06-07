using UnityEngine;
using System.Collections;

public class FinishGameEvent {
    public delegate void FinishGameAction(bool Player1Won);
    public static event FinishGameAction OnGameFinish;

    public static void Send(bool Player1Won)
    {
        if (OnGameFinish != null) OnGameFinish(Player1Won);
    }
}
