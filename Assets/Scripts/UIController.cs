using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum ScaleColor { Red, Normal };
    [SerializeField] private Image _heatScale;
    [SerializeField] private Image _healthScale;
    [SerializeField] private Texture2D _crosshair;
    [SerializeField] private Vector2 _crosshairHotspot;

    private Color _scaleNormalColor;

    public static UIController Instance
    {
        get; private set;
    }

    private void Awake()
    {
        SetCrosshair();
        Instance = this;
    }

    void Start()
    {
        _scaleNormalColor = _heatScale.color;
    }

    public void SetCrosshair()
    {
        Cursor.SetCursor(_crosshair, _crosshairHotspot, CursorMode.Auto);
    }

    public void UpdateHeatScaleFillAmount(float value)
    {
        _heatScale.fillAmount = value;
    }

    public void UpdateHealthScaleFillAmount(float value)
    {
        _healthScale.fillAmount = value;
    }

    public void ChangeHeatScaleColor(ScaleColor color)
    {
        if (color == ScaleColor.Red)
        {
            _heatScale.color = Color.red;
        } else
        {
            _heatScale.color = _scaleNormalColor;
        }
    }
}
