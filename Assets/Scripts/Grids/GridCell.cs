using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private IGridObject gridObject;

    private GridColumn gridColumn;
    private Transform gridCellTransform;
    private int cellIndex;

    public GridCell(int cellIndex)
    {
        this.cellIndex = cellIndex;
    }

    public int GetCellIndex()
    {
        return cellIndex;
    }

    public void SetGridCellColumn(GridColumn gridColumn)
    {
        this.gridColumn = gridColumn;
    }

    public GridColumn GetGridCellColumn()
    {
        return gridColumn;
    }

    public void SetGridCellTransform(Transform gridColumnTransform)
    {
        this.gridCellTransform = gridColumnTransform;
    }

    public Transform GetGridCellTransform()
    {
        return gridCellTransform;
    }

    public void SetGridObject(IGridObject blob)
    {
        this.gridObject = blob;
    }

    public IGridObject GetGridObject()
    {
        return this.gridObject;
    }

}
