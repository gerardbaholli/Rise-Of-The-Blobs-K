using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class ColoredBullet : BaseBullet
{
    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;
    [SerializeField] private ColoredBlob coloredBlobPrefab;

    private Material bulletColorMaterial;

    public ColoredBullet(Material bulletColorMaterial)
    {
        this.bulletColorMaterial = bulletColorMaterial;
        bulletVisual.GetComponent<MeshRenderer>().material = bulletColorMaterial;
    }

    protected override void Start()
    {
        base.Start();
        int randomIndex = Random.Range(0, coloredMaterialListSO.coloredMaterialList.Count);
        bulletColorMaterial = coloredMaterialListSO.coloredMaterialList[randomIndex];
        bulletVisual.GetComponent<MeshRenderer>().material = bulletColorMaterial;
    }

    protected override void Collision(BaseBlob collidedBlob)
    {
        if (collidedBlob is ColoredBlob)
        {
            ColoredBlob coloredBlob = (ColoredBlob)collidedBlob;
            Material coloredBlobMaterial = coloredBlob.GetColorMaterial();

            if (coloredBlobMaterial == bulletColorMaterial)
            {
                gameManager.OnNextStep -= GameManager_OnNextStep;
                gameManager.OnActiveColumnChanged -= GameManager_OnActiveColumnChanged;

                currentGridCell.SetGridObject(null);

                collidedBlob.DestroyBlob();

                gridSystem.CompactGrid();
                Destroy(gameObject);
            }
            else
            {
                // TODO: Big BUG here

                //Debug.Log("2 " + bulletColorMaterial.name);
                //ColoredBlob coloredBlobToSpawn = coloredBlobPrefab;
                //coloredBlobToSpawn.SetColorMaterial(bulletColorMaterial);

                //ColoredBlob newBlob = Instantiate(coloredBlobToSpawn, currentGridCell.GetGridCellTransform());

                //newBlob.transform.localScale = Vector3.zero;
                //newBlob.transform.DOScale(1f, 1f);
                //currentGridCell.SetGridObject(newBlob);
                //Destroy(gameObject);
            }
        }
    }

}
