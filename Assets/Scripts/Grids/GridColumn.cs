using Services;
using System;
using UnityEngine;

public class GridColumn
{
    private GridCell[] gridCellArray;

    private GridColumn prevGridColumn;
    private GridColumn nextGridColumn;

    private Transform gridColumnTransform;

    private GameManager gameManager;

    public GridColumn(GridCell[] gridCellArray)
    {
        this.gridCellArray = gridCellArray;

        gameManager = ServiceLocator.Get<GameManager>();
        gameManager.OnActiveColumnChanged += GameManager_OnActiveColumnChanged;
    }

    private void GameManager_OnActiveColumnChanged(object sender, EventArgs e)
    {
        if (gameManager.GetActiveGridColumn() == this)
        {
            //Debug.Log(gridColumnTransform.position);
            
            // TODO: activate visual update for the column
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

    public void SetPrevGridColumn(GridColumn prevGridColumn)
    {
        this.prevGridColumn = prevGridColumn;
    }

    public void SetNextGridColumn(GridColumn nextGridColumn)
    {
        this.nextGridColumn = nextGridColumn;
    }

    public GridColumn GetPrevGridColumn()
    {
        return prevGridColumn;
    }

    public GridColumn GetNextGridColumn()
    {
        return nextGridColumn;
    }

    public GridCell[] GetGridCellsArray()
    {
        return gridCellArray;
    }

}
