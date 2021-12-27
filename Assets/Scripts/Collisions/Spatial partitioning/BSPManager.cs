using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPManager : MonoBehaviour
{
    public GameManager gm;
    public float planeDistanceFromOrigin;
    public Vector3 mScreenSize;
    public bool active = true;
    public int bspCollisionCount;
    public GameObject planePrefab;
    public GameObject parentPlanes;

    private BSPNode root;
    private float waitTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm.GetComponent<GameManager>();
        //root = //athing
        root = new BSPNode();
        root.sParticles = gm.particles;
        GameObject tmp = new GameObject();
        parentPlanes = Instantiate(tmp, this.transform);
        addPlane(root);
        
    }

    void deletePlaneParentsChildren()
    {
        Transform[] objs = parentPlanes.GetComponentsInChildren<Transform>();
        for (int i = 1; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
    }

    void addPlane(BSPNode parent)
    {
        BSPPlane plane;

        if (parent.sParticles.Count <= 2) 
            return;

        Vector3 sumOfPositions = new Vector3();
        foreach (Particle3D part in parent.sParticles)
        {
            if (part == null) continue;
            sumOfPositions += part.transform.position;
        }
        sumOfPositions *= 1.0f/ parent.sParticles.Count;

        plane.position = sumOfPositions;
        plane.direction = Vector3.right;
        //if(waitTime < 0)
            //Instantiate(planePrefab, plane.position, new Quaternion(), parentPlanes.transform).transform.up = plane.direction;
        BSPNode front = new BSPNode();
        BSPNode back = new BSPNode();
        foreach (Particle3D part in parent.sParticles)
        {
            if (part == null) continue;
            if (Vector3.Dot((part.transform.position - plane.position), plane.direction) < 0.0f)
            {
                //Behind plane
                //should create a BSP Node: done
                //Shoulde place all particles in that node
                back.sParticles.Add(part);
                //Will be the back in Current node
            }
            else
            {
                //infront of plane
                //should create a BSP Node
                //Shoulde place all particles in that node
                front.sParticles.Add(part);
                //Will be the front in Current node
            }
        }
        addPlane(front);
        addPlane(back);
        parent.sFront = front;
        parent.sBack = back;

    }

    private void FixedUpdate()
    {
        bspCollisionCount = 0;
        if (!active) return;
        if (root == null) return;
        if (checkCollisions(root))
        {
            //Debug.Log("Collision BSP");
        }

        if (waitTime < 0)
        {
            deletePlaneParentsChildren();
            addPlane(root);
            waitTime = 1;
        }
        else
        {
            addPlane(root);
        }
        
        waitTime -= Time.fixedDeltaTime;

    }

    bool checkCollisions(BSPNode parent)
    {
        //bool colliding = false;
        if (parent.sParticles.Count < 2) return false;


        if(parent.sParticles.Count > 2)
        {
            checkCollisions(parent.sFront);
            checkCollisions(parent.sBack);
        }
        else
        {
            foreach (Particle3D part in parent.sParticles)
            {
                if (part == null) continue;
                part.TryGetComponent<SphereCollider>(out SphereCollider s1);
                part.TryGetComponent<BoxCollider>(out BoxCollider b1);

                foreach (Particle3D colidable in parent.sParticles)
                {
                    if (colidable == null || GameObject.ReferenceEquals(part.gameObject, colidable.gameObject)) continue;//particle is null or testing collision between the same object
                    colidable.TryGetComponent<SphereCollider>(out SphereCollider s2);
                    colidable.TryGetComponent<BoxCollider>(out BoxCollider b2);
                    if (s1 != null && s2 != null)
                    {
                        //Sphere on Sphere Collision 
                        if (gm.checkWithingWalls(s1.transform)) { Destroy(s1.gameObject); Debug.Log("BSPobjDestroyed"); continue; }


                        CollisionFunctions.sphereCollisionHandler(s1, s2);
                        bspCollisionCount++;
                        continue;
                    }

                    if(b1 != null && b2 != null)
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

            foreach (Particle3D part in parent.sParticles)
            {
                if (part != null && part.TryGetComponent<SphereCollider>(out SphereCollider s1))
                {
                    foreach (Vector4 plane in gm.planes)
                    {
                        if (gm.checkWithingWalls(s1.transform)) { Destroy(s1.gameObject); Debug.Log("BSPobjDestroyed"); continue; }
                        CollisionFunctions.spherePlaneColisionHandler(s1, plane);
                        bspCollisionCount++;
                    }
                }
            }
        }
        return true;
    }
}

struct BSPPlane
{
    public Vector3 position;
    public Vector3 direction;

}

class BSPNode
{
    public BSPNode()
    {
        sPlane.position = new Vector3();
        sPlane.direction = new Vector3();
        sParticles = new List<Particle3D>();
    }
    public BSPPlane sPlane;
    public BSPNode sFront;
    public BSPNode sBack;
    public List<Particle3D> sParticles;
}
