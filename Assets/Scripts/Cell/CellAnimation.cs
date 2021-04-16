using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class CellAnimation : MonoBehaviour
{
    [SerializeField] private Cell _cell;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _cell.UpdatedValue += OnUpdatedValue;
        _cell.MergeCell += OnMergeCell;
    }

    private void OnDisable()
    {
        _cell.UpdatedValue -= OnUpdatedValue;
        _cell.MergeCell -= OnMergeCell;
    }

    private void OnUpdatedValue(int value)
    {
        if(value == 0)
        {
            _rectTransform.DOScale(Vector3.one, .2f);
        }
    }

    private void OnMergeCell(Cell cell)
    {
        StartCoroutine(Impulse(cell));
    }

    IEnumerator Impulse(Cell cell)
    {
        var rect = cell.GetComponent<RectTransform>();

        rect.localScale = Vector2.one;
        rect.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .2f);

        yield return new WaitForSeconds(.2f);

        rect.DOScale(Vector3.one, .2f);
    }
}
