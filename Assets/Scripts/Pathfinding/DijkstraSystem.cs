using UnityEngine;
using System.Collections;
using System;

public class DijkstraSystem : MonoBehaviour {

    [SerializeField]
    static private BattlefieldCreater battleField;
    

    static private CellComparer comp = new CellComparer();
    static private ArrayList abgeschlosseneZellen = new ArrayList();


    static public bool initialize()
    {
        battleField = FindObjectOfType<BattlefieldCreater>();
        if (battleField == null)
            return false;


        return true;
    }

    static public void executeDijsktra(Cell start, int moveRange,int attackRange)
    {
        resetDijkstra();

        start.dij_GesamtKosten = 0;
        ArrayList entdeckteZellen = new ArrayList();
        entdeckteZellen.Add(start);

        while(entdeckteZellen.Count != 0)
        {
            entdeckteZellen.Sort(comp);
            Cell currentCell = (Cell)entdeckteZellen[entdeckteZellen.Count-1];
            entdeckteZellen.RemoveAt(entdeckteZellen.Count-1);
            currentCell.dij_ZellZustand = Cell.dij_Zustand.DIJ_ABGESCHLOSSEN;
            if (currentCell.dij_GesamtKosten > moveRange + attackRange)
                return;
            for(int i = 0; i < currentCell.neighbours.Count; ++i)
            {
                Cell currentNeighbour = (Cell)currentCell.neighbours[i];
                if (currentNeighbour.dij_ZellZustand != Cell.dij_Zustand.DIJ_ABGESCHLOSSEN)
                {
                    updateDistance(currentNeighbour, currentCell, entdeckteZellen);
                }
            }
        }
    }
    static void updateDistance(Cell zielKnoten, Cell vorgaenger, ArrayList entdeckteZellenListe)
    {
        if(zielKnoten.isOccupied)
        {
            zielKnoten.dij_ZellZustand = Cell.dij_Zustand.DIJ_ABGESCHLOSSEN;
            zielKnoten.dij_Vorgaenger = null;
            return;
        }
        if((vorgaenger.dij_GesamtKosten + zielKnoten.dij_KnotenKosten) < zielKnoten.dij_GesamtKosten)
        {
            zielKnoten.dij_Vorgaenger = vorgaenger;
            zielKnoten.dij_GesamtKosten = vorgaenger.dij_GesamtKosten + zielKnoten.dij_KnotenKosten;
            zielKnoten.dij_ZellZustand = Cell.dij_Zustand.DIJ_ENTDECKT;
            entdeckteZellenListe.Add(zielKnoten);
        }
    }

    static public ArrayList getPath(Cell startKnoten, Cell zielKnoten)
    {
        ArrayList result = new ArrayList();
        Cell currentNode = zielKnoten;
        Vector3 offset = new Vector3(0, 1, 0);
        Ray raycast = new Ray(startKnoten.transform.position+offset,zielKnoten.transform.position - startKnoten.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(raycast, raycast.direction.magnitude, LayerMask.NameToLayer("Cell"),QueryTriggerInteraction.Collide);
        Debug.DrawRay(raycast.origin, raycast.direction, Color.red, 10.0f);
        foreach (RaycastHit tmp in hits)
            Debug.Log(tmp.collider.gameObject.name);
        while(currentNode != startKnoten)
        {
            result.Add(currentNode);
            currentNode = currentNode.dij_Vorgaenger;
        }
        return result;
    }

    static public void resetDijkstra()
    {
        abgeschlosseneZellen.Clear();
        for(int i = 0; i < BattlefieldCreater.mapSizeX; ++i)
            for(int j = 0; j < BattlefieldCreater.mapSizeZ; ++j)
            {
                Cell currentCell = battleField.getCell(i, j);
                MeshRenderer meshRend = (MeshRenderer)currentCell.gameObject.GetComponent(typeof(MeshRenderer));
                meshRend.material = defaultMat;
                meshRend.enabled = false;
                currentCell.dij_ZellZustand = Cell.dij_Zustand.DIJ_UNBESUCHT;
                currentCell.dij_Vorgaenger = null;
                currentCell.dij_GesamtKosten = int.MaxValue;
             }
    }
}


//SORTS FROM HIGH TO LOW
public class CellComparer: IComparer
{
    int IComparer.Compare(object x, object y)
    {
        if(x.GetType() == y.GetType() && x.GetType() == typeof(Cell))
        {
            Cell xCell = (Cell)x;
            Cell yCell = (Cell)y;

            if (xCell.dij_GesamtKosten == yCell.dij_GesamtKosten)
                return 0;
            if (xCell.dij_GesamtKosten < yCell.dij_GesamtKosten)
                return 1;
            return -1;
        }
        return 0;
    }
}