using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class URManager : MonoBehaviour{

    private static List<URAction> list = new List<URAction>();
    private static int currentIndex = -1;

    public void Start()
    {
        list = new List<URAction>();
        currentIndex = -1;
    }

    public static void addAction(URAction action)
    {
        list.Insert(++currentIndex, action);
        for(int i = list.Count-1; i > currentIndex;--i) {
            URAction current = list[i];
            if(null != current) {
                current.Delete();
                list.Remove(current);
            }
        }
    }

    public static void undo()
    {
        if(currentIndex >= 0) {
            list[currentIndex--].Undo();
        }
    }

    public static void redo()
    {
        if(currentIndex < (list.Count-1)) {
            list[++currentIndex].Redo();
        }
    }

    public static URAction getCurrentAction()
    {
        if(list.Count > 0) {
            if(currentIndex < list.Count)
                return list[currentIndex];
        }
        return null;
    }
}
