using System;
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
        int randomIndex = UnityEngine.Random.Range(0, coloredMaterialListSO.coloredMaterialList.Count);
        bulletColorMaterial = coloredMaterialListSO.coloredMaterialList[randomIndex];
        bulletVisual.GetComponent<MeshRenderer>().material = bulletColorMaterial;
    }

    public override void CollisionEffect(BaseBlob collidedBlob)
    {
        CollisionStart();

        if (collidedBlob is ColoredBlob)
        {
            ColoredBlob coloredBlob = (ColoredBlob)collidedBlob;
            Material coloredBlobMaterial = coloredBlob.GetColorMaterial();

            if (coloredBlobMaterial == bulletColorMaterial)
            {
                // Unsub from services
                gameManager.OnNextStep -= GameManager_OnNextStep;
                gameManager.OnActiveColumnChanged -= GameManager_OnActiveColumnChanged;

                // Remove from grid
                gridSystem.RemoveObjectFromGridCell(this);
                //gridSystem.RemoveObjectFromGrid(coloredBlob);

                // Remove visual
                gameObject.SetActive(false);
                //coloredBlob.gameObject.SetActive(false);


                coloredBlob.DestroyBlob();


                // TODO: destroy...
                //gridSystem.CompactGrid();
                //Destroy(gameObject);
            }
        }

        CollisionEnd();

    }

}
