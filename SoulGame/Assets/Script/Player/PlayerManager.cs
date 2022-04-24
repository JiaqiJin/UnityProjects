using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    Animator animator;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;

    public bool isInteracting;
    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        cameraHandler = CameraHandler.singleton;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");
      
        inputHandler.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

    }
    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        isSprinting = inputHandler.b_Input;

        if(isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }
}
