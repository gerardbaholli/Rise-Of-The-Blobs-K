using Services;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoRegistrable
{

    [SerializeField] BulletListSO bulletListSO;
    [SerializeField] Transform bulletSpawnTransform;

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
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        GridColumn activeGridColumn = gameManager.GetActiveGridColumn();
        GridCell[] gridCellsArray = activeGridColumn.GetGridCellsArray();
        int index = gridCellsArray.Length - 1;

        List<BaseBullet> bulletList = bulletListSO.bulletList;
        int randomIndex = Random.Range(0, bulletList.Count);
        BaseBullet bulletToSpawn = bulletList[randomIndex];
        BaseBullet newBullet = Instantiate(bulletToSpawn, gridCellsArray[index].GetGridCellTransform());
        
        newBullet.TriggerSpawnAnimation();

        gridCellsArray[index].SetGridObject(newBullet);
        newBullet.SetGridCell(gridCellsArray[index]);
    }

}
