using UnityEngine;
using Services;

public class Test : MonoBehaviour
{
    [SerializeField] private ColoredMaterialListSO coloredMaterialListSO;

    private GridSystem gridSystem;
    private GameManager gameManager;

    private GridCell[,] gridCellArray;

    private void Start()
    {
        gridSystem = ServiceLocator.Get<GridSystem>();
        gameManager = ServiceLocator.Get<GameManager>();

        gridCellArray = gridSystem.GetCellArray();
        gameManager.OnNextStep += GameManager_OnNextStep;
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

                if (gridObject is ColoredBlob)
                {
                    if ((gridObject as ColoredBlob).GetColorMaterial() == coloredMaterialListSO.coloredMaterialList[0])
                        message = message + " " + "Y";
                    else if ((gridObject as ColoredBlob).GetColorMaterial() == coloredMaterialListSO.coloredMaterialList[1])
                        message = message + " " + "O";
                    else if ((gridObject as ColoredBlob).GetColorMaterial() == coloredMaterialListSO.coloredMaterialList[2])
                        message = message + " " + "G";
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