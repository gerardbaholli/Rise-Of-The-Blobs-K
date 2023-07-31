using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGridSystem : MonoRegistrable
{
    [SerializeField] public int Width { get; private set; } = 20;
    [SerializeField] public int Height { get; private set; } = 13;

    private float range = 4f;

    private GridCell[,] gridCellArray;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        InitGridSystem();
    }

    private void InitGridSystem()
    {
        gridCellArray = new GridCell[Width, Height];

        float angleOffset = 360f / Width;

        for (int x = 0; x < Width; x++)
        {
            float angle = x * angleOffset;
            Vector3 columnPosition = ComputePosition(angle);
            Vector3 columnVersor = ComputeVersor(columnPosition);

            GameObject column = new GameObject("Column " + x);
            column.transform.position = columnPosition;
            column.transform.rotation = Quaternion.LookRotation(columnVersor);

            Transform columnTransform = column.transform;
            columnTransform.parent = transform;

            for (int y = 0; y < Height; y++)
            {
                Vector3 cellPosition = new Vector3(columnTransform.position.x, y, columnTransform.position.z);
                Quaternion cellRotation = columnTransform.rotation;

                GameObject cell = new GameObject("Cell " + y);
                cell.transform.position = cellPosition;
                cell.transform.rotation = cellRotation;

                Transform cellTransform = cell.transform;
                cellTransform.parent = columnTransform.transform;

                gridCellArray[x, y] = new GridCell(cellTransform, x, y);
            }
        }
    }

    private Vector3 ComputePosition(float angle)
    {
        float radAngle = Mathf.Deg2Rad * angle;
        float x = Vector3.zero.x + range * Mathf.Cos(radAngle);
        float z = Vector3.zero.z + range * Mathf.Sin(radAngle);
        return new Vector3(x, Vector3.zero.y, z);
    }

    private Vector3 ComputeVersor(Vector3 position)
    {
        return (position - Vector3.zero).normalized;
    }

}
