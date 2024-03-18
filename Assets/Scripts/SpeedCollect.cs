using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCollect : MonoBehaviour
{
    Animation speedAnimation;

    // Start is called before the first frame update
    void Start()
    {
        speedAnimation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().SpeedBoost();
            DestroySpeedItem();
        }
    }

    void DestroySpeedItem()
    {
        GetComponent<AudioSource>().Play();

        //play animation clip for destroy
        speedAnimation.Play();
        
        Destroy(gameObject, 2f);
    }
}
