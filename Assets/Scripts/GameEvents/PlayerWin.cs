using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    [SerializeField] private Field _field;

    private void OnEnable()
    {
        _field.MaxValueCell += OnMaxValueCell;
    }

    private void OnDisable()
    {
        _field.MaxValueCell -= OnMaxValueCell;        
    }

    private void OnMaxValueCell()
    {
        Debug.Log("Win");
    }
}
