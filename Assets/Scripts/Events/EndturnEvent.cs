using UnityEngine;
using System.Collections;

public class EndturnEvent
{
    public delegate void EndTurnAction(bool PlayerOne);
    public static event EndTurnAction OnEndTurn;

    public static void Send(bool PlayerOne)
    {
        if (OnEndTurn != null) OnEndTurn(PlayerOne);
    }
}
