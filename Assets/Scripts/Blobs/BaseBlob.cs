using Services;
using UnityEngine;

public class BaseBlob : GridObject
{

    [SerializeField] protected Transform blobVisual;

    protected GridSystem gridSystem;
    protected BlobManager blobManager;

    protected virtual void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        blobManager = ServiceLocator.Get<BlobManager>();

        blobManager.OnSpawnNewRow += BlobManager_OnSpawnNewRow;
    }

    private void BlobManager_OnSpawnNewRow(object sender, System.EventArgs e)
    {

        //GridCell upGridCell = gridSystem.GetUpGridCell(currentGridCell);

        //if (gridSystem.IsFree(upGridCell))
        //{
        //    gridSystem.MoveObjectToGridCell(upGridCell, this);
        //}

        //if (upGridCell?.gridObject is BaseBullet)
        //{
        //    this.CollideWithBullet(upGridCell);
        //    return;
        //}
    }

    public void SetGridCell(GridCell gridCell)
    {
        currentGridCell = gridCell;
    }

    public virtual void DestroyEffect()
    {
        Debug.LogError("DestroyEffect() not implemented");
    }

    protected virtual void CollideWithBullet(GridCell bulletGridCell)
    {
        Debug.Log("CollideWithBullet");
    }

}
