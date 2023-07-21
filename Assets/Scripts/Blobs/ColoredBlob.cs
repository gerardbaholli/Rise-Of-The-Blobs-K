using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBlob : BaseBlob
{
    [SerializeField] private ColoredMaterialListSO coloredBlobMaterialListSO;

    private Material blobColorMaterial;

    public ColoredBlob(Material blobColorMaterial)
    {
        this.blobColorMaterial = blobColorMaterial;
        blobVisual.GetComponent<MeshRenderer>().material = blobColorMaterial;
    }

    private void Start()
    {
        int randomIndex = Random.Range(0, coloredBlobMaterialListSO.coloredBlobMaterialList.Count);
        blobColorMaterial = coloredBlobMaterialListSO.coloredBlobMaterialList[randomIndex];
        blobVisual.GetComponent<MeshRenderer>().material = blobColorMaterial;
    }

}
