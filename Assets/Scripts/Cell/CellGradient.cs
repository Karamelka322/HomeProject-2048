using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CellGradient : MonoBehaviour
{
    [SerializeField] private Cell _cell;
    [Space]
    [SerializeField] private Gradient _gradien;

    private Image _image;

    private readonly float _maxValue = 11f;

    private void Awake()
    {
        _image = GetComponent<Image>();
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
            float _colorValue = (1f / _maxValue) * value;
            _image.color = _gradien.Evaluate(_colorValue);
        }
        else
        {
            _image.color = _gradien.Evaluate(0f);
        }

    }
}
