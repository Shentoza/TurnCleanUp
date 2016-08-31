using System.Collections;
using UnityEngine;

public class Constants
{
    public const string FILE_BEGINNING_TAG = "CivilWarNationMap";
    public const string FILE_EXTENSION = ".cwmap";
    public const string FILE_NO_PREFAB_TAG = "?NoPrefab?";
    public const string FILE_NO_MATERIAL_TAG = "?NoMaterial?";
    public const string FILE_END_OF_TEXTURE_CELLS = "?END_OF_TEXTURE_CELLS?";
    public const string FILE_END_OF_HEADER = "?HEADEND?";
    public static readonly string[] FILE_LEVEL_ITEM_TAGS = { "LevelItem", "CosmeticItem", "RebPlaceholder", "GovPlaceholder"};

    public static readonly string[] IGNORE_IN_PLAY_MODE_TAGS = { "RebPlaceholder", "GovPlaceholder" };

    public static string UNDO_REDO_TAG = "UndoRedo";
    public const string PROPS_PREFAB_PATH = "Prefabs/PrefabsFinalTest/FinalProbs/";
    public const string MATERIAL_PREFAB_PATH = "Materials/";
    public const string GROUND_MATERIAL_PATH = "Materials/IngameMats/";
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
    public const Material DEFAULT_CUBE_MATERIAL = null;
}