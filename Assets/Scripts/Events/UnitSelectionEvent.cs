using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UnitSelectionEvent
{
    public delegate void SelectUnitAction(GameObject unit);
    public static event SelectUnitAction OnUnitSelection;

    public static void Send(GameObject unit)
    {
        Debug.Log("Seend");
        if (OnUnitSelection != null) OnUnitSelection(unit);
    }
}
