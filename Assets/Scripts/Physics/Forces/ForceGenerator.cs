using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle3D))]
public class ForceGenerator : MonoBehaviour
{
    protected Particle3D mParticle;
    // Start is called before the first frame update
    void Start()
    {
        mParticle = GetComponent<Particle3D>();
    }

    // Update is called once per frame
    void Update()
    {
        updateForce(mParticle);
    }

    protected virtual void updateForce(Particle3D particle)
    {

    }

}
