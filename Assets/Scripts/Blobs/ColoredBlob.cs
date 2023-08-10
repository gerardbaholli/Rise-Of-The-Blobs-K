using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBlob : BaseBlob
{
    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;

    private Material blobColorMaterial;

    private bool hasToBeDestroyed = false;

    public ColoredBlob(Material blobColorMaterial)
    {
        this.blobColorMaterial = blobColorMaterial;
        blobVisual.GetComponent<MeshRenderer>().material = blobColorMaterial;
    }

    protected override void Start()
    {
        base.Start();
        int randomIndex = Random.Range(0, coloredMaterialListSO.coloredMaterialList.Count);
        blobColorMaterial = coloredMaterialListSO.coloredMaterialList[randomIndex];
        blobVisual.GetComponent<MeshRenderer>().material = blobColorMaterial;
    }

    public Material GetColorMaterial()
    {
        return blobColorMaterial;
    }

    public override void DestroyBlob()
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

            gridSystem.RemoveObjectFromGridCell(this);
            currentGridCell = null;
            gameObject.SetActive(false);
        }
    }

    private void CheckColoredBlob(GridCell gridCell)
    {
        if (gridCell == null)
        {
            return;
        }

        if (gridCell.gridObject == null)
            return;

        if (gridCell.gridObject is ColoredBlob)
        {
            ColoredBlob coloredBlob = gridCell.gridObject as ColoredBlob;

            if (blobColorMaterial == coloredBlob.GetColorMaterial())
            {
                coloredBlob.DestroyBlob();
            }
        }
    }

}
