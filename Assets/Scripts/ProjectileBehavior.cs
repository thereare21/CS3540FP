using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    
    public int damageAmount = 20;
    Boolean isHit;
    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {

        if (!isHit) {
            if (other.CompareTag("Player")) {
                isHit = true;
                var playerHealth = other.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
