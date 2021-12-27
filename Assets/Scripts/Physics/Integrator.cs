using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Integrator : MonoBehaviour
{
    public static void Integrate(Particle2D particle, Transform transform)
    {
        Vector2 acceleration = particle.Force * particle.getInverseMass();// * Time.deltaTime;

        particle.velocity += acceleration * Time.fixedDeltaTime; //update Velocity

        particle.velocity *= Mathf.Pow(particle.dampingConstant, Time.fixedDeltaTime);

        transform.position += new Vector3(particle.velocity.x, particle.velocity.y, 0.0f) * Time.fixedDeltaTime; //update position

        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f); //locks to z axis

        particle.Force = new Vector2();
    }

    public static void Integrate(Particle3D particle, Transform transform)
    {
        Vector3 acceleration = particle.Force * particle.getInverseMass();// * Time.deltaTime;

        particle.velocity += acceleration * Time.fixedDeltaTime; //update Velocity

        particle.velocity *= Mathf.Pow(particle.dampingConstant, Time.fixedDeltaTime);

        transform.position += new Vector3(particle.velocity.x, particle.velocity.y, particle.velocity.z) * Time.fixedDeltaTime; //update position

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); //locks to z axis

        particle.Force = new Vector3();
    }


}
