using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFunctions : MonoBehaviour
{
    public static void collisionBetweenLists(List<Particle3D> list)
    {
       
    }

    public static void boxCollisionsAABBAABB(BoxCollider b1, BoxCollider b2)
    {
      
        //Return if no collision
        Vector3 diff = b1.gameObject.transform.position - b2.gameObject.transform.position; //midline
        Vector3 absDiff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z));
        Vector3 sumOfRadii = b1.size * .5f + b2.size * .5f;
        if (absDiff.x > sumOfRadii.x || absDiff.y > sumOfRadii.y || absDiff.z > sumOfRadii.z) return;

        //Collision
        Debug.Log("BoxBox Collision");
        float boxRadius1 = Mathf.Abs((getClosestPointAABB(b1, b2.gameObject.transform.position) - b1.gameObject.transform.position).magnitude);
        float boxRadius2 = Mathf.Abs((getClosestPointAABB(b2, b1.gameObject.transform.position) - b2.gameObject.transform.position).magnitude);
        float penatration = (boxRadius1 + boxRadius2) - diff.magnitude;
        Vector3 pointOnWall = getClosestPointAABB(b1, b2.gameObject.transform.position);//in world space
        Vector3 collisionDirection = getBoxPlaneDirection(b1, pointOnWall);
        if (b1.TryGetComponent(out Particle3D s1Part) && b2.TryGetComponent(out Particle3D s2Part))
        {
            if (collisionBetweenTwoParticles(s1Part, s2Part, -collisionDirection, penatration))
            {
                //Debug.Log("BoxBox Collision");
            }
        }


    }

    public static void SphereAABBCollision(BoxCollider b1, SphereCollider s2)
    {

        //Check for collision
        float sphereRadius = s2.radius * s2.transform.localScale.y;
        float sqDistance = sqDistPointAABB(b1, s2.gameObject.transform.position);

        if (sqDistance > sphereRadius * sphereRadius) return;
        //Debug.Log("SphereAABB Collision");
        

        Vector3 diff = b1.gameObject.transform.position - s2.gameObject.transform.position; //midline
        Vector3 pointOnWall = getClosestPointAABB(b1, s2.gameObject.transform.position);//in world space
        float boxRadius = (pointOnWall - b1.gameObject.transform.position).magnitude;
        //Vector4 plane = getWallPlanePointIsOn(b1, pointOnWall);
        //spherePlaneColisionHandler(s2, plane);
        float penatration = (Mathf.Abs(boxRadius) + sphereRadius) - diff.magnitude;
        Vector3 collisionDirection = getBoxPlaneDirection(b1, pointOnWall);
        if (b1.TryGetComponent(out Particle3D s1Part) && s2.TryGetComponent(out Particle3D s2Part))
        {

            if (collisionBetweenTwoParticles(s1Part, s2Part, -collisionDirection, penatration))
            {
                Debug.Log("SphereAABB Collision");
            }
        }

    }

    public static Vector3 getClosestPointAABB(BoxCollider b1, Vector3 point)
    {
        Vector3 halfSize = b1.size * .5f;
        Vector3 pos = b1.gameObject.transform.position;
        return new Vector3(Mathf.Clamp(point.x, pos.x-halfSize.x, pos.x+halfSize.x), 
                            Mathf.Clamp(point.y, pos.y-halfSize.y, pos.y+halfSize.y), 
                            Mathf.Clamp(point.z, pos.z-halfSize.z, pos.z+halfSize.z));
    }

    public static Vector3 getBoxPlaneDirection(BoxCollider b1, Vector3 pointOnWall)
    {
        Vector3 bCenter = b1.gameObject.transform.position;
        Vector3 halfSize = b1.size * .5f;
        if (pointOnWall.x >= bCenter.x + halfSize.x) return Vector3.right;
        if (pointOnWall.x <= bCenter.x - halfSize.x) return Vector3.left;
        if (pointOnWall.y >= bCenter.y + halfSize.y) return Vector3.up;
        if (pointOnWall.y <= bCenter.y - halfSize.y) return Vector3.down;
        if (pointOnWall.z >= bCenter.z + halfSize.z) return Vector3.forward;
        if (pointOnWall.z <= bCenter.z - halfSize.z) return Vector3.back;
        Debug.LogWarning("Point was not outside box");
        return new Vector4(0,0,0);
    }

    public static float sqDistPointAABB(BoxCollider b1, Vector3 point)
    {
        float sqDist = 0.0f;

        Vector3 v = point;
        Vector3 halfSize = b1.gameObject.transform.position + b1.size * .5f ;
        
        sqDist += distanceOutsideBoundsOnAxis(v.x, b1.bounds.min.x, b1.bounds.max.x);
        sqDist += distanceOutsideBoundsOnAxis(v.y, b1.bounds.min.y, b1.bounds.max.y);
        sqDist += distanceOutsideBoundsOnAxis(v.z, b1.bounds.min.z, b1.bounds.max.z);
        return sqDist;
    }
    private static float distanceOutsideBoundsOnAxis(float v, float axisMin, float axisMax)
    {
        float sqDist = 0.0f;
        if (v < axisMin) sqDist += (axisMin - v) * (axisMin - v);
        if (v > axisMax) sqDist += (v - axisMax) * (v - axisMax);
        return sqDist;
    }

    public static void sphereCollisionHandler(SphereCollider s1, SphereCollider s2)
    {

        Vector3 diff = s1.gameObject.transform.position - s2.gameObject.transform.position; //midline
        float penatration = (s1.radius * s1.transform.localScale.y + s2.radius * s2.transform.localScale.y) - diff.magnitude;
        if (s1.TryGetComponent(out Particle3D s1Part) && s2.TryGetComponent(out Particle3D s2Part))
        {

            if(collisionBetweenTwoParticles(s1Part, s2Part, diff.normalized, penatration))
            {
                Debug.Log("sphereCollision");
            }
        }
        else
        {
        }

    }

    public static void sphereCylyinderHandler(SphereCollider s, SphereCollider c)
    {
        Vector3 sPosInC = c.transform.InverseTransformPoint( s.transform.position);
        Vector3 diff = sPosInC - new Vector3(0.0f, sPosInC.y,0.0f );
        diff = c.transform.TransformDirection(diff);
        float penatration = (s.radius * s.transform.localScale.y + c.radius) - diff.magnitude;
        if (s.TryGetComponent(out Particle3D s1Part) && c.TryGetComponent(out Particle3D s2Part))
        {

            if (collisionBetweenTwoParticles(s1Part, s2Part, diff.normalized, penatration))
            {
                Debug.Log("sphereCyliderCollision");
            }
        }
        else
        {
        }
    }


    public static void spherePlaneColisionHandler(SphereCollider s1, Vector4 plane)
    {
        Vector3 planeNormal = new Vector3(plane.x, plane.y, plane.z);
        float distanceFromOriginAlongNormal = plane.w;
        float penatration = Vector3.Dot(s1.gameObject.transform.position + (s1.radius * s1.transform.localScale.y) * planeNormal.normalized, planeNormal) + distanceFromOriginAlongNormal;

        if (s1.TryGetComponent(out Particle3D s1Part))
        {
            if (-penatration > 0)
            {
            }
            else
            {
                if (Mathf.Abs(penatration) < s1.radius * s1.transform.localScale.y && penatration > 0)
                {
                    planeNormal *= -1;
                }
                Particle3D planePart = s1.gameObject.AddComponent<Particle3D>();
                planePart.setInfiniteMass();
                if (collisionBetweenTwoParticles(s1Part, planePart, planeNormal, penatration))
                    //Debug.Log("Colision With Plane");
                
                GameObject.Destroy(planePart);
            }
        }

    }

    public static bool collisionBetweenTwoParticles(Particle3D part1, Particle3D part2, Vector3 colisionDirection, float penatration)
    {
        if (-penatration > 0)
        {
            //not Colliding
            return false;
        }
        else
        {
            //coliding
            //Debug.Log("Colliding");

            //calc move per mass
            float TotalInverseMass = (part1.getInverseMass() + part2.getInverseMass());
            Vector3 movePerMass = colisionDirection * penatration * 1 / TotalInverseMass;

            //Calc velocity along colision direction
            float restitution = .5f;
            float seperatingVelocity = Vector3.Dot((part1.velocity - part2.velocity), colisionDirection); //magnitude of net velocity onlong the colision direction
            float newSeperatingVelocity = -seperatingVelocity * restitution; //Multiply by the restitution constant
            float deltaVelocity = newSeperatingVelocity - seperatingVelocity; //Change in velocity
            float impulseVelocity = deltaVelocity * 1 / TotalInverseMass; //impulse velocity based on total mass of collision

            //convert back to vector3
            Vector3 velImpulsePerIMass = impulseVelocity * colisionDirection; //converting velocity back to vector3 along the direction of the colision

            //Debug.Log(impulseVelocity + "; " + colisionDirection + "; " + velImpulsePerIMass);

            //move they from being inside each other
            if (part1.getInverseMass() != 0)
                part1.transform.position += movePerMass * part1.getInverseMass();
            else
            {
                part2.transform.position += -movePerMass * part1.getInverseMass();
            }
            if (part2.getInverseMass() != 0)
                part2.transform.position += -movePerMass * part2.getInverseMass();
            else
            {
                part1.transform.position += movePerMass * part1.getInverseMass();
            }

            //Add velocity
            part1.addVelocity(velImpulsePerIMass);
            part2.addVelocity(-velImpulsePerIMass);
            //Debug.Log(part1.velocity+"; "+ part2.velocity);

            return true;

        }
        // return false;

    }

    
}
