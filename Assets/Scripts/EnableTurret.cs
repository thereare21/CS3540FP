using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTurret : MonoBehaviour
{
    public GameObject enabledTurret;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableThis()
    {

        Instantiate(enabledTurret, transform.position, transform.rotation);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(gameObject);

    }
}
