using Services;
using System;
using UnityEngine;

public class BulletManager : MonoRegistrable
{

    public event EventHandler<BaseBullet> OnBulletSpawned;

    [SerializeField] BulletListSO bulletListSO;

    private GridSystem gridSystem;
    private GameManager gameManager;

    private bool isBulletRunning = false;
    //private bool isGamePaused = false;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        gameManager = ServiceLocator.Get<GameManager>();

        //gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    //private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    //{
    //    isGamePaused = (gameManager.GetGameState() == GameManager.GameState.Paused);
    //}

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isBulletRunning)
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
        bullet.StartingEffect();

        int column = activeGridCell.X;
        int row = gridSystem.Height - 1;
        GridCell gridCell = gridCellArray[column, row];

        bullet.OnCollisionStart += Bullet_OnCollisionStart; // ?
        bullet.OnCollisionEnd += Bullet_OnCollisionEnd;
        isBulletRunning = true;

        OnBulletSpawned?.Invoke(this, bullet);
    }

    private void Bullet_OnCollisionStart(object sender, EventArgs e) // ?
    {
        ((BaseBullet)sender).OnCollisionStart -= Bullet_OnCollisionStart;
    }

    private void Bullet_OnCollisionEnd(object sender, EventArgs e)
    {
        isBulletRunning = false;
        ((BaseBullet)sender).OnCollisionEnd -= Bullet_OnCollisionEnd;
    }
}
