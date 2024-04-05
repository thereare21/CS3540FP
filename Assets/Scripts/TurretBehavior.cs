using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public GameObject player;
    public GameObject currentProjectile;
    public float turnSpeed = 2f;
    public float range = 7f;
    public AudioClip disableSFX;
    public AudioClip shootSFX;
    public GameObject disabledTurret;
    public GameObject disableParticles;
    bool playerIsInRange = false;

    GameObject turretTip;
    float lockedTime = 0.0f;
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
        if (lockedTime >= 2)
        {
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, 0.5f);
            GameObject projectile = Instantiate(currentProjectile,
                turretTip.transform.position + transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 50, ForceMode.VelocityChange);

            lockedTime = 0;
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
                if (Physics.Raycast(turretTip.transform.position, transform.forward, out hit, range))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        lockedTime += Time.deltaTime;
                    }
                }
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
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        Instantiate(disableParticles, transform.position, transform.rotation);
        
        AudioSource.PlayClipAtPoint(disableSFX, Camera.main.transform.position);
        Destroy(gameObject);

    }

    public void playFiringSFX(AudioClip sfx) {
            AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
    }
}
