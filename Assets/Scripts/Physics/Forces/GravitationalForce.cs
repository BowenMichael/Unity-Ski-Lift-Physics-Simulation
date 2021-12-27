using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalForce : ForceGenerator
{
    private void Update()
    {

        updateForce(base.mParticle);
    }
    protected override void updateForce(Particle3D particle)
    {
        base.updateForce(particle);
        particle.addForce(particle.gavitaionalAcceleration * particle.scalar);
    }

    private Vector2 getGravitationalForce(Particle3D particle)
    {
        return particle.gavitaionalAcceleration;
    }
}
