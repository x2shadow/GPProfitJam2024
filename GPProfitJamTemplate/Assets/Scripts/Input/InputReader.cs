using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, InputActions.IGameplayActions, InputActions.IUIActions
{
    InputActions inputActions;

    void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new InputActions();

            inputActions.Gameplay.SetCallbacks(this);
            inputActions.UI.SetCallbacks(this);

            SetGameplay();
        }
    }

    public void SetGameplay()
    {
        inputActions.Gameplay.Enable();
        inputActions.UI.Disable();
    }

    public void SetUI()
    {
        inputActions.Gameplay.Disable();
        inputActions.UI.Enable();
    }

    public event Action<Vector2> MoveEvent;

    public event Action JumpEvent;
    public event Action JumpCanceledEvent;

    public event Action PauseEvent;
    public event Action ResumeEvent;

    public event Action ClickEvent;

    public event Action PointEvent;


    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed) JumpEvent?.Invoke();
        if(context.phase == InputActionPhase.Canceled)  JumpCanceledEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            //SetUI();
        } 
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
            //SetGameplay();
        } 
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed) ClickEvent?.Invoke();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed) PointEvent?.Invoke();
    }
}
