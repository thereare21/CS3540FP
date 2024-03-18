using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 5;
    public float gravity = 9.81f;
    public float airControl = 5f;
    public AudioClip[] footStepSFX; // array to allow variability in sfx
    public float footstepDelay = 0.5f; 

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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        input *= speed;

        if (controller.isGrounded)
        {
            moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f;
            }

            // play footstep sfx when moving on the ground
            if (input.magnitude > 0 && Time.time - lastFootstepTime > footstepDelay && footStepSFX.Length > 0)
            {
                lastFootstepTime = Time.time;
                AudioSource.PlayClipAtPoint(footStepSFX[lastFootstepIndex], transform.position, 0.1f); 
                // go to next sfx (or back to the beginning)
                lastFootstepIndex = (lastFootstepIndex + 1) % footStepSFX.Length;
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
}
