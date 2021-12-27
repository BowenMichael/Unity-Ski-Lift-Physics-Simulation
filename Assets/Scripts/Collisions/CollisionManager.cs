using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Collidable[] collidables;
    public GameManager gm;
    public List<Particle3D> particleToRemove = new List<Particle3D>();
    public int collisionChecks = 0;
    public bool active = true;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        
    }
    private void FixedUpdate()
    {
        collisionChecks = 0;
        if (!active) return;
        collidables = GameObject.FindObjectsOfType<Collidable>();
        collisionHandler();
    }
    public void collisionHandler()
    {
        foreach (Particle3D part in gm.particles)
        {
            if (part == null) continue;
            part.TryGetComponent<SphereCollider>(out SphereCollider s1);
            part.TryGetComponent<BoxCollider>(out BoxCollider b1);

            foreach (Particle3D colidable in gm.particles)
            {
                if (colidable == null || GameObject.ReferenceEquals(part.gameObject, colidable.gameObject)) continue;//particle is null or testing collision between the same object
                colidable.TryGetComponent<SphereCollider>(out SphereCollider s2);
                colidable.TryGetComponent<BoxCollider>(out BoxCollider b2);
                if (s1 != null && s2 != null)
                {
                    //Sphere on Sphere Collision 
                    if (gm.checkWithingWalls(s1.transform)) { 

                        Destroy(s1.gameObject); 
                        Debug.Log("BSPobjDestroyed"); 
                        continue; 
                    }


                    CollisionFunctions.sphereCollisionHandler(s1, s2);
                    collisionChecks++;
                    continue;
                }

                if (b1 != null && b2 != null)
                {
                    //Box on Box
                    CollisionFunctions.boxCollisionsAABBAABB(b1, b2);
                    continue;
                }

                if (s1 != null && b2 != null)
                {
                    //sphere on Box
                    CollisionFunctions.SphereAABBCollision(b2, s1);
                    continue;
                }



            }

        }

        foreach (Particle3D part in gm.particles)
        {
            if(part != null && part.TryGetComponent<SphereCollider>(out SphereCollider s1))
            {
                foreach(Vector4 plane in gm.planes)
                {
                    if (gm.checkWithingWalls(s1.transform)) { Destroy(s1.gameObject); continue; }
                    CollisionFunctions.spherePlaneColisionHandler(s1, plane);
                    collisionChecks++;
                }
            }
        }

        //sphereCollisionHandler(gm.particles[0].GetComponent<SphereCollider>(), gm.particles[1].GetComponent<SphereCollider>());
        //if (particleToRemove != null)
        //{
        //    foreach (Particle3D part in particleToRemove)
        //    {
        //        if (particleToRemove != null)
        //        {
        //            gm.particles.Remove(part);
        //        }
        //    }
        //    particleToRemove.Clear();
        //}

    }

}
