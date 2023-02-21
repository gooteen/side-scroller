using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProcessor : MonoBehaviour
{
    private PlayerControls _controls;

    public static InputProcessor Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;
        _controls = new PlayerControls();
        _controls.Mouse.Enable();
        _controls.Duck.Enable();
    }

    public Vector2 GetMousePosition()
    {
        return _controls.Mouse.MousePosition.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return _controls.Mouse.MouseDelta.ReadValue<Vector2>();
    }

    public Vector2 GetMovementDirection()
    {
        return _controls.Duck.Movement.ReadValue<Vector2>();
    }
}
