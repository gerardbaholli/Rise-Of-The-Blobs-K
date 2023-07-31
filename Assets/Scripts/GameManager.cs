using Cinemachine;
using Services;
using System;
using UnityEngine;

public class GameManager : MonoRegistrable
{
    public event EventHandler OnNextStep;
    public event EventHandler OnActiveColumnChanged;

    private GridSystem gridSystem;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private Transform activeColumnTransform;

    private float stepTimerMax = 0.5f;
    private float stepTimer;

    private float updateActiveColumnTimerMax = 0.1f;
    private float updateActiveColumnTimer;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();

        UpdateActiveColumn();
    }

    private void Update()
    {
        UpdateActiveColumn();
        UpdateStep();
    }

    private void UpdateStep()
    {
        stepTimer += Time.deltaTime;
        if (stepTimer > stepTimerMax)
        {
            stepTimer = 0f;

            OnNextStep?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateActiveColumn()
    {

        updateActiveColumnTimer += Time.deltaTime;
        if (updateActiveColumnTimer > updateActiveColumnTimerMax)
        {
            updateActiveColumnTimer = 0f;


            GridCell[,] gridCellArray = gridSystem.GetCellArray();
            float[] dotProductArray = new float[gridSystem.Width];

            for (int i = 0; i < gridSystem.Width; i++)
            {
                Transform columnTransform = gridCellArray[i, gridSystem.Height - 1].GetCellTransform();
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
                    gridCellTransform = gridCellArray[i, gridSystem.Height - 1].GetCellTransform();
                }
            }

            if (activeColumnTransform != gridCellTransform)
            {
                activeColumnTransform = gridCellTransform;
                OnActiveColumnChanged?.Invoke(this, new EventArgs());
            }

        }
    }

    public Transform GetActiveColumnTransform()
    {
        return activeColumnTransform;
    }

    public float GetStepValue()
    {
        return stepTimerMax;
    }

}
