using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBlob : BaseBlob
{
    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;

    private Material blobColorMaterial;

    private bool hasToBeDestroyed = false;

    protected override void Start()
    {
        base.Start();
        int randomIndex = Random.Range(0, coloredMaterialListSO.coloredMaterialList.Count);
        SetColorMaterial(coloredMaterialListSO.coloredMaterialList[randomIndex]);
    }

    public Material GetColorMaterial()
    {
        return blobVisual.GetComponent<MeshRenderer>().material;
    }

    public void SetColorMaterial(Material blobColorMaterial)
    {
        blobVisual.GetComponent<MeshRenderer>().material = blobColorMaterial;
        //Debug.Log(GetColorMaterial().ToString());
    }

    public string PrintColor()
    {
        if (GetColorMaterial() == coloredMaterialListSO.coloredMaterialList[2])
        {
            return "G";
        }
        else if (GetColorMaterial() == coloredMaterialListSO.coloredMaterialList[1])
        {
            return "O";
        }
        else if (GetColorMaterial() == coloredMaterialListSO.coloredMaterialList[0])
        {
            return "Y";
        }

        return "-";
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

            gridSystem.RemoveGridObjectFromGridCell(this);
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
