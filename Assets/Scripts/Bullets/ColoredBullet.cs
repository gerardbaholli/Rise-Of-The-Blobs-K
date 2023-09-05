using System;
using UnityEngine;

public class ColoredBullet : BaseBullet
{
    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;
    [SerializeField] private ColoredBlob coloredBlobPrefab;

    protected override void Start()
    {
        base.Start();
        int randomIndex = UnityEngine.Random.Range(0, coloredMaterialListSO.coloredMaterialList.Count);
        Material bulletColorMaterial = coloredMaterialListSO.coloredMaterialList[randomIndex];
        SetColorMaterial(bulletColorMaterial);
    }

    private void SetColorMaterial(Material colorMaterial)
    {
        bulletVisual.GetComponent<MeshRenderer>().material = colorMaterial;
    }

    private Material GetColorMaterial()
    {
        return bulletVisual.GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BaseBlob collidedBlob = collision.gameObject.GetComponent<BaseBlob>();

        if (collidedBlob is ColoredBlob)
        {
            ColoredBlob collidedColoredBlob = (ColoredBlob)collidedBlob;
            Material coloredBlobMaterial = collidedColoredBlob.GetColorMaterial();
            Material bulletColorMaterial = GetColorMaterial();

            if (coloredBlobMaterial.mainTexture == bulletColorMaterial.mainTexture)
            {
                // Remove from grid
                gridSystem.RemoveGridObjectFromGridCell(collidedBlob);

                collidedColoredBlob.DestroyEffect();
            }
            else
            {
                // Add new blob to grid
                GridCell lowerFreeGridCell = gridSystem.GetLowerFreeGridCell(transform.position);
                ColoredBlob newColoredBlob = Instantiate(coloredBlobPrefab, lowerFreeGridCell.GetTransform());

                gridSystem.AddGridObjectToGridCell(newColoredBlob, lowerFreeGridCell);
                newColoredBlob.SetColorMaterial(bulletColorMaterial);
                newColoredBlob.GetColorMaterial().mainTexture = bulletColorMaterial.mainTexture;
            }
        }

        CollisionEnd();
        DestroyBullet();
    }

}
