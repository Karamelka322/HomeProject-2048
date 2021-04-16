using UnityEngine;
using TMPro;

public abstract class ScoreCounter: MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _text;
    protected int _counter;
}
