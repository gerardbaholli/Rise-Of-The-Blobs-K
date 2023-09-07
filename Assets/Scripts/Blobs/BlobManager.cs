using Services;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlobManager : MonoRegistrable
{
    public event EventHandler OnSpawnNewRow;

    [SerializeField] private BlobListSO blobListSO;

    [SerializeField] private int stepsRequiredToSpawnBlobs = 6;
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

        SpawnFirstRow();
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
            SpawnNewRow();
            
            stepCounter = 0;
        }
    }

    private void SpawnFirstRow()
    {
        SpawnBlobs();
    }

    private void SpawnNewRow()
    {
        OnSpawnNewRow?.Invoke(this, EventArgs.Empty);

        MoveAllBlobsUp();

        SpawnBlobs();
    }

    private void SpawnBlobs()
    {
        GridCell[,] gridCellArray = gridSystem.GetCellArray();
        for (int x = 0; x < gridSystem.Width; x++)
        {
            Transform gridCellParent = gridCellArray[x, 0].GetTransform();
            BaseBlob blob = Instantiate(GenerateRandomBlob(blobListSO.blobList), gridCellParent);
            blob.StartingEffect();

            gridCellArray[x, 0].gridObject = blob;
            blob.SetCurrentGridCell(gridCellArray[x, 0]);
        }
    }

    private BaseBlob GenerateRandomBlob(List<BaseBlob> blobList)
    {
        int randomIndex = UnityEngine.Random.Range(0, blobList.Count);
        BaseBlob blobGenerated = blobList[randomIndex];
        return blobGenerated;
    }


    // TODO: REFACTOR THIS
    private void MoveAllBlobsUp()
    {
        GridCell[,] gridCellArray = gridSystem.GetCellArray();

        for (int x = 0; x < gridSystem.Width; x++)
        {
            for (int y = gridSystem.Height - 1; y > 0; y--)
            {
                // If GridCell is null: SKIP
                if (gridCellArray[x, y - 1].gridObject == null)
                    continue;

                BaseBlob downBlob = gridCellArray[x, y - 1].gridObject as BaseBlob;
                downBlob.transform.parent = gridCellArray[x, y].GetTransform();

                gridCellArray[x, y - 1].gridObject = null;
                gridCellArray[x, y].gridObject = downBlob;
                downBlob.SetGridCell(gridCellArray[x, y]);

                if (gridCellArray[x, y].gridObject != null)
                {
                    Vector3 endPosition = gridCellArray[x, y].GetPosition();

                    downBlob.gameObject.transform.position = gridCellArray[x, y].GetPosition();
                    downBlob.gameObject.transform.rotation = gridCellArray[x, y].GetRotation();
                }
            }
        }
    }

}
