using DG.Tweening;
using Services;
using System.Collections.Generic;
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

    public void SpawnFirstRow(List<BaseBlob> blobList)
    {
        for (int x = 0; x < Width; x++)
        {
            BaseBlob blob = Instantiate(GenerateRandomBlob(blobList), gridCellArray[x, 0].GetCellTransform());

            // DoTween
            Transform blobTransform = blob.transform;
            blobTransform.localScale = Vector3.zero;
            blobTransform.DOScale(1f, 1f);

            gridCellArray[x, 0].SetGridObject(blob);
        }
    }

    public void SpawnNewRow(List<BaseBlob> blobList)
    {
        MoveAllRowsUp();

        for (int x = 0; x < Width; x++)
        {
            BaseBlob blob = Instantiate(GenerateRandomBlob(blobList), gridCellArray[x, 0].GetCellTransform());

            // DoTween
            Transform blobTransform = blob.transform;
            blobTransform.localScale = Vector3.zero;
            blobTransform.DOScale(1f, 1f);

            gridCellArray[x, 0].SetGridObject(blob);
        }
    }

    private BaseBlob GenerateRandomBlob(List<BaseBlob> blobList)
    {
        int randomIndex = Random.Range(0, blobList.Count);
        BaseBlob blobGenerated = blobList[randomIndex];
        return blobGenerated;
    }

    private void MoveAllRowsUp()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = Height - 1; y > 0; y--)
            {
                if (gridCellArray[x, y - 1].GetGridObject() == null ||
                    gridCellArray[x, y].GetGridObject() is BaseBullet ||
                    gridCellArray[x, y - 1].GetGridObject() is BaseBullet)
                    continue;

                BaseBlob downBlob = gridCellArray[x, y - 1].GetGridObject() as BaseBlob;
                downBlob.transform.parent = gridCellArray[x, y].GetCellTransform();

                gridCellArray[x, y - 1].SetGridObject(null);
                gridCellArray[x, y].SetGridObject(downBlob);

                downBlob.SetGridCell(gridCellArray[x, y]);

                if (gridCellArray[x, y].GetGridObject() != null)
                {
                    Vector3 endPosition = gridCellArray[x, y].GetCellTransform().position;

                    // DoTween
                    downBlob.gameObject.transform.DOMove(endPosition, 1f).SetEase(Ease.InOutCirc);
                }
            }
        }
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
                if (gridCellArray[x, y].GetCellTransform() == cellTransform)
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
                if (gridCellArray[x, y].GetCellTransform().position == cellPosition)
                    return gridCellArray[x, y];
            }
        }

        return null;
    }

    //public void CompactGrid()
    //{
    //    for (int x = 0; x < width; x++)
    //    {
    //        int count = 0;

    //        for (int y = 0; y < height; y++)
    //        {
    //            IGridObject gridObject = gridCellArray[x, y].GetGridObject();

    //            if (gridObject == null)
    //            {
    //                count++;
    //                continue;
    //            }

    //            if (gridObject is BaseBullet)
    //            {
    //                break;
    //            }

    //            if (count == 0)
    //            {
    //                continue;
    //            }

    //            BaseBlob blob = gridCellArray[x, y].GetGridObject() as BaseBlob;
    //            gridCellArray[x, y - count].SetGridObject(blob);
    //            gridCellArray[x, y].SetGridObject(null);

    //            Vector3 movePosition = gridCellArray[x, y - count].GetCellTransform().position;
    //            blob.gameObject.transform.DOMove(movePosition, 1f).SetEase(Ease.InOutCirc); //.OnComplete(() => isLocked = false);

    //            y -= count;
    //            count = 0;
    //        }
    //    }
    //}

    public GridCell GetUpGridCell(GridCell gridCell)
    {
        if (gridCell != null)
        {
            int cellColumn = gridCell.GetColumn();
            int cellRow = gridCell.GetRow();

            if (cellRow + 1 < Height)
                return gridCellArray[cellColumn, cellRow + 1];
        }

        return null;
    }

    public GridCell GetDownGridCell(GridCell gridCell)
    {
        if (gridCell != null)
        {
            int cellColumn = gridCell.GetColumn();
            int cellRow = gridCell.GetRow();

            if (cellRow - 1 > 0)
                return gridCellArray[cellColumn, cellRow - 1];
        }

        return null;
    }

    public GridCell GetLeftGridCell(GridCell gridCell)
    {
        if (gridCell != null)
        {
            int cellColumn = gridCell.GetColumn();
            int cellRow = gridCell.GetRow();

            if (cellColumn - 1 < 0)
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
            int cellColumn = gridCell.GetColumn();
            int cellRow = gridCell.GetRow();

            if (cellColumn + 1 < Width)
                return gridCellArray[cellColumn + 1, cellRow];
            else
                return gridCellArray[0, cellRow];
        }

        return null;
    }

    // TODO: 
    public void SetObjectToGridCell(IGridObject gridObject)
    {
        // Put object to grid cell + set new parent
    }

}
