using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    public enum ProjectileType
    {
        UNKOWN = -1,
        PISTOL,
        ARTILLERY,
        FIREBALL,
        SPRING,
        SPRING_FIXED,
        BOYANCY
    }
    public float scalar;
    public Vector3 velocity;
    public Vector3 acceleration;
    private Vector3 accumulatedForces;
    public Vector3 gavitaionalAcceleration;
    public float dampingConstant;
    public float mass;
    private float inverseMass;
    public bool isProjectile = true;
    public ProjectileType type;

    public Vector3 Force
    {
        get
        {
            return accumulatedForces;
        }
        set
        {
            accumulatedForces = value;
        }
    }

    public void addForce(Vector3 force)
    {
        accumulatedForces += force;
    }

    private void Start()
    {
        if (mass != 0)
            inverseMass = 1 / mass;
        else
            setInfiniteMass();
    }

    private void FixedUpdate()
    {
        Integrator.Integrate(this, transform);
    }

    public float getInverseMass()
    {
        return inverseMass;
    }

    public void setInfiniteMass()
    {
        inverseMass = 0;
    }

    public void setDirection(Vector3 dir)
    {
        velocity = dir * scalar;
    }

    public Vector3 getDirection()
    {
        return velocity.normalized;
    }

    public void setVelocity(Vector3 velocity)
    {
        this.velocity = velocity * inverseMass;
    }

    public void addVelocity(Vector3 velocity)
    {
        Vector3 aditionalVelocity = velocity * inverseMass;
        this.velocity += aditionalVelocity;
    }
}
