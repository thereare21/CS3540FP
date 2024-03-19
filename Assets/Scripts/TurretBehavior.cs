using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public GameObject player;
    public float turnSpeed = 2f;
    public float range = 7f;
    public AudioClip disableSFX;
    public GameObject disabledTurret;
    bool playerIsInRange = false;

    GameObject turretTip;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        var childTransforms = transform.GetComponentsInChildren<Transform>();
        foreach (var childTransform in childTransforms)
        {
            if (childTransform.tag == "TurretTip")
            {
                turretTip = childTransform.gameObject;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerIsInRange)
        {

                Vector3 targetDirection = player.transform.position - transform.position;

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);

                newDirection.y = 0;

                transform.rotation = Quaternion.LookRotation(newDirection);

        }
    }



    private void FixedUpdate()
    {
        Vector3 playerDirection = player.transform.position - turretTip.transform.position;
        RaycastHit hit;
        //Debug.DrawRay(turretTip.transform.position, playerDirection, Color.green, range);
        if (Physics.Raycast(turretTip.transform.position, playerDirection, out hit, range))
        {
            //print("Hit something with tag " + hit.collider.tag);
            if (hit.collider.CompareTag("Player"))
            {
                playerIsInRange = true;
            }

            else
            {
                playerIsInRange = false;

            }
        }
        else
        {
            playerIsInRange = false;
        }
    }
    public void DisableThis()
    {

        Instantiate(disabledTurret, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(disableSFX, Camera.main.transform.position);
        Destroy(gameObject);

    }

    public void playFiringSFX(AudioClip sfx) {
            AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
    }
}
