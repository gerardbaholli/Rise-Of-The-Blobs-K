using UnityEngine;
using Services;

public class Test : MonoBehaviour
{

    private GridSystem gridSystem;
    private GameManager gameManager;

    private GridColumn[] gridColumnArray;

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        gameManager = ServiceLocator.Get<GameManager>();

        gridColumnArray = gridSystem.GetGridColumnArray();
        gameManager.OnNextStep += GameManager_OnNextStep;
    }

    private void GameManager_OnNextStep(object sender, System.EventArgs e)
    {
        string message = "";

        for (int y = 13 - 1; y >= 0; y--)
        {
            for (int x = 0; x < 20; x++)
            {
                GridCell[] gridCellArray = gridColumnArray[x].GetGridCellsArray();
                IGridObject gridObject = gridCellArray[y].GetGridObject();

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
