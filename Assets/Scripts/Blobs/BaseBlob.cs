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
    }

    public virtual void StartingEffect()
    {
        Debug.LogError("BaseBlob --> StartingEffect not implemented!");
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
