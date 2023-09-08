using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBlob : BaseBlob
{
    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;

    private bool hasToBeDestroyed = false;

    protected override void Start()
    {
        base.Start();
    }

    public override void StartingEffect()
    {
        SetRandomColorMaterial();
    }

    public Material GetColorMaterial()
    {
        return blobVisual.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void SetColorMaterial(Material blobColorMaterial)
    {
        blobVisual.GetComponent<MeshRenderer>().sharedMaterial = blobColorMaterial;
    }

    public void SetRandomColorMaterial()
    {
        int randomIndex = Random.Range(0, coloredMaterialListSO.coloredMaterialList.Count);
        SetColorMaterial(coloredMaterialListSO.coloredMaterialList[randomIndex]);
    }

    public override void DestroyEffect()
    {
        if (!hasToBeDestroyed)
        {
            hasToBeDestroyed = true;

            GridCell upGridCell = gridSystem.GetUpGridCell(currentGridCell);
            GridCell downGridCell = gridSystem.GetDownGridCell(currentGridCell);
            GridCell leftGridCell = gridSystem.GetLeftGridCell(currentGridCell);
            GridCell rightGridCell = gridSystem.GetRightGridCell(currentGridCell);

            CheckColoredBlob(upGridCell);
            CheckColoredBlob(downGridCell);
            CheckColoredBlob(leftGridCell);
            CheckColoredBlob(rightGridCell);

            gridSystem.RemoveGridObjectFromGridCell(this);
            currentGridCell = null;
            Destroy(gameObject);
        }
    }

    private void CheckColoredBlob(GridCell gridCell)
    {
        if (gridCell == null)
            return;

        if (gridCell.gridObject == null)
            return;

        if (gridCell.gridObject is ColoredBlob)
        {
            ColoredBlob coloredBlob = gridCell.gridObject as ColoredBlob;

            if (GetColorMaterial() == coloredBlob.GetColorMaterial())
            {
                coloredBlob.DestroyEffect();
            }
        }
    }

    public override string ToString()
    {
        return $"x: {currentGridCell.X} @ y: {currentGridCell.Y} @ color: {GetColorMaterial().name}";
    }

}
