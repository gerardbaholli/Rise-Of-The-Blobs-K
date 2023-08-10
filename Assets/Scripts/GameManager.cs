using Cinemachine;
using Services;
using System;
using UnityEngine;

public class GameManager : MonoRegistrable
{
    public event EventHandler OnNextStep;
    public event EventHandler OnActiveColumnChanged;

    private GridSystem gridSystem;
    private BulletManager bulletManager;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private Transform activeColumnTransform;

    private float stepTimerMax = 0.25f;
    private float stepTimer;
    private bool isPaused = false;

    private float updateActiveColumnTimerMax = 0.1f;
    private float updateActiveColumnTimer;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        bulletManager = ServiceLocator.Get<BulletManager>();


        bulletManager.OnBulletSpawned += BulletManager_OnBulletSpawned;
        UpdateActiveColumn();
    }

    private void Update()
    {
        UpdateActiveColumn();
        UpdateStep();
    }

    private void UpdateStep()
    {
        if (!isPaused)
            stepTimer += Time.deltaTime;

        if (stepTimer > stepTimerMax)
        {
            stepTimer = 0f;

            Debug.Log("OnNextStep");
            OnNextStep?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateActiveColumn()
    {

        updateActiveColumnTimer += Time.deltaTime;
        if (updateActiveColumnTimer > updateActiveColumnTimerMax || activeColumnTransform is null)
        {
            updateActiveColumnTimer = 0f;


            GridCell[,] gridCellArray = gridSystem.GetCellArray();
            float[] dotProductArray = new float[gridSystem.Width];

            for (int i = 0; i < gridSystem.Width; i++)
            {
                Transform columnTransform = gridCellArray[i, gridSystem.Height - 1].GetTransform();
                Transform virtualCameraTransform = virtualCamera.transform;
                dotProductArray[i] = Vector3.Dot(virtualCameraTransform.forward, columnTransform.forward);
            }

            float maxDotProduct = 1f;
            Transform gridCellTransform = null;

            for (int i = 0; i < gridSystem.Width; i++)
            {
                if (dotProductArray[i] < maxDotProduct)
                {
                    maxDotProduct = dotProductArray[i];
                    gridCellTransform = gridCellArray[i, gridSystem.Height - 1].GetTransform();
                }
            }

            if (activeColumnTransform != gridCellTransform)
            {
                activeColumnTransform = gridCellTransform;
                OnActiveColumnChanged?.Invoke(this, new EventArgs());
            }

        }
    }

    private void BulletManager_OnBulletSpawned(object sender, BaseBullet bullet)
    {
        bullet.OnCollisionStart += Bullet_OnCollisionStart;
        bullet.OnCollisionEnd += Bullet_OnCollisionEnd;
    }

    private void Bullet_OnCollisionStart(object sender, EventArgs e)
    {
        isPaused = true;
    }

    private void Bullet_OnCollisionEnd(object sender, EventArgs e)
    {
        gridSystem.CompactGrid();
        isPaused = false;
    }

    public Transform GetActiveColumnTransform()
    {
        return activeColumnTransform;
    }

}
