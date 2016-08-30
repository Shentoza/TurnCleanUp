using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BrushAction : URAction
{
    private Dictionary<MeshRenderer, Material> oldValues;
    private Material newMaterial;

    public void addCell(MeshRenderer mr)
    {
        oldValues.Add(mr, mr.material);
    }

    public BrushAction(Material brushedMaterial)
    {
        oldValues = new Dictionary<MeshRenderer, Material>();
        newMaterial = brushedMaterial;
    }

    public void Delete()
    {
        oldValues.Clear();
    }

    public void Redo()
    {
        foreach(MeshRenderer mr in oldValues.Keys) {
            mr.material = newMaterial;
        }
    }

    public void Undo()
    {
        foreach(MeshRenderer mr in oldValues.Keys) {
            mr.material = oldValues[mr];
        }
    }
}
