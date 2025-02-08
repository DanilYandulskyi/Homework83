using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GoldUIView : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        UpdateText(0);

        _base.GoldAmountChanged += UpdateText;
    }

    private void OnDestroy()
    {
        _base.GoldAmountChanged -= UpdateText;
    }

    public GoldUIView Initialize(Base @base)
    {
        _base = @base;
        return this;
    }

    public void UpdateText(int goldAmount)
    {
        _text.text = $"Gold - {goldAmount}";
    }
}
