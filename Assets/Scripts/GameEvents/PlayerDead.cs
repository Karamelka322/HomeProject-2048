using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] private Field _field;

    private void OnEnable()
    {
        _field.ZeroEmptyCells += OnZeroEmptyCells;
    }

    private void OnDisable()
    {
        _field.ZeroEmptyCells -= OnZeroEmptyCells;        
    }

    private void OnZeroEmptyCells(Cell[,] cells)
    {
        if(!CheckMerge(cells, Vector2.up) && !CheckMerge(cells, Vector2.down) &&
            !CheckMerge(cells, Vector2.left) && !CheckMerge(cells, Vector2.right))
        {
            Debug.Log("Dead");
        }
    }

    private bool CheckMerge(Cell[,] cells, Vector2 direction)
    {
        int startPoint = direction.x > 0 || direction.y < 0 ? Field.FieldSize - 1 : 0;
        int startDirection = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < Field.FieldSize; i++)
        {
            for (int k = startPoint; k >= 0 && k < Field.FieldSize; k -= startDirection)
            {
                var cell = direction.x != 0 ? cells[k, i] : cells[i, k];

                var cellToMerge = FindCellToMerge(cell, cells, direction);
                if (cellToMerge != null)
                    return true;
            }
        }

        return false;
    }

    private Cell FindCellToMerge(Cell cell, Cell[,] cells, Vector2 direction)
    {
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; CheckFieldSize(x, y); x += (int)direction.x, y -= (int)direction.y)
        {
            if (cells[x, y].IsEmpty)
                continue;

            if (cells[x, y].Value == cell.Value && !cells[x, y].HasMerged)
                return cells[x, y];

            break;
        }

        return null;

        bool CheckFieldSize(int x, int y)
        {
            return x >= 0 && x < Field.FieldSize && y >= 0 && y < Field.FieldSize;
        }
    }
}
