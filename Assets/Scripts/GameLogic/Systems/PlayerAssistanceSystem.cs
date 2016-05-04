using UnityEngine;
using System.Collections;

public class PlayerAssistanceSystem : MonoBehaviour {

    bool drawingWalkPath;
    ArrayList walkPath = new ArrayList();
    bool drawingThrowPath;
    ArrayList throwPath = new ArrayList();

    DijkstraSystem dij;

    public Material oneArrow;
    public Material startArrow;
    public Material endArrow;
    public Material middleArrow;

    public Material pathMaterial;

	// Use this for initialization
	void Start () {
	    dij = (DijkstraSystem)FindObjectOfType(typeof(DijkstraSystem));
    }
	
	// Update is called once per frame
	void Update () {
	
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
            mr.material = dij.begebarMat;
        }

        walkPath.Clear();
    }
}
