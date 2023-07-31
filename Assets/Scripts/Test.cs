using UnityEngine;
using Services;

public class Test : MonoBehaviour
{

    private GridSystem gridSystem;
    private GameManager gameManager;

    private GridCell[,] gridCellArray;

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        gameManager = ServiceLocator.Get<GameManager>();

        gridCellArray = gridSystem.GetCellArray();
        //gameManager.OnNextStep += GameManager_OnNextStep;
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
                IGridObject gridObject = gridCellArray[x, y].GetGridObject();

                if (gridObject is BaseBullet)
                {
                    message = message + " " + "X";
                }
                else if (gridObject is BaseBlob)
                {
                    message = message + " " + "O";
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
