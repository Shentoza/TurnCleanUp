using UnityEngine;
using System.Collections;


//Level Configuration nach dem Singleton Pattern
public class LevelConfiguration {
    public bool defaultValues = true;
    public int gridWidth;
    public int gridHeight;
    public int objectCount;

    public static readonly LevelConfiguration instance = new LevelConfiguration();

    static LevelConfiguration()
    {
    }

    private LevelConfiguration()
    {
        defaultValues = true;
        gridWidth = Constants.DEFAULT_GRID_WIDTH;
        gridHeight = Constants.DEFAULT_GRID_HEIGHT;
        objectCount = Constants.DEFAULT_OBJECT_COUNT;
    }
}
