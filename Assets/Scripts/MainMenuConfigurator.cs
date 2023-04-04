using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuConfigurator : MonoBehaviour
{
    public Texture2D _normalCursor;
    public Texture2D _handCursor;
    public Vector2 _hotSpot;

    void Start()
    {
        SetNormalCursor();
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(_normalCursor, _hotSpot, CursorMode.Auto);
    }

    public void SetHandCursor()
    {
        Cursor.SetCursor(_handCursor, _hotSpot, CursorMode.Auto);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

}
