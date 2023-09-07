using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;
    [SerializeField] private ColoredBullet coloredBulletPrefab;
    [SerializeField] private ColoredBlob coloredBlobPrefab;

    [SerializeField] private TestBlob testBlobPrefab;
    [SerializeField] private TestBullet testBulletPrefab;

    [SerializeField] Material testMaterial;

    private void Start()
    {
        ColoredBullet coloredBullet = Instantiate(coloredBulletPrefab);
        ColoredBlob coloredBlob = Instantiate(coloredBlobPrefab);
        TestBlob testBlob = Instantiate(testBlobPrefab);
        TestBullet testBullet = Instantiate(testBulletPrefab);


        //coloredBullet.SetColorMaterial(testMaterial);
        //coloredBlob.SetColorMaterial(testMaterial);

        //coloredBullet.SetColorMaterial(coloredMaterialListSO.coloredMaterialList[0]);
        //coloredBlob.SetColorMaterial(coloredMaterialListSO.coloredMaterialList[0]);

        coloredBullet.SetColorMaterial(coloredMaterialListSO.coloredMaterialList[2]);
        coloredBullet.SetColorMaterial(coloredMaterialListSO.coloredMaterialList[1]);
        testMaterial = coloredBullet.GetColorMaterial();
        coloredBlob.SetColorMaterial(coloredMaterialListSO.coloredMaterialList[2]);
        coloredBlob.SetColorMaterial(testMaterial);

        //Debug.Log(testBlob.GetComponent<MeshRenderer>().sharedMaterial == testBullet.GetComponent<MeshRenderer>().sharedMaterial);
        //Debug.Log(testBlob.GetComponent<MeshRenderer>().sharedMaterial + " " + testBullet.GetComponent<MeshRenderer>().sharedMaterial);

        Debug.Log(coloredBullet.GetColorMaterial() + " " + coloredBlob.GetColorMaterial());
        Debug.Log(coloredBullet.GetColorMaterial() == coloredBlob.GetColorMaterial());

    }

}
