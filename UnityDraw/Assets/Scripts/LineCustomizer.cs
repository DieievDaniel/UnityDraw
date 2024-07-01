using UnityEngine;
using UnityEngine.UI;

public class LineCustomizer : MonoBehaviour
{
    [SerializeField] private DrawScript drawScript;
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Button redButton;
    [SerializeField] private Button greenButton;
    [SerializeField] private Button blueButton;

    private void Start()
    {
        widthSlider.onValueChanged.AddListener(SetLineWidth);

        redButton.onClick.AddListener(() => SetLineColor(Color.red));
        greenButton.onClick.AddListener(() => SetLineColor(Color.green));
        blueButton.onClick.AddListener(() => SetLineColor(Color.blue));
    }

    public void SetLineWidth(float width)
    {
        drawScript.SetLineWidth(width);
    }

    public void SetLineColor(Color color)
    {
        drawScript.SetLineColor(color);
    }
}
