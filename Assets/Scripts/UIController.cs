using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum ScaleColor { Red, Normal };
    [SerializeField] Image _scale;
    private Color _scaleNormalColor;

    public static UIController Instance
    {
        get; private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _scaleNormalColor = _scale.color;
    }

    public void UpdateScaleFillAmount(float value)
    {
        _scale.fillAmount = value;
    }

    public void ChangeScaleColor(ScaleColor color)
    {
        if (color == ScaleColor.Red)
        {
            _scale.color = Color.red;
        } else
        {
            _scale.color = _scaleNormalColor;
        }
    }
}
