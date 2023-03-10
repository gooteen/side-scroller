using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum ScaleColor { Red, Normal };
    [SerializeField] private Image _scale;
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
        _scaleNormalColor = _scale.color;
    }

    public void SetCrosshair()
    {
        Cursor.SetCursor(_crosshair, _crosshairHotspot, CursorMode.Auto);
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
