using UnityEngine;
using System.Collections;

public class SpendAPEvent
{
    public delegate void SpendAPAction(int amount, PlayerComponent player);
    public static event SpendAPAction OnAPSpent;

    public static void Send(int amount, PlayerComponent player)
    {
        if (OnAPSpent != null) OnAPSpent(amount, player);
    }
}