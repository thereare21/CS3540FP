using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralCollect : MonoBehaviour
{
    public int points = 10;

    private AudioSource collectSFX;

    // Start is called before the first frame update
    void Start()
    {
        collectSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            ScoreManager.AddScore(points);
            collectSFX.Play();
        }

        DestroyMineral();

        
    }

    private void DestroyMineral()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        collectSFX.Play();
        Destroy(gameObject, 1f);
    }
}
