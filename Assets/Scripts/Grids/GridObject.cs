using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected GridCell currentGridCell;

    public void SetCurrentGridCell(GridCell gridCell)
    {
        currentGridCell = gridCell;
    }

    public GridCell GetCurrentGridCell()
    {
        return currentGridCell;
    }

}
