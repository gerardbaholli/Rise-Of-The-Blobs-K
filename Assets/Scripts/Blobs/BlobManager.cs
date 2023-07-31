using Services;
using UnityEngine;

public class BlobManager : MonoRegistrable
{

    [SerializeField] private BlobListSO blobListSO;

    [SerializeField] private int stepsRequiredToSpawnBlobs = 2;
    private int stepCounter = 0;

    private GridSystem gridSystem;
    private GameManager gameManager;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        gameManager = ServiceLocator.Get<GameManager>();

        gridSystem.SpawnFirstRow(blobListSO.blobList);
        gameManager.OnNextStep += GameManager_OnNextStep;
    }

    private void GameManager_OnNextStep(object sender, System.EventArgs e)
    {
        if (stepCounter < stepsRequiredToSpawnBlobs)
        {
            stepCounter++;
        }
        else
        {
            gridSystem.SpawnNewRow(blobListSO.blobList);
            stepCounter = 0;
        }
    }

}
