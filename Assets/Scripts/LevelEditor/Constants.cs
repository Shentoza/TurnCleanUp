using UnityEngine;
using System.Collections;

public class Constants
{
    public const string GAME_SETTINGS_TAG = "Settings";
    public const string FILE_ENDING = ".cwmap";
    public readonly string[] LEVEL_ITEM_TAGS = { "LevelItem", "CosmeticItem" };
    public const string MAP_FILE_BEGINNING = "CivilWarNationMap";
    public enum COMPONENT_FLAGS { EndOfObject, Cell, ObjectSetter, Count};

}