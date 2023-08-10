using Services;
using System;
using UnityEngine;

public class BulletManager : MonoRegistrable
{

    public event EventHandler<BaseBullet> OnBulletSpawned;

    [SerializeField] BulletListSO bulletListSO;

    private GridSystem gridSystem;
    private GameManager gameManager;

    private bool isBusy = false;

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
        if (Input.GetKeyDown(KeyCode.Space) && !isBusy)
        {
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Transform activeColumnTransform = gameManager.GetActiveColumnTransform();
        GridCell activeGridCell = gridSystem.GetGridCell(activeColumnTransform);

        GridCell[,] gridCellArray = gridSystem.GetCellArray();

        int randomIndex = UnityEngine.Random.Range(0, bulletListSO.bulletList.Count);
        BaseBullet bulletToSpawn = bulletListSO.bulletList[randomIndex];
        BaseBullet bullet = Instantiate(bulletToSpawn, activeGridCell.GetTransform());

        int column = activeGridCell.X;
        int row = gridSystem.Height - 1;
        GridCell gridCell = gridCellArray[column, row];

        gridCell.gridObject = bullet;
        bullet.SetGridCell(gridCell);

        // DoTween
        //bullet.TriggerSpawnAnimation();

        bullet.OnCollisionStart += Bullet_OnCollisionStart;
        bullet.OnCollisionEnd += Bullet_OnCollisionEnd;
        isBusy = true;

        OnBulletSpawned?.Invoke(this, bullet);
    }

    private void Bullet_OnCollisionStart(object sender, System.EventArgs e)
    {
        ((BaseBullet)sender).OnCollisionStart -= Bullet_OnCollisionStart;
    }

    private void Bullet_OnCollisionEnd(object sender, System.EventArgs e)
    {
        isBusy = false;
        ((BaseBullet)sender).OnCollisionEnd -= Bullet_OnCollisionEnd;
    }
}
