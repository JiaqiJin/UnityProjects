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

    PlayerControl inputAction;
    CameraHandler cameraHandler;

    Vector2 movementInput;
    Vector2 cameraInput;
    private void Awake()
    {
        cameraHandler = CameraHandler.singleton;
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        if(cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }

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
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }
}