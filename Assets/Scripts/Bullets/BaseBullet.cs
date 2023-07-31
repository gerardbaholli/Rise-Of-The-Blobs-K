using Services;
using UnityEngine;
using DG.Tweening;
using System;

public class BaseBullet : MonoBehaviour, IGridObject
{
    [SerializeField] protected Transform bulletVisual;

    [SerializeField] private int stepsRequiredToMoveDown = 1;
    private int stepCounter = 0;

    protected GridCell currentGridCell;

    protected GridSystem gridSystem;
    protected GameManager gameManager;

    protected virtual void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        gameManager = ServiceLocator.Get<GameManager>();

        gameManager.OnNextStep += GameManager_OnNextStep;
        gameManager.OnActiveColumnChanged += GameManager_OnActiveColumnChanged;
    }

    public void TriggerSpawnAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 1f);
    }


    protected void GameManager_OnActiveColumnChanged(object sender, EventArgs e)
    {
        Transform activeColumn = gameManager.GetActiveColumnTransform();
        Vector3 newPosition = new Vector3(activeColumn.position.x, transform.position.y, activeColumn.position.z);
        GridCell newGridCell = gridSystem.GetGridCell(newPosition);

        GridCell[,] gridCellArray = gridSystem.GetCellArray();
        int column = newGridCell.GetColumn();
        int row = newGridCell.GetRow();

        currentGridCell.SetGridObject(null);
        gridCellArray[column, row].SetGridObject(this);
        currentGridCell = gridCellArray[column, row];

        Transform newTransform = newGridCell.GetCellTransform();
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }

    protected void GameManager_OnNextStep(object sender, EventArgs e)
    {
        if (stepCounter < stepsRequiredToMoveDown)
        {
            stepCounter++;
        }
        else
        {
            GridCell downGridCell = gridSystem.GetDownGridCell(currentGridCell);


            if (downGridCell != null)
            {
                currentGridCell.SetGridObject(null);
                downGridCell.SetGridObject(this);
                currentGridCell = downGridCell;

                Transform newTransform = downGridCell.GetCellTransform();
                transform.position = newTransform.position;
                transform.rotation = newTransform.rotation;
            }


            //if (nextGridCell.GetGridObject() is BaseBlob)
            //{
            //    Collision(nextGridCell.GetGridObject() as BaseBlob);
            //}
            //else
            //{
            //    Transform newTransform = nextGridCell.GetGridCellTransform();

            //    //transform.DOMove(newTransform.position, gameManager.GetStepValue());
            //    transform.position = newTransform.position;
            //    transform.rotation = newTransform.rotation;

            //    Debug.Log("1 " + currentGridCell);
            //    currentGridCell.SetGridObject(null);
            //    gridCellArray[nextIndex].SetGridObject(this);
            //    currentGridCell = nextGridCell;
            //    Debug.Log("2 " + currentGridCell);
            //}
            stepCounter = 0;
        }
    }

    public void SetGridCell(GridCell gridCell)
    {
        currentGridCell = gridCell;
    }

    protected virtual void Collision(BaseBlob collidedBlob)
    {
        Debug.LogError("Not implemented");
    }

}
