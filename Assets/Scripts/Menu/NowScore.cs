using UnityEngine;
using UnityEngine.Events;

public class NowScore : ScoreCounter
{
    [SerializeField] private Field _field;

    private Cell[,] cells;

    public event UnityAction<int> UpdatedScore;

    private void Start()
    {
        cells = _field.ArrayField;
        _text.text = "0";

        for (int x = 0; x < Field.FieldSize; x++)
        {
            for (int y = 0; y < Field.FieldSize; y++)
            {
                cells[x, y].UpdatedValue += OnUpdateValue;
            }
        }
    }

    private void OnDisable()
    {
        for (int x = 0; x < Field.FieldSize; x++)
        {
            for (int y = 0; y < Field.FieldSize; y++)
            {
                cells[x, y].UpdatedValue -= OnUpdateValue;
            }
        }
    }

    private void OnUpdateValue(int value)
    {
        if(value > 1)
        {
            value = (int)Mathf.Pow(2, value);
            _counter += value;
            _text.text = _counter.ToString();
            UpdatedScore?.Invoke(_counter);
        }
    }
}
