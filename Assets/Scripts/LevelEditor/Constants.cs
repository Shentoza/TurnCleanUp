using System.Collections;
using UnityEngine;

public class Constants
{
    public const string FILE_EXTENSION = ".cwmap";
    public static readonly string[] LEVEL_ITEM_TAGS = { "LevelItem", "CosmeticItem" };
    public const string MAP_FILE_BEGINNING = "CivilWarNationMap";

    public enum COMPONENT_FLAGS
    {
        EndOfObject,
        Cell,
        ObjectSetter,
        ChildObject,
        Count
    };

    public enum OBJECT_FLAGS
    {
        EndOfFile,
        NewObject,
        Count
    };

    public const int DEFAULT_GRID_WIDTH = 100;
    public const int DEFAULT_GRID_HEIGHT = 100;
    public const int DEFAULT_OBJECT_COUNT = 0;

}