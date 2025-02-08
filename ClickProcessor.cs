using UnityEngine;
using System;

public class ClickProcessor : MonoBehaviour
{
    public event Action OnClicked;

    private void OnMouseDown()
    {
        OnClicked?.Invoke();
    }
}
