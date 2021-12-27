using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Particle2D : MonoBehaviour
{
    public enum ProjectileType
    {
        UNKOWN = -1,
        PISTOL,
        ARTILLERY,
        FIREBALL,
        LAZER,
        POPPER,
        SPRING,
        SPRING_FIXED,
        BOYANCY
    }
    public float scalar;
    public Vector2 velocity;
    public Vector2 acceleration;
    private Vector2 accumulatedForces;
    public Vector2 gavitaionalAcceleration;
    public float dampingConstant;
    public float mass;
    //private Vector2 gravitationalForce;
    private float inverseMass;
    public bool isProjectile = true;
    public ProjectileType type;

    public Vector2 Force
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

    public void addForce(Vector2 force)
    {
        accumulatedForces += force;
    }

    private void Start()
    {
        if(mass != 0)
            inverseMass = 1 / mass;
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
    
    public void setDirection(Vector2 dir)
    {
        velocity = dir * scalar;
    }

    public Vector2 getDirection()
    {
        return velocity.normalized;
    }

    public void setVelocity(Vector3 velocity)
    {
        this.velocity = velocity * inverseMass;
    }

    public void addVelocity(Vector3 velocity)
    {
        Vector2 aditionalVelocity = velocity * inverseMass;
        this.velocity += aditionalVelocity;
    }

}
