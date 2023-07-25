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

    private GridColumn currentActiveColumn;

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
        //virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
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

            GridColumn[] gridColumnArray = gridSystem.GetGridColumnArray();

            // Update
            float[] dotArray = new float[gridColumnArray.Length];

            for (int i = 0; i < gridColumnArray.Length; i++)
            {
                Transform gridColumnTransform = gridColumnArray[i].GetGridColumnTransform();
                Transform virtualCameraTransform = virtualCamera.transform;
                dotArray[i] = Vector3.Dot(virtualCameraTransform.forward, gridColumnTransform.forward);
            }

            float activeDot = 1f;
            GridColumn tempGridColumn = null;

            for (int i = 0; i < gridColumnArray.Length; i++)
            {
                if (dotArray[i] < activeDot)
                {
                    activeDot = dotArray[i];
                    tempGridColumn = gridColumnArray[i];
                }
            }

            if (currentActiveColumn != tempGridColumn)
            {
                currentActiveColumn = tempGridColumn;
                OnActiveColumnChanged?.Invoke(this, new EventArgs());
            }
            
        }
    }

    public GridColumn GetActiveGridColumn()
    {
        return currentActiveColumn;
    }

    public float GetStepValue()
    {
        return stepTimerMax;
    }

}
