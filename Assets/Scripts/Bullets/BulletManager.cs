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
        Transform activeColumnTransform = gameManager.GetActiveColumnTransform();
        GridCell activeGridCell = gridSystem.GetGridCell(activeColumnTransform);

        GridCell[,] gridCellArray = gridSystem.GetCellArray();
        
        int randomIndex = Random.Range(0, bulletListSO.bulletList.Count);
        BaseBullet bulletToSpawn = bulletListSO.bulletList[randomIndex];
        BaseBullet bullet = Instantiate(bulletToSpawn, activeGridCell.GetCellTransform());

        int column = activeGridCell.GetColumn();
        int row = gridSystem.Height - 1;
        GridCell gridCell = gridCellArray[column, row];

        gridCell.SetGridObject(bullet);
        bullet.SetGridCell(gridCell);

        // DoTween
        bullet.TriggerSpawnAnimation();
    }

}
