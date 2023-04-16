using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public enum ScaleColor { Red, Normal };

    [SerializeField] private Image _heatScale;
    [SerializeField] private Image _healthScale;
    [SerializeField] private GameObject _heatContainer;
    [SerializeField] private GameObject _healthContainer;
    [SerializeField] private GameObject _coinImage;

    [SerializeField] private Image _fadeOutPanel;
    [SerializeField] private Texture2D _crosshair;
    [SerializeField] private Texture2D _normalPointer;
    [SerializeField] private Texture2D _handPointer;
    [SerializeField] private Vector2 _crosshairHotspot;
    [SerializeField] private Vector2 _normalPointerHotspot;
    [SerializeField] private Vector2 _handPointerHotspot;
    [SerializeField] private GameObject _mainCanvas;

    [SerializeField] private GameObject _exitButton;
    [SerializeField] private GameObject _replayButton;

    [SerializeField] private TMP_Text _pointCounter;
    [SerializeField] private TMP_Text _timer;

    [SerializeField] private TMP_Text _message;
    [SerializeField] private string _victoryMessageText;
    [SerializeField] private string _deathMessageText;
    [SerializeField] private float _timeBetweenLetters;

    [SerializeField] private float _fadeOutTempo;

    private Color _scaleNormalColor;

    public static UIController Instance
    {
        get; private set;
    }

    public TMP_Text Counter { get { return _pointCounter; } }
    public TMP_Text Timer { get { return _timer; } }

    private void Awake()
    {
        SetCrosshair();
        Instance = this;
    }

    void Start()
    {
        _scaleNormalColor = _heatScale.color;
    }

    public void HideAllUI()
    {
        _heatScale.gameObject.SetActive(false);
        _healthScale.gameObject.SetActive(false);
        _heatContainer.SetActive(false);
        _healthContainer.SetActive(false);
        _pointCounter.gameObject.SetActive(false);
        _timer.gameObject.SetActive(false);
        _coinImage.SetActive(false);
    }

    public void ShowAllUI()
    {
        _heatScale.gameObject.SetActive(true);
        _healthScale.gameObject.SetActive(true);
        _heatContainer.SetActive(true);
        _healthContainer.SetActive(true);
        _pointCounter.gameObject.SetActive(true);
        _timer.gameObject.SetActive(true);
        _coinImage.SetActive(true);
    }

    public void ShowButtons()
    {
        _exitButton.SetActive(true);
        _replayButton.SetActive(true);
    }

    public void SetCrosshair()
    {
        Cursor.SetCursor(_crosshair, _crosshairHotspot, CursorMode.Auto);
    }

    public void SetNormalPointer()
    {
        Cursor.SetCursor(_normalPointer, _normalPointerHotspot, CursorMode.Auto);
    }

    public void SetHandPointer()
    {
        Cursor.SetCursor(_handPointer, _handPointerHotspot, CursorMode.Auto);
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
    
    public IEnumerator SpellVictoryMessage()
    {
        _message.color = Color.green;
        foreach (char _c in _victoryMessageText)
        {
            _message.text += _c;
            yield return new WaitForSeconds(_timeBetweenLetters);
        }
    }

    public IEnumerator SpellDeathMessage()
    {
        _message.color = Color.red;
        foreach (char _c in _deathMessageText)
        {
            _message.text += _c;
            yield return new WaitForSeconds(_timeBetweenLetters);
        }
    }

}
