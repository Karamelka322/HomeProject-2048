using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Field : MonoBehaviour
{
    [SerializeField] private Cell _cellTemplate;
    [Space]
    [SerializeField] private RectTransform _rectTransform;

    public float CellSize;
    public float Spacing;

    private Cell[,] _arrayField;
    private bool _cellMoved;

    public const int FieldSize = 4;
    private readonly int _startFieldSize = 2;

    public event UnityAction<Cell[,]> ZeroEmptyCells;
    public event UnityAction MaxValueCell;

    public Cell[,] ArrayField => _arrayField;

    private void Awake()
    {
        _arrayField = new Cell[FieldSize, FieldSize];

        CreateField();
        StartGenerateCells();

    }

    private void OnEnable()
    {
        SwipeDetection.SwipeInput += OnSwipeInput;        
    }

    private void OnDisable()
    {
        SwipeDetection.SwipeInput -= OnSwipeInput;        
    }

    private void OnSwipeInput(Vector2 direction)
    {
        _cellMoved = false;

        ResetCellFlags();
        MoveField(direction);

        if (_cellMoved)
        {
            GenerateRandomCell();
        }

        if (GetEmptyCells().Count == 0)
        {
            ZeroEmptyCells?.Invoke(_arrayField);
        }
    }

    private void CreateField()
    {
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        _rectTransform.sizeDelta = new Vector2(fieldWidth, fieldWidth);

        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int i = 0; i < FieldSize; i++)
        {
            for (int n = 0; n < FieldSize; n++)
            {
                Cell cell = Instantiate(_cellTemplate, transform);
                var cellPosition = new Vector2(startX + (i * (CellSize + Spacing)), startY - (n * (CellSize + Spacing)));
                cell.transform.localPosition = cellPosition;

                _arrayField[i, n] = cell;
                cell.name = $"Cell({i}; {n})";

                cell.SetValue(i, n, 0);
            }
        }
    }

    private void StartGenerateCells()
    {
        for (int i = 0; i < _startFieldSize; i++)
        {
            GenerateRandomCell();
        }
    }

    private void MoveField(Vector2 direction)
    {
        int startPoint = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
        int startDirection = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < FieldSize; i++)
        {
            for (int k = startPoint; k >= 0 && k < FieldSize; k -= startDirection)
            {
                var cell = direction.x != 0 ? _arrayField[k, i] : _arrayField[i, k];

                if (cell.IsEmpty)
                    continue;

                var cellToMerge = FindCellToMerge(cell, direction);
                if(cellToMerge != null)
                {
                    cell.MergeWithCell(cellToMerge);

                    if (cellToMerge.Value == Cell.MaxValue)
                        MaxValueCell?.Invoke();

                    _cellMoved = true;
                    continue;
                }

                var emptyCell = FindEmptyCell(cell, direction);
                if(emptyCell != null)
                {
                    cell.MoveToCell(emptyCell);
                    _cellMoved = true;
                }
            }
        }
    }

    private Cell FindCellToMerge(Cell cell, Vector2 direction)
    {
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; CheckFieldSize(x, y); x += (int)direction.x, y -= (int)direction.y)
        {
            if (_arrayField[x, y].IsEmpty)
                continue;

            if (_arrayField[x, y].Value == cell.Value && !_arrayField[x, y].HasMerged)
                return _arrayField[x, y];

            break;
        }

        return null;
    }

    private Cell FindEmptyCell(Cell cell, Vector2 direction)
    {
        Cell emptyCell = null;

        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; CheckFieldSize(x, y); x += (int)direction.x, y -= (int)direction.y)
        {
            if (_arrayField[x, y].IsEmpty)
                emptyCell = _arrayField[x, y];
            else
                break;
        }

        return emptyCell;
    }

    private bool CheckFieldSize(int x, int y)
    {
        return x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
    }

    private void ResetCellFlags()
    {
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                _arrayField[x, y].ResetFlags();
            }
        }
    }

    private void GenerateRandomCell()
    {
        var emptyCells = GetEmptyCells();

        if (emptyCells.Count != 0)
        {
            int value = Random.Range(0, 10) == 0 ? 2 : 1;
            Cell randomCell = emptyCells[Random.Range(0, emptyCells.Count)];
            randomCell.SetValue(randomCell.X, randomCell.Y, value);
        }
    }

    private List<Cell> GetEmptyCells()
    {
        var emptyCells = new List<Cell>();

        for (int i = 0; i < FieldSize; i++)
        {
            for (int n = 0; n < FieldSize; n++)
            {
                if (_arrayField[i, n].IsEmpty)
                {
                    emptyCells.Add(_arrayField[i, n]);
                }
            }
        }

        return emptyCells;
    }
}
