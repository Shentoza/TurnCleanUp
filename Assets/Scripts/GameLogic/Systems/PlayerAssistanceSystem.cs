using UnityEngine;
using System.Collections;

public class PlayerAssistanceSystem : MonoBehaviour {

    public static PlayerAssistanceSystem instance{ get; private set; }

    bool drawingWalkPath;
    ArrayList walkPath = new ArrayList();
    bool drawingThrowPath;
    ArrayList throwPath = new ArrayList();

    [SerializeField]
    private Material defaultMat;
    [SerializeField]
    private Material pathMaterial;
    [SerializeField]
    private Material begehbarMat;
    [SerializeField]
    private Material attackableMat;
    [SerializeField]
    private Material highlightedMat;

    private Cell highlightedCell;

    static GameObject selectedUnit;
    static int moveRange;
    static int attackRange;

    static public GameObject Initialize(GameObject prefab, GameObject toAdd)
    {
        GameObject result;
        if (!instance.Equals(null))
            Destroy(instance);

        if (!toAdd.Equals(null))
            result = toAdd;
        else
        { 
            result = new GameObject("PlayerAssistanceSystem");
        }

        instance = result.AddComponent<PlayerAssistanceSystem>();

        PlayerAssistanceSystem prefabComp = prefab.GetComponent<PlayerAssistanceSystem>();
        instance.defaultMat = prefabComp.defaultMat;
        instance.pathMaterial = prefabComp.pathMaterial;
        instance.begehbarMat = prefabComp.begehbarMat;
        instance.attackableMat = prefabComp.attackableMat;
        instance.highlightedMat = prefabComp.highlightedMat;

        UnitSelectionEvent.OnUnitSelection += instance.UnitSelection;

        return result;
    }

    void UnitSelection(GameObject unit)
    {
        selectedUnit = unit;
        AttributeComponent selectedAttribute = unit.GetComponent<AttributeComponent>();
        moveRange = selectedAttribute.actMovRange;
        attackRange = selectedAttribute.weapon.GetComponent<WeaponComponent>().weaponRange;
    }

    void OnDestroy()
    {
        UnitSelectionEvent.OnUnitSelection -= instance.UnitSelection;
    }

    static public void highlightSingleCell(Cell cell)
    {
        MeshRenderer meshRend = (MeshRenderer)cell.gameObject.GetComponent(typeof(MeshRenderer));
        meshRend.material = instance.highlightedMat;
        meshRend.enabled = true;
    }

    static public void resetSingleCell(Cell cell)
    {
        MeshRenderer meshRend = (MeshRenderer)cell.gameObject.GetComponent(typeof(MeshRenderer));
        meshRend.material = instance.defaultMat;
        meshRend.enabled = false;
    }

    static public void colorAllCells()
    {
        for (int i = 0; i < (BattlefieldCreater.mapSizeX); ++i)
            for (int j = 0; j < (BattlefieldCreater.mapSizeZ); ++j)
            {
                Cell currentCell = BattlefieldCreater.instance.getCell(i, j);
                colorCell(currentCell);
            }
    }

    static public void resetAllCellColors()
    {
        for (int i = 0; i < (BattlefieldCreater.mapSizeX); ++i)
            for (int j = 0; j < (BattlefieldCreater.mapSizeZ); ++j)
            {
                Cell currentCell = BattlefieldCreater.instance.getCell(i, j);
                resetSingleCell(currentCell);
            }
    }

    static public void colorCell(Cell cell)
    {
        MeshRenderer meshRend = (MeshRenderer)cell.gameObject.GetComponent(typeof(MeshRenderer));
        meshRend.enabled = true;
        if (cell.dij_GesamtKosten <= moveRange)
            meshRend.material = instance.begehbarMat;
        else if (cell.dij_GesamtKosten <= moveRange + attackRange)
            meshRend.material = instance.attackableMat;
        else
            meshRend.enabled = false;
    }

    /*Todo: neuen pfad nur bei wechselndem Pfad zeichnen (aktuell jeder Frame neuer Pfad)
    *irgendwelche fckn nullpointer fliegen
    */
    static public void PaintWalkPath(ArrayList path)
    {
        ClearWalkPath();
        MeshRenderer meshr;
        Cell currentCell = null;
        for(int i = 0; i < path.Count;++i)
        {
            currentCell = (Cell)path[i];
            meshr = (MeshRenderer)currentCell.gameObject.GetComponent(typeof(MeshRenderer));
            instance.walkPath.Add(currentCell);

            meshr.material = instance.pathMaterial;

            
        }
    }

    static public void ClearThrowPath()
    {

    }

    static public void ClearWalkPath()
    {
        foreach (Cell current in instance.walkPath)
        {
            colorCell(current);
        }
        instance.walkPath.Clear();
    }
}
