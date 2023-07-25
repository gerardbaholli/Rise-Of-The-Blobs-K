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

    // TODO: fix animation
    //public void TriggerDestroyAnimation()
    //{
    //    float scaleDuration = 5f;
    //    float fadeDuration = 2f;
    //    float finalAlpha = 0f;

    //    transform.DOScale(new Vector3(2f, -1f, 2f), scaleDuration)
    //            .SetEase(Ease.OutQuad)
    //            .OnComplete(() =>
    //            {
    //                Debug.Log("DOScale completed");
    //                Renderer objectRenderer = GetComponentInChildren<Renderer>();
    //                Color currentColor = objectRenderer.material.color;
    //                Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, finalAlpha);
    //                objectRenderer.material.DOColor(targetColor, fadeDuration)
    //                .OnComplete(() =>
    //                {
    //                    Debug.Log("DOColor completed");
    //                    //Destroy(gameObject);
    //                });
    //            });
    //}

    protected void GameManager_OnActiveColumnChanged(object sender, EventArgs e)
    {
        GridColumn activeGridColumn = gameManager.GetActiveGridColumn();
        GridCell[] gridCellArray = activeGridColumn.GetGridCellsArray();

        int cellIndex = currentGridCell.GetCellIndex();

        currentGridCell.SetGridObject(null);
        gridCellArray[cellIndex].SetGridObject(this);
        currentGridCell = gridCellArray[cellIndex];

        Transform newTransform = gridCellArray[cellIndex].GetGridCellTransform();

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
            GridColumn gridColumn = currentGridCell.GetGridCellColumn();
            GridCell[] gridCellArray = gridColumn.GetGridCellsArray();
            int nextIndex = currentGridCell.GetCellIndex() - 1;
            GridCell nextGridCell = gridCellArray[nextIndex];

            if (nextGridCell.GetGridObject() is BaseBlob)
            {
                Collision(nextGridCell.GetGridObject() as BaseBlob);
            }
            else
            {
                Transform newTransform = nextGridCell.GetGridCellTransform();

                //transform.DOMove(newTransform.position, gameManager.GetStepValue());
                transform.position = newTransform.position;
                transform.rotation = newTransform.rotation;

                Debug.Log("1 " + currentGridCell);
                currentGridCell.SetGridObject(null);
                gridCellArray[nextIndex].SetGridObject(this);
                currentGridCell = nextGridCell;
                Debug.Log("2 " + currentGridCell);
            }
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
