using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CellCounter : MonoBehaviour
{
    [SerializeField] private Cell _cell;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _cell.UpdatedValue += OnUpdatedValue;
    }

    private void OnDisable()
    {
        _cell.UpdatedValue -= OnUpdatedValue;        
    }

    private void OnUpdatedValue(int value)
    {
        if(value != 0)
        {
            _text.text = _cell.Points.ToString();
        }
        else
        {
            _text.text = "";
        }
    }
}
