using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float normalSpeed = 5f;
    public float speedUpSpeed = 8f;
    public float jumpHeight = 5;
    public float superJumpHeight = 5;
    public float gravity = 9.81f;
    public float airControl = 5f;
    public AudioClip[] footStepSFX; // array to allow variability in sfx
    public float footstepDelay = 0.5f; 
    
    // height variables for crouching and standing
    public float standHeight = 2.0f; 
    public float crouchHeight = 1.0f;
    public Transform playerTransform;
    public Transform cameraTransform;
    public float standCameraHeight = 0.8f; 
    public float crouchCameraHeight = 0.4f;

    bool isCrouching = false;
    bool shiftJump = false;
    bool isSpeeding = false;

    float speedTimeTotal = 2f;
    float speedTimer = 0f;

    CharacterController controller;
    Vector3 input, moveDirection;
    float lastFootstepTime; 
    int lastFootstepIndex = 0; 

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver) {
            HandleSpeed();
            HandleMovement();
            HandleCrouch();
        }  
        
    }

    void HandleSpeed()
    {
        if (isSpeeding)
        {
            speed = speedUpSpeed;
        }
        else
        {
            speed = normalSpeed;
        }

        speedTimer -= Time.deltaTime;
        if (speedTimer < 0f)
        {
            isSpeeding = false;
            GetComponentInChildren<ParticleSystem>().GetComponent<Renderer>().enabled = false;

        }

    }

    void HandleMovement() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        input *= speed;
        shiftJump = Input.GetKey(KeyCode.LeftShift); // check if crouch is pressed

        if (controller.isGrounded)
        {
            moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                if (shiftJump) {
                    moveDirection.y = Mathf.Sqrt(2 * superJumpHeight * gravity);

                }
                else {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
            }
            else
            {
                moveDirection.y = 0.0f;
            }

            if (!isCrouching) { 
                PlayFootstepSFX();
            }
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void HandleCrouch() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (!isCrouching && controller.isGrounded) {
                Crouch();
            }
        }
        else if (isCrouching) {
            StandUp();
        }
    }

    void Crouch() {
        controller.height = crouchHeight;
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, crouchCameraHeight,
            cameraTransform.localPosition.z);
        playerTransform.localScale = new Vector3(playerTransform.localScale.x, 0.5f, playerTransform.localScale.z);
        isCrouching = true;
    }

    void StandUp() {
        controller.height = standHeight;
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, standCameraHeight, 
            cameraTransform.localPosition.z);
        playerTransform.localScale = new Vector3(playerTransform.localScale.x, 1f, playerTransform.localScale.z);

        isCrouching = false;
    }

    void PlayFootstepSFX() {
        // play footstep sfx when moving on the ground
        if (input.magnitude > 0 && Time.time - lastFootstepTime > footstepDelay && footStepSFX.Length > 0)
        {
            lastFootstepTime = Time.time;
            AudioSource.PlayClipAtPoint(footStepSFX[lastFootstepIndex], transform.position, 0.1f); 
            // go to next sfx (or back to the beginning)
            lastFootstepIndex = (lastFootstepIndex + 1) % footStepSFX.Length;
        }
    }

    public void SpeedBoost()
    {

        isSpeeding = true;
        speedTimer = speedTimeTotal;
        GetComponentInChildren<ParticleSystem>().GetComponent<Renderer>().enabled = true;
    }

    // when crouching and then jumping, jump 2x as high
    public void HandleSuperJump() {
        if (isCrouching && Input.GetKey(KeyCode.Space)) {
            moveDirection.y = Mathf.Sqrt(4 * jumpHeight * gravity);
        }
        else {
            moveDirection.y = 0.0f;
        }
    }
}