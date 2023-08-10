using DG.Tweening;
using Services;
using System.Collections.Generic;
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

    public GridCell GetGridCell(Vector3 cellPosition)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (gridCellArray[x, y].GetPosition() == cellPosition)
                    return gridCellArray[x, y];
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
                    count++;
                    continue;
                }

                if (gridObject is BaseBullet)
                {
                    break;
                }

                if (count == 0)
                {
                    continue;
                }

                BaseBlob blob = gridCellArray[x, y].gridObject as BaseBlob;
                gridCellArray[x, y].gridObject = null;
                gridCellArray[x, y - count].gridObject = blob;

                blob.gameObject.transform.position = gridCellArray[x, y - count].GetPosition();
                blob.gameObject.transform.rotation = gridCellArray[x, y - count].GetRotation(); // Maybe useless

                y -= count;
                count = 0;
            }
        }
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

    public bool TrySetObjectToGridCell(GridCell gridCellToSet, GridObject gridObjectToSet)
    {
        if (gridCellToSet is not null && gridObjectToSet is not null)
        {
            // Update old grid cell
            GridCell gridObjectGridCell = gridObjectToSet.GetCurrentGridCell();
            int c = gridObjectGridCell.X;
            int r = gridObjectGridCell.Y;
            gridCellArray[c, r].gridObject = null;

            gridObjectToSet.SetCurrentGridCell(null);

            // Update new grid cell
            c = gridCellToSet.X;
            r = gridCellToSet.Y;
            gridCellArray[c, r].gridObject = gridObjectToSet;

            gridObjectToSet.SetCurrentGridCell(gridCellToSet);

            // Set parent
            Transform gridCellTransform = gridCellToSet.GetTransform();
            gridObjectToSet.transform.parent = gridCellTransform;

            return true;
        }

        if (gridCellToSet is null)
            Debug.LogError("TrySetObjectToGridCell: gridCell is null");

        if (gridObjectToSet is null)
            Debug.LogError("TrySetObjectToGridCell: gridObject is null");

        return false;
    }

    // Ma a cosa mi serve se posso eseguire questo controllo direttamente nel codice in cui mi serve
    public bool IsFree(GridCell gridCellToCheck)
    {
        if (gridCellToCheck is not null)
        {
            //Debug.Log("is not null");
            //Debug.Log(gridCellToCheck.gridObject == null);
            return gridCellToCheck.gridObject == null;
        }

        Debug.Log("is null");
        return false;
    }

    public void MoveObjectToGridCell(GridCell gridCellToSet, GridObject gridObjectToSet)
    {
        if (gridCellToSet == null || gridObjectToSet == null)
        {
            Debug.LogWarning("gridCellToSet or gridObjectToSet: NULL");
            return;
        }


        GridCell gridObjectGridCell = gridObjectToSet.GetCurrentGridCell();
        gridObjectGridCell.gridObject = null;
        gridCellToSet.gridObject = gridObjectToSet;

        MoveGridObject(gridObjectToSet, gridCellToSet);
    }

    private void MoveGridObject(GridObject gridObjectToMove, GridCell gridCellToMoveOn)
    {
        Debug.Log(gridCellToMoveOn.GetPosition());
        gridObjectToMove.transform.position = gridCellToMoveOn.GetPosition();
        gridObjectToMove.transform.rotation = gridCellToMoveOn.GetRotation();
    }

    public void RemoveObjectFromGridCell(GridObject gridObject)
    {
        GridCell gridObjectGridCell = gridObject.GetCurrentGridCell();
        int c = gridObjectGridCell.X;
        int r = gridObjectGridCell.Y;
        gridCellArray[c, r].gridObject = null;

        //CompactGrid();
    }

}
