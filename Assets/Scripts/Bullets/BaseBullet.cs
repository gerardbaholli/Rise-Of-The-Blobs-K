using Services;
using UnityEngine;
using DG.Tweening;
using System;

public class BaseBullet : GridObject
{
    public event EventHandler OnCollisionStart;
    public event EventHandler OnCollisionEnd;

    [SerializeField] protected Transform bulletVisual;

    protected int stepsRequiredToMoveDown = 1;
    private int stepCounter = 0;

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

        if (gridSystem.TrySetObjectToGridCell(newGridCell, this))
        {
            Transform newTransform = newGridCell.GetTransform();
            transform.position = newTransform.position;
            transform.rotation = newTransform.rotation;
        }
        else
        {
            Debug.LogError("GameManager_OnActiveColumnChanged failed");
        }
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

            if (downGridCell is null)
                return;

            if (downGridCell.gridObject is BaseBlob)
            {
                CollisionEffect(downGridCell.gridObject as BaseBlob);
            }
            else
            {
                if (gridSystem.TrySetObjectToGridCell(downGridCell, this))
                {
                    Transform newTransform = downGridCell.GetTransform();
                    transform.position = newTransform.position;
                    transform.rotation = newTransform.rotation;
                }
                else
                {
                    Debug.LogError("GameManager_OnNextStep failed");
                }
            }

            stepCounter = 0;

        }
    }

    public void SetGridCell(GridCell gridCell)
    {
        currentGridCell = gridCell;
    }

    public virtual void CollisionEffect(BaseBlob collidedBlob)
    {
        Debug.LogError("CollisionEffect not implemented!");
    }

    protected void CollisionStart() => OnCollisionStart?.Invoke(this, EventArgs.Empty);

    protected void CollisionEnd() => OnCollisionEnd?.Invoke(this, EventArgs.Empty);

}
