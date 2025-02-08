using UnityEngine;

public class GoldUIViewSpawner : MonoBehaviour
{
    [SerializeField] private GoldUIView _goldUIView;

    public void Spawn(Base @base)
    {
        GoldUIView goldUIView = Instantiate(_goldUIView, transform).Initialize(@base);

        Vector3 newGoldUIViewPosition = @base.transform.position;
        goldUIView.transform.localPosition = newGoldUIViewPosition;
    }
}
