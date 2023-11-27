using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private const string MOVE = "Move";
    private const string RUN = "Run";
    private const string LOOK = "Look";
    private const string PLAYER_PREFS_BINDING = "InputBindings";
    [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInput;
    public event EventHandler OnInteractAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        Move_Forward,
        Move_Backward,
        Move_Left,
        Move_Right,
        Jump,
        Interact,
        Interact_Alternate,
        Pause,
    }
    //private PlayerInputActions playerInputActions;
    Vector2 movementVector;
    Vector2 lookVector;

    public void Jump_performed(InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    public void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public void InteractAlternate_performed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    public bool IsRunning()
    {
        bool isRunning = playerInput.actions[RUN].IsPressed();

        return isRunning;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        movementVector = playerInput.actions[MOVE].ReadValue<Vector2>();

        return movementVector.normalized;
    }
    public Vector2 GetLookVector()
    {
        lookVector = playerInput.actions[LOOK].ReadValue<Vector2>();

        return lookVector;
    }
}
