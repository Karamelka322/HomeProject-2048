using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image _image;

    public int X { get; private set; }
    public int Y { get; private set; }
    public int Value { get; private set; }
    public bool HasMerged { get; private set; }

    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);
    public bool IsEmpty => Value == 0;

    public const int MaxValue = 11;

    public event UnityAction<int> UpdatedValue;
    public event UnityAction<Cell> MergeCell;

    public void SetValue(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;

        UpdatedValue?.Invoke(Value);
    }

    private void IncreaseValue()
    {
        Value++;
        HasMerged = true;

        UpdatedValue?.Invoke(Value);
    }

    public void MergeWithCell(Cell otherCell)
    {
        otherCell.IncreaseValue();
        SetValue(X, Y, 0);
        MergeCell?.Invoke(otherCell);
    }

    public void MoveToCell(Cell targetCell)
    {
        targetCell.SetValue(targetCell.X, targetCell.Y, Value);
        SetValue(X, Y, 0);
    }

    public void ResetFlags()
    {
        HasMerged = false;
    }
}
