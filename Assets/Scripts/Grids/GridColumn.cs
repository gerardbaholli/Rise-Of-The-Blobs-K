using Services;
using System;
using UnityEngine;

public class GridColumn
{
    private int columnIndex;
    private int columnHeight;
    private GridCell[] gridCellArray;
    private Transform gridColumnTransform;


    public GridColumn(int columnIndex, int columnHeight)
    {
        this.columnIndex = columnIndex;
        this.columnHeight = columnHeight;

        gridCellArray = new GridCell[columnHeight];

        for (int y = 0; y < columnHeight; y++)
        {
            gridCellArray[y] = new GridCell(y);
        }
    }

    public void SetGridColumnTransform(Transform gridColumnTransform)
    {
        this.gridColumnTransform = gridColumnTransform;
    }

    public Transform GetGridColumnTransform()
    {
        return gridColumnTransform;
    }

    public GridCell[] GetGridCellsArray()
    {
        return gridCellArray;
    }

}
