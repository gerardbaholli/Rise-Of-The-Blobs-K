using DG.Tweening.Core.Easing;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlob : MonoBehaviour, IGridObject
{

    [SerializeField] protected Transform blobVisual;

    [SerializeField] protected GridCell currentGridCell;

    protected GridSystem gridSystem;

    protected virtual void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
    }

    public void SetGridCell(GridCell gridCell)
    {
        currentGridCell = gridCell;
    }

    public virtual void DestroyBlob()
    {
        Debug.LogError("DestroyEffect() not implemented");
    }

}
