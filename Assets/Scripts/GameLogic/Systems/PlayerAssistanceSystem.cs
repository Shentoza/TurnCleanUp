using UnityEngine;
using System.Collections;

public class PlayerAssistanceSystem : MonoBehaviour {

    bool drawingWalkPath;
    ArrayList walkPath = new ArrayList();
    bool drawingThrowPath;
    ArrayList throwPath = new ArrayList();

    [SerializeField]
    private Material defaultMat;
    [SerializeField]
    private Material pathMaterial;
    [SerializeField]
    private Material begebarMat;
    [SerializeField]
    private Material attackableMat;
    [SerializeField]
    private Material highlightedMat;

    

    public void highlightSingleCell(Cell cell)
    {
        MeshRenderer meshRend = (MeshRenderer)cell.gameObject.GetComponent(typeof(MeshRenderer));
        meshRend.material = highlightedMat;
        meshRend.enabled = true;
    }

    public void resetSingleCell(Cell cell)
    {
        MeshRenderer meshRend = (MeshRenderer)cell.gameObject.GetComponent(typeof(MeshRenderer));
        meshRend.material = defaultMat;
        meshRend.enabled = false;
    }

    public void colorAllCells(int moveRange, int attackRange)
    {
        for (int i = 0; i < (battleField.sizeX * 10); ++i)
            for (int j = 0; j < (battleField.sizeZ * 10); ++j)
            {
                Cell currentCell = battleField.getCell(i, j);
                colorCell(currentCell, moveRange, attackRange);
            }
    }

    public void resetAllCellColors()
    {
        for (int i = 0; i < (battleField.sizeX * 10); ++i)
            for (int j = 0; j < (battleField.sizeZ * 10); ++j)
            {
                Cell currentCell = battleField.getCell(i, j);
                resetSingleCell(currentCell);
            }
    }

    public void colorCell(Cell cell, int moveRange, int attackRange)
    {
        MeshRenderer meshRend = (MeshRenderer)cell.gameObject.GetComponent(typeof(MeshRenderer));
        meshRend.enabled = true;
        if (cell.dij_GesamtKosten <= moveRange)
            meshRend.material = begebarMat;
        else if (cell.dij_GesamtKosten <= moveRange + attackRange)
            meshRend.material = attackableMat;
        else
            meshRend.enabled = false;
    }

    /*Todo: neuen pfad nur bei wechselndem Pfad zeichnen (aktuell jeder Frame neuer Pfad)
    *irgendwelche fckn nullpointer fliegen
    */
    public void PaintWalkPath(ArrayList path)
    {
        ClearWalkPath();
        MeshRenderer meshr;
        Cell currentCell = null;
        for(int i = 0; i < path.Count;++i)
        {
            currentCell = (Cell)path[i];
            meshr = (MeshRenderer)currentCell.gameObject.GetComponent(typeof(MeshRenderer));
            walkPath.Add(currentCell);

            meshr.material = pathMaterial;

            
        }
    }

    public void ClearThrowPath()
    {

    }

    public void ClearWalkPath()
    {
        foreach (Cell current in walkPath)
        {
            MeshRenderer mr = (MeshRenderer)current.gameObject.GetComponent(typeof(MeshRenderer));
            mr.material = begebarMat;
        }
        walkPath.Clear();
    }
}
