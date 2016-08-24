using System.Collections;
using UnityEngine;

public class Constants
{
    public const string FILE_EXTENSION = ".cwmap";
    public static readonly string[] FILE_LEVEL_ITEM_TAGS = { "LevelItem", "CosmeticItem" };
    public const string FILE_BEGINNING_TAG = "CivilWarNationMap";
    public const string PROPS_PREFAB_PATH = "Prefabs/PrefabsFinalTest/FinalProbs/";
    public const string FILE_NO_PREFAB_TAG = "?NoPrefab?";
    public enum FILE_COMPONENT_FLAGS
    {
        EndOfObject,
        ObjectComponent,
        ObjectSetter,
        ChildObject,
        Count
    };

    public enum FILE_OBJECT_FLAGS
    {
        EndOfFile,
        NewObject,
        Count
    };

    public const int DEFAULT_GRID_WIDTH = 100;
    public const int DEFAULT_GRID_HEIGHT = 100;
    public const int DEFAULT_OBJECT_COUNT = 0;
}