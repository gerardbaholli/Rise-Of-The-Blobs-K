using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{

    private IGridObject gridObject;

    private Transform cellTransform;
    private int column;
    private int row;

    public GridCell(Transform cellTransform, int column, int row)
    {
        this.cellTransform = cellTransform;
        this.column = column;
        this.row = row;
    }

    public void SetGridObject(IGridObject blob)
    {
        this.gridObject = blob;
    }

    public IGridObject GetGridObject()
    {
        return this.gridObject;
    }

    public Transform GetCellTransform()
    {
        return cellTransform;
    }

    public int GetColumn()
    {
        return column;
    }

    public int GetRow()
    {
        return row;
    }

}
