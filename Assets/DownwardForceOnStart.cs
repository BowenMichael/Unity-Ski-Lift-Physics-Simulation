using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardForceOnStart : MonoBehaviour
{
    public float forceOnStart;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Particle3D>().addForce(new Vector3(0.0f, forceOnStart, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
