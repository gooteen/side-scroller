using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum ScaleColor { Red, Normal };
    [SerializeField] private Image _heatScale;
    [SerializeField] private Image _healthScale;
    [SerializeField] private Image _fadeOutPanel;
    [SerializeField] private Texture2D _crosshair;
    [SerializeField] private Vector2 _crosshairHotspot;
    [SerializeField] private GameObject _mainCanvas;

    [SerializeField] private float _fadeOutTempo;
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

    public IEnumerator FadeOut()
    {
        _mainCanvas.SetActive(false);
        while (_fadeOutPanel.color.a <= 0.99)
        {
            _fadeOutPanel.color = new Color(_fadeOutPanel.color.r, _fadeOutPanel.color.g, _fadeOutPanel.color.b, _fadeOutPanel.color.a + _fadeOutTempo * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
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
