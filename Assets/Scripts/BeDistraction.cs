using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeDistraction : MonoBehaviour
{
    GameObject npc;
    // Start is called before the first frame update
    void Start()
    {
        npc = GameObject.FindGameObjectWithTag("NPC");
        Invoke("MakeDistraction", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeDistraction()
    {
        npc.GetComponent<EnemyAI>().SetDistraction(transform.position);
    }
}
