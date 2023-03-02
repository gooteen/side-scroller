using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        /*
        _controls.Mouse.RightMouseButton.performed += RightMouseButtonPressed;
        _controls.Mouse.RightMouseButton.canceled += RightMouseButtonPressed;
        */
    }

    public Vector2 GetMousePosition()
    {
        return _controls.Mouse.MousePosition.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return _controls.Mouse.MouseDelta.ReadValue<Vector2>();
    }

    public bool RightMouseButtonPressed()
    {
        if (_controls.Mouse.RightMouseButton.phase == InputActionPhase.Performed)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool LeftMouseButtonPressed()
    {
        if (_controls.Mouse.LeftMouseButton.phase == InputActionPhase.Performed)
        {
            Debug.Log("Shoot");
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
    private void IsRightMouseButtonPressed(InputAction.CallbackContext ctxt)
    {
        _controls.Mouse.RightMouseButton.phase = InputActionPhase.
        if (ctxt.performed)
        {
            return true;
        } else
        {
            return false

        }
    }
    */

    public Vector2 GetMovementDirection()
    {
        return _controls.Duck.Movement.ReadValue<Vector2>();
    }

    public bool JumpButtonPressed()
    {
        return _controls.Duck.Jump.triggered;
    }
}