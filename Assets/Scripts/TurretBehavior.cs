using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public GameObject player;
    public float turnSpeed = 200f;


    bool playerIsInRange = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsInRange)
        {
            transform.LookAt(player.transform);
            //transform.rotation = Vector3.Lerp(transform.rotation, transform.LookAt(player.transform); //can't be done :(

            //Vector3 forward = transform.forward;
            //Vector3 relativePos = player.transform.position - transform.position;

            //float angle = Vector3.Angle(relativePos, forward);

            //Quaternion lookRotation = Quaternion.LookRotation(relativePos);

            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

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
}
