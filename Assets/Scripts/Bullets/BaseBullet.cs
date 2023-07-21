using Services;
using UnityEngine;
using DG.Tweening;
using System;

public class BaseBullet : MonoBehaviour, IGridObject
{
    private GridCell currentGridCell;

    private GridSystem gridSystem;
    private GameManager gameManager;

    private void Start()
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
    public void TriggerDestroyAnimation()
    {
        float scaleDuration = 5f;
        float fadeDuration = 2f;
        float finalAlpha = 0f;

        transform.DOScale(new Vector3(2f, -1f, 2f), scaleDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    Debug.Log("DOScale completed");
                    Renderer objectRenderer = GetComponentInChildren<Renderer>();
                    Color currentColor = objectRenderer.material.color;
                    Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, finalAlpha);
                    objectRenderer.material.DOColor(targetColor, fadeDuration)
                    .OnComplete(() =>
                    {
                        Debug.Log("DOColor completed");
                        //Destroy(gameObject);
                    });
                });
    }

    private void GameManager_OnActiveColumnChanged(object sender, System.EventArgs e)
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

    private void GameManager_OnNextStep(object sender, System.EventArgs e)
    {
        GridColumn gridColumn = currentGridCell.GetGridCellColumn();
        GridCell[] gridCellArray = gridColumn.GetGridCellsArray();
        int nextIndex = currentGridCell.GetCellIndex() - 1;
        GridCell nextGridCell = gridCellArray[nextIndex];

        if (nextGridCell.GetGridObject() is BaseBlob)
        {
            Debug.Log("Collision");
        }
        else
        {
            Transform newTransform = nextGridCell.GetGridCellTransform();

            transform.DOMove(newTransform.position, gameManager.GetStepValue());

            currentGridCell.SetGridObject(null);
            gridCellArray[nextIndex].SetGridObject(this);
            currentGridCell = nextGridCell;
        }
    }

    public void SetBulletGridCell(GridCell gridCell)
    {
        currentGridCell = gridCell;
    }

}
