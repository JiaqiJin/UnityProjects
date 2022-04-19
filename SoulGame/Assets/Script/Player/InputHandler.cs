using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float vertical;
    public float horizontal;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;

    public bool rollFlag;
    public bool sprintFlag;
    public float rollInputTime;

    PlayerControl inputAction;

    Vector2 movementInput;
    Vector2 cameraInput;

    public void OnEnable()
    {
        if (inputAction == null)
        {
            inputAction = new PlayerControl();
            inputAction.PlayerMovement.Movement.performed += inputAction => movementInput = inputAction.ReadValue<Vector2>();
            inputAction.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRoolInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRoolInput(float delta)
    {
        b_Input = inputAction.PlayerAction.Roll.IsPressed(); //== UnityEngine.InputSystem.InputActionPhase.Started;
        //b_Input = true;
       
        if (b_Input)
        {
            rollInputTime += delta;
            sprintFlag = true;
            //rollFlag = true;
        }
        else
        {
            if(rollInputTime > 0 && rollInputTime < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTime = 0;
        }
    }
}