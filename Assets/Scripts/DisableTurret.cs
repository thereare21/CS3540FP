using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTurret : MonoBehaviour
{
    public float disableDistance = 3f;
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
