using DG.Tweening;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoRegistrable
{

    [SerializeField] private Transform columnTransform;
    [SerializeField] private Transform cellTransform;

    [SerializeField] private int width = 20;
    [SerializeField] private int height = 13;

    private float range = 4f;

    private GridColumn[] gridColumnArray;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        InitGridSystemArray();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        GridCell[] gridCellArray = gridColumnArray[x].GetGridCellsArray();

        //        for (int y = 0; y < height; y++)
        //        {
        //            if (y < 4)
        //            {
        //                Destroy(gridCellArray[y].GetBlob().gameObject);
        //                Debug.Log(gridCellArray[y].GetBlob().gameObject);
        //            }
        //        }
        //    }
        //}

        //CompactGrid();
    }

    private void InitGridSystemArray()
    {
        gridColumnArray = new GridColumn[width];

        for (int x = 0; x < width; x++)
        {
            GridCell[] gridCellArray = new GridCell[height];

            for (int y = 0; y < height; y++)
            {
                gridCellArray[y] = new GridCell(y);
            }

            GridColumn gridColumn = new GridColumn(gridCellArray);
            gridColumnArray[x] = gridColumn;
        }

        InitColumnTransform();
        InitCellTransform();
    }

    private void InitColumnTransform()
    {
        float angleOffset = 360f / width;

        for (int i = 0; i < width; i++)
        {
            float angle = i * angleOffset;
            Vector3 position = ComputePosition(angle);
            Vector3 versor = ComputeVersor(position);

            Transform columnSpawner = Instantiate(columnTransform, position, Quaternion.LookRotation(versor));
            columnSpawner.parent = this.transform;
            gridColumnArray[i].SetGridColumnTransform(columnSpawner);
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

    private void InitCellTransform()
    {
        for (int x = 0; x < width; x++)
        {
            Transform gridColumnTransform = gridColumnArray[x].GetGridColumnTransform();
            GridCell[] gridCellArray = gridColumnArray[x].GetGridCellsArray();

            for (int y = 0; y < height; y++)
            {
                Vector3 cellPosition = new Vector3(gridColumnTransform.position.x, y, gridColumnTransform.position.z);
                Quaternion cellRotation = gridColumnTransform.rotation;

                Transform cellTransform = Instantiate(this.cellTransform, cellPosition, cellRotation);
                cellTransform.parent = gridColumnTransform.transform;
                gridCellArray[y].SetGridCellTransform(cellTransform);
                gridCellArray[y].SetGridCellColumn(gridColumnArray[x]);
            }
        }
    }

    public void SpawnNewRow(List<BaseBlob> blobList)
    {
        MoveAllRowsUp();

        for (int x = 0; x < width; x++)
        {
            GridCell[] gridCellArray = gridColumnArray[x].GetGridCellsArray();

            int randomIndex = Random.Range(0, blobList.Count);
            BaseBlob blobToSpawn = blobList[randomIndex];
            BaseBlob newBlob = Instantiate(blobToSpawn, gridCellArray[0].GetGridCellTransform());
            newBlob.transform.localScale = Vector3.zero;
            newBlob.transform.DOScale(1f, 1f);
            gridCellArray[0].SetGridObject(newBlob);
        }
    }

    private void MoveAllRowsUp()
    {
        for (int x = 0; x < width; x++)
        {
            GridCell[] gridCellArray = gridColumnArray[x].GetGridCellsArray();

            for (int y = height - 1; y > 0; y--)
            {
                if (gridCellArray[y - 1].GetGridObject() == null)
                    continue;

                BaseBlob prevBlob = gridCellArray[y - 1].GetGridObject() as BaseBlob;
                prevBlob.transform.parent = gridCellArray[y].GetGridCellTransform();

                gridCellArray[y - 1].SetGridObject(null);
                gridCellArray[y].SetGridObject(prevBlob);

                if (gridCellArray[y].GetGridObject() != null)
                {
                    Vector3 endPosition = gridCellArray[y].GetGridCellTransform().position;
                    prevBlob.gameObject.transform.DOMove(endPosition, 1f).SetEase(Ease.InOutCirc);
                }
            }
        }
    }

    public GridColumn[] GetGridColumnArray()
    {
        return gridColumnArray;
    }

    public void CompactGrid()
    {
        for (int x = 0; x < width; x++)
        {
            GridCell[] gridCellArray = gridColumnArray[x].GetGridCellsArray();
            int count = 0;

            for (int y = 0; y < height; y++)
            {
                IGridObject gridObject = gridCellArray[y].GetGridObject();

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

                BaseBlob blob = gridCellArray[y].GetGridObject() as BaseBlob;
                gridCellArray[y - count].SetGridObject(blob);
                gridCellArray[y].SetGridObject(null);

                Vector3 movePosition = gridCellArray[y - count].GetGridCellTransform().position;
                blob.gameObject.transform.DOMove(movePosition, 1f).SetEase(Ease.InOutCirc); //.OnComplete(() => isLocked = false);

                y -= count;
                count = 0;
            }
        }
    }

}