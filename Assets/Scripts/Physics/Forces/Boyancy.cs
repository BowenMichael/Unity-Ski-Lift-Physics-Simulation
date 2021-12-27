using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boyancy : ForceGenerator
{
    private float maxDepth;
    public float volume;
    public float waterHeight;
    public float liquidDensity;

    protected override void updateForce(Particle3D particle)
    {
        maxDepth = particle.GetComponent<SphereCollider>().bounds.size.y;
        float depth = particle.transform.position.y;
       // Debug.Log((waterHeight + maxDepth) + "; " + depth);
        if (depth >= waterHeight + maxDepth) {  return; }
        Vector3 force = new Vector3();

        if(depth <= waterHeight - maxDepth)
        {
            force.y = liquidDensity * volume;
            particle.addForce(force);
            Debug.Log("Under Water");
            return;
        }
        Debug.Log("Partially under Water");
        force.y = liquidDensity * volume * (depth - maxDepth - waterHeight) / 2.0f * maxDepth;
        particle.addForce(force);
    }
}
