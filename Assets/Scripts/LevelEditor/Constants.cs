using UnityEngine;
using System.Collections;

public class Constants
{
    public const string GAME_SETTINGS_TAG = "Settings";
    public const string FILE_ENDING = ".cwmap";
    public readonly string[] LEVEL_ITEM_TAGS = { "LevelItem", "CosmeticItem" };
    public const string MAP_FILE_BEGINNING = "CivilWarNationMap";
    public enum COMPONENT_FLAGS { EndOfObject, Cell, ObjectSetter, Count};
    public enum OBJECT_FLAGS { EndOfFile, NewObject};

    public const int DEFAULT_GRID_WIDTH = 100;
    public const int DEFAULT_GRID_HEIGHT = 100;
    public const int DEFAULT_OBJECT_COUNT = 0;

}