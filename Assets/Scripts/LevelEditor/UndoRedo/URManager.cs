using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class URManager {

    private static List<URAction> list = new List<URAction>();
    private static int currentIndex = -1;

    public static void addAction(URAction action)
    {
        list.Insert(++currentIndex, action);
        for(int i = list.Count-1; i > currentIndex;--i) {
            URAction current = list[i];
            if(null != current) {
                current.delete();
                list.Remove(current);
            }
        }
    }

    public static void undo()
    {
        if(currentIndex >= 0) {
            list[currentIndex--].undo();
        }
    }

    public static void redo()
    {
        if(currentIndex < (list.Count-1)) {
            list[currentIndex++].redo();
        }
    }
}
