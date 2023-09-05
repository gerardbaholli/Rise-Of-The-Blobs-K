using DG.Tweening;
using Services;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem : MonoRegistrable
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

            // Create Column GameObject
            GameObject column = new GameObject("Column " + x);
            column.transform.position = columnPosition;
            column.transform.rotation = Quaternion.LookRotation(columnVersor);

            Transform columnTransform = column.transform;
            columnTransform.parent = transform;

            for (int y = 0; y < Height; y++)
            {
                Vector3 cellPosition = new Vector3(columnTransform.position.x, y, columnTransform.position.z);
                Quaternion cellRotation = columnTransform.rotation;

                // Create Cell GameObject
                GameObject gridCellGO = new GameObject("Cell " + y);
                Transform cellTransform = gridCellGO.transform;
                cellTransform.position = cellPosition;
                cellTransform.rotation = cellRotation;
                cellTransform.parent = columnTransform.transform;


                GridCell gridCell = new GridCell(gridCellGO, x, y);
                gridCellArray[x, y] = gridCell;
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

    public GridCell[,] GetCellArray()
    {
        return gridCellArray;
    }

    public GridCell GetGridCell(Transform cellTransform)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (gridCellArray[x, y].GetTransform() == cellTransform)
                    return gridCellArray[x, y];
            }
        }

        return null;
    }

    public GridCell GetNearestGridCell(Vector3 worldPosition)
    {
        int tempX = 0;

        for (int x = 0; x < Width; x++)
        {
            //Debug.Log("x: " + Mathf.RoundToInt(gridCellArray[x, 0].GetPosition().x) + " " + Mathf.RoundToInt(worldPosition.x));
            //Debug.Log("z: " + Mathf.RoundToInt(gridCellArray[x, 0].GetPosition().z) + " " + Mathf.RoundToInt(worldPosition.z));

            if (Mathf.RoundToInt(gridCellArray[x, 0].GetPosition().x) == Mathf.RoundToInt(worldPosition.x) &&
                Mathf.RoundToInt(gridCellArray[x, 0].GetPosition().z) == Mathf.RoundToInt(worldPosition.z))
            {
                tempX = x;
                break;
            }

            //if (x == Width - 1)
            //Debug.LogWarning("NON HA TROVATO NULLA");
        }

        for (int y = 0; y < Height; y++)
        {
            if (Vector3.Distance(gridCellArray[tempX, y].GetPosition(), worldPosition) < 1.0f)
            {
                //Debug.Log("Grid Cell: " + gridCellArray[tempX, y].cellGameObject.name);
                return gridCellArray[tempX, y];
            }
        }

        return null;
    }

    public void CompactGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            int count = 0;

            for (int y = 0; y < Height; y++)
            {
                GridObject gridObject = gridCellArray[x, y].gridObject;

                if (gridObject == null)
                {
                    // Increase count if the cell is empty.
                    count++;
                    continue;
                }

                if (count > 0)
                {
                    // Se ci sono celle vuote sopra, sposta l'oggetto verso l'alto con interpolazione.
                    BaseBlob blob = gridCellArray[x, y].gridObject as BaseBlob;
                    gridCellArray[x, y].gridObject = null;
                    gridCellArray[x, y - count].gridObject = blob;

                    // blob.gameObject.transform.position = gridCellArray[x, y - count].GetPosition();
                    // blob.gameObject.transform.rotation = gridCellArray[x, y - count].GetRotation(); // Possibly unnecessary

                    Vector3 targetPosition = gridCellArray[x, y - count].GetPosition();
                    StartCoroutine(MoveObjectAsync(blob.gameObject, targetPosition));

                    y -= count;
                    count = 0;
                }

            }
        }
    }

    private IEnumerator MoveObjectAsync(GameObject obj, Vector3 targetPosition)
    {
        float journeyLength = Vector3.Distance(obj.transform.position, targetPosition);
        float startTime = Time.time;
        float distanceCovered = 0.0f;
        float movingSpeed = 0.75f;

        while (distanceCovered < journeyLength)
        {
            float fractionOfJourney = distanceCovered / journeyLength;
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, fractionOfJourney);

            distanceCovered = (Time.time - startTime) * movingSpeed;

            yield return null;
        }

        obj.transform.position = targetPosition;
    }

    public GridCell GetUpGridCell(GridCell gridCell)
    {
        if (gridCell != null)
        {
            int cellColumn = gridCell.X;
            int cellRow = gridCell.Y;

            if (cellRow + 1 < Height)
                return gridCellArray[cellColumn, cellRow + 1];
        }

        return null;
    }

    public GridCell GetDownGridCell(GridCell gridCell)
    {
        if (gridCell != null)
        {
            int cellColumn = gridCell.X;
            int cellRow = gridCell.Y;

            if (cellRow - 1 >= 0)
                return gridCellArray[cellColumn, cellRow - 1];
        }

        return null;
    }

    public GridCell GetLeftGridCell(GridCell gridCell)
    {
        if (gridCell != null)
        {
            int cellColumn = gridCell.X;
            int cellRow = gridCell.Y;

            if (cellColumn - 1 >= 0)
                return gridCellArray[cellColumn - 1, cellRow];
            else
                return gridCellArray[Width - 1, cellRow];
        }

        return null;
    }

    public GridCell GetRightGridCell(GridCell gridCell)
    {
        if (gridCell != null)
        {
            int cellColumn = gridCell.X;
            int cellRow = gridCell.Y;

            if (cellColumn + 1 < Width)
                return gridCellArray[cellColumn + 1, cellRow];
            else
                return gridCellArray[0, cellRow];
        }

        return null;
    }

    public void AddGridObjectToGridCell(GridObject gridObjectToAdd, GridCell gridCell)
    {
        int x = gridCell.X;
        int y = gridCell.Y;

        if ((x >= Width) || (y >= Height) || y < 0 || x < 0)
        {
            Debug.LogError("AddGridObjectToGridCell: wrong X and/or Y.");
            return;
        }

        GridObject gridObjectToCheck = gridCellArray[x, y].gridObject;

        if (gridObjectToCheck is null)
        {
            gridCellArray[x, y].gridObject = gridObjectToAdd;
            gridObjectToAdd.SetCurrentGridCell(gridCellArray[x, y]);
        }
        else
        {
            Debug.LogError("AddGridObjectToGridCell: gridObject is not empty.");
        }
    }

    public void RemoveGridObjectFromGridCell(GridObject gridObject)
    {
        GridCell gridObjectGridCell = gridObject.GetCurrentGridCell();

        int c = gridObjectGridCell.X;
        int r = gridObjectGridCell.Y;
        gridCellArray[c, r].gridObject = null;
    }

    private int GetColumnIndex(Vector3 worldPosition)
    {
        for (int x = 0; x < Width; x++)
        {
            if (Mathf.RoundToInt(gridCellArray[x, 0].GetPosition().x) == Mathf.RoundToInt(worldPosition.x) &&
                Mathf.RoundToInt(gridCellArray[x, 0].GetPosition().z) == Mathf.RoundToInt(worldPosition.z))
            {
                return x;
            }
        }

        return -1;
    }

    public GridCell GetLowerFreeGridCell(Vector3 worldPosition)
    {
        int gridCellColumnIndex = GetColumnIndex(worldPosition);

        for (int i = 0; i < Height; i++)
        {
            if (gridCellArray[gridCellColumnIndex, i].IsFree())
            {
                return gridCellArray[gridCellColumnIndex, i];
            }
        }

        return null;
    }

}
