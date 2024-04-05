using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTurret : MonoBehaviour
{
    public float disableDistance = 3f;
    public AudioClip firingSFX;
    public GameObject turret;
    public GameObject distract;

    bool firing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firing = true;
            TurretBehavior turretBehavior = turret.GetComponent<TurretBehavior>();
            if (turretBehavior != null) {
                turretBehavior.playFiringSFX(firingSFX);
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject projectile = Instantiate(distract,
                transform.position + transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 10, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        if (firing)
        {
            Debug.Log("firing");
            RaycastHit hit;
            //Debug.DrawRay(transform.position, transform.forward, Color.green, disableDistance);
            if (Physics.Raycast(transform.position, transform.forward, out hit, disableDistance))
            {
                Debug.Log("Object found");
                if (hit.collider.CompareTag("Turret"))
                {
                    hit.collider.GetComponent<TurretBehavior>().DisableThis();
                }
            }
            firing = false;
        }
    }
}
