using UnityEngine;
using Services;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

public class Test : MonoBehaviour
{

    private GridSystem gridSystem;
    private GameManager gameManager;

    private GridCell[,] gridCellArray;
    [SerializeField] private ColoredMaterialListSO coloredMaterialList;

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        gameManager = ServiceLocator.Get<GameManager>();

        gridCellArray = gridSystem.GetCellArray();
        gameManager.OnNextStep += GameManager_OnNextStep;
    }

    private void FixedUpdate()
    {
        //DebugMethod();
    }

    private void GameManager_OnNextStep(object sender, System.EventArgs e)
    {
        DebugMethod();
    }

    private void DebugMethod()
    {
        string message = "";

        for (int y = 13 - 1; y >= 0; y--)
        {
            for (int x = 0; x < 20; x++)
            {
                GridObject gridObject = gridCellArray[x, y].gridObject;

                if (gridObject is BaseBullet)
                {
                    message = message + " " + "X";
                }
                else if (gridObject is BaseBlob)
                {
                    //Debug.Log(((ColoredBlob)gridObject).GetComponentInChildren<Renderer>().material.name);
                    if (gridObject is ColoredBlob)
                    {
                        if (((ColoredBlob)gridObject).GetColorMaterial() != null)
                        {

                            if (((ColoredBlob)gridObject).GetColorMaterial().Equals(coloredMaterialList.coloredMaterialList.ElementAt(0)))
                            {
                                message = message + " " + "Y";
                            }
                            else if (((ColoredBlob)gridObject).GetColorMaterial().Equals(coloredMaterialList.coloredMaterialList.ElementAt(1)))
                            {
                                message = message + " " + "O";
                            }
                            else if (((ColoredBlob)gridObject).GetColorMaterial().Equals(coloredMaterialList.coloredMaterialList.ElementAt(2)))
                            {
                                message = message + " " + "G";
                            }
                        }
                    }
                }
                else
                {
                    message = message + " " + "N";
                }

            }
            message += "\n";
        }

        Debug.Log(message);
    }


}
