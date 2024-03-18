using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public GameObject player;
    public float turnSpeed = 200f;
    public float range = 7f;
    public AudioClip disableSFX;

    bool playerIsInRange = false;
    bool turretEnabled = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < range)
        {
            playerIsInRange = true;
        }
        else
        {
            playerIsInRange = false;

        }
        if (playerIsInRange)
        {
            //transform.LookAt(player.transform);
            //transform.rotation = Vector3.Lerp(transform.rotation, transform.LookAt(player.transform); //can't be done :(

            //Vector3 forward = transform.forward;
            //Vector3 relativePos = player.transform.position - transform.position;

            //float angle = Vector3.Angle(relativePos, forward);

            //Quaternion lookRotation = Quaternion.LookRotation(relativePos);

            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

            if (turretEnabled)
            {
                Vector3 targetDirection = player.transform.position - transform.position;

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);

                newDirection.y = 0;

                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             Debug.Log("player detected");
        }

        playerIsInRange = true;

        //aggro towards player

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player has left range");
        }

        playerIsInRange = false;
    }

    public void DisableThis()
    {
        if (turretEnabled)
        {
            GameObject gun = transform.GetChild(0).gameObject;
            gun.transform.Rotate(-30, 0, 0);
            turretEnabled = false;
            AudioSource.PlayClipAtPoint(disableSFX, Camera.main.transform.position);
        }
        
    }

    public void playFiringSFX(AudioClip sfx) {
            AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
    }
}
