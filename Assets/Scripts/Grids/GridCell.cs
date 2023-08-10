using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public GameObject cellGameObject { get; private set; }
    public GridObject gridObject { get; set; }

    public GridCell(GameObject gridCellGO, int x, int y)
    {
        this.cellGameObject = gridCellGO;
        X = x;
        Y = y;
    }

    public Transform GetTransform()
    {
        return cellGameObject.transform;
    }

    public Vector3 GetPosition()
    {
        return cellGameObject.transform.position;
    }

    public Quaternion GetRotation()
    {
        return cellGameObject.transform.rotation;
    }

    public void SetAndMoveGridObject(GridObject gridObject)
    {
        if (this.gridObject != null)
            Debug.LogError("SetAndMoveGridObject: not a free position!");

        // Set
        this.gridObject = gridObject;
        
        // Move
        gridObject.transform.position = GetPosition();
        gridObject.transform.rotation = GetRotation();

    }

}
