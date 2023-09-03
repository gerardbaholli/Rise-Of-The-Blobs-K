using System;
using UnityEngine;

public class ColoredBullet : BaseBullet
{
    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;
    [SerializeField] private ColoredBlob coloredBlobPrefab;

    private Material bulletColorMaterial;

    protected override void Start()
    {
        base.Start();
        int randomIndex = UnityEngine.Random.Range(0, coloredMaterialListSO.coloredMaterialList.Count);
        bulletColorMaterial = coloredMaterialListSO.coloredMaterialList[randomIndex];
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


            if (coloredBlobMaterial.mainTexture == bulletColorMaterial.mainTexture)
            {
                Debug.Log("Right Collision");

                // Unsub from services
                //gameManager.OnNextStep -= GameManager_OnNextStep;
                //gameManager.OnActiveColumnChanged -= GameManager_OnActiveColumnChanged;

                // Remove from grid
                //gridSystem.RemoveGridObjectFromGridCell(this);
                //gridSystem.RemoveObjectFromGrid(coloredBlob);

                // Remove visual
                gameObject.SetActive(false);
                //coloredBlob.gameObject.SetActive(false);

                collidedColoredBlob.DestroyBlob();
            }
            else
            {
                Debug.Log("Wrong Collision");

                // Unsub from services
                //gameManager.OnNextStep -= GameManager_OnNextStep;
                //gameManager.OnActiveColumnChanged -= GameManager_OnActiveColumnChanged;

                // Add new blob to grid
                //ColoredBlob newColoredBlob = Instantiate(coloredBlobPrefab, currentGridCell.GetTransform());
                //gridSystem.AddGridObjectToGridCell(newColoredBlob, currentGridCell);
                //newColoredBlob.SetColorMaterial(bulletColorMaterial);
            }
        }

        CollisionEnd();

        DestroyBullet();
    }

    //public override void CollisionEffect(BaseBlob collidedBlob)
    //{
    //    CollisionStart();

    //    if (collidedBlob is ColoredBlob)
    //    {

    //        ColoredBlob collidedColoredBlob = (ColoredBlob) collidedBlob;
    //        Material coloredBlobMaterial = collidedColoredBlob.GetColorMaterial();


    //        if (coloredBlobMaterial.mainTexture == bulletColorMaterial.mainTexture)
    //        {
    //            Debug.Log("Right Collision");

    //            // Unsub from services
    //            gameManager.OnNextStep -= GameManager_OnNextStep;
    //            gameManager.OnActiveColumnChanged -= GameManager_OnActiveColumnChanged;

    //            // Remove from grid
    //            gridSystem.RemoveGridObjectFromGridCell(this);
    //            //gridSystem.RemoveObjectFromGrid(coloredBlob);

    //            // Remove visual
    //            gameObject.SetActive(false);
    //            //coloredBlob.gameObject.SetActive(false);

    //            collidedColoredBlob.DestroyBlob();
    //        }
    //        else
    //        {
    //            Debug.Log("Wrong Collision");

    //            // Unsub from services
    //            gameManager.OnNextStep -= GameManager_OnNextStep;
    //            gameManager.OnActiveColumnChanged -= GameManager_OnActiveColumnChanged;

    //            // Remove bullet from grid
    //            gridSystem.RemoveGridObjectFromGridCell(this);

    //            // Add new blob to grid
    //            ColoredBlob newColoredBlob = Instantiate(coloredBlobPrefab, currentGridCell.GetTransform());
    //            gridSystem.AddGridObjectToGridCell(newColoredBlob, currentGridCell);
    //            newColoredBlob.SetColorMaterial(bulletColorMaterial);
    //        }
    //    }

    //    CollisionEnd();

    //    Destroy(gameObject);
    //}


}
