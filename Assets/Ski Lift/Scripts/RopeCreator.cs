using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class RopeCreator : MonoBehaviour
{
    public enum RopeType
    {
        UNKOWN = -1,
        CIRCLE,
        PATH
    }
    [Header("Rope details")]
    public RopeType ropeType;
    public GameObject ropeNode;
    public int numOfNodes;
    public float tensionMult;

    [Header("Circle")]
    public float radius1;


    [Header("Path")]
    public PathCreator path;
    public PathFollower follower;


    public List<ParticleSpring> nodes = new List<ParticleSpring>();
    public List<SphereCollider> nodesParts = new List<SphereCollider>();
    public List<SphereCollider> cylinderParts = new List<SphereCollider>();
    public Particle3D center;
    public SphereCollider centerCollider;
    public GameObject groundPlane;
    Plane slope;
    Vector4 plane;
    public float throttle = 1f;

    float restLengthPerNode = 1;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(SphereCollider part in nodesParts)
        {
            foreach(SphereCollider cParts in cylinderParts) 
            {
                CollisionFunctions.sphereCylyinderHandler(part, cParts);
            }
            CollisionFunctions.spherePlaneColisionHandler(part, plane);
        }
        moveRope();
    }

    private void init()
    {
        plane = -new Vector4(groundPlane.transform.up.x, groundPlane.transform.up.y, groundPlane.transform.up.z, 0);

        if (ropeNode.TryGetComponent(out ParticleSpring partSpring))
        {
            if (ropeType == RopeType.CIRCLE)
            {
                centerCollider.radius = radius1;
                //radius1 = numOfNodes;
                //Restlength from num of nodes
                float circumfrence = 2 * Mathf.PI * radius1;
                restLengthPerNode = numOfNodes / circumfrence;
                offset = new Vector3(radius1, 1, radius1);
            }
            else if (ropeType == RopeType.PATH)
            {
                restLengthPerNode =   path.path.length / numOfNodes;
            }
            partSpring.restLength = restLengthPerNode * tensionMult;
            createRope();
        }
        else
        {
            Debug.LogError("RopeNode needs Particle Spring Component");
        }
    }
    void createRope()
    {
        for (int i = 0; i < numOfNodes; ++i)
        {
            Vector3 spawnPos = new Vector3();
            if (ropeType == RopeType.CIRCLE)
            {
                //populate nodes
                float nodesRange = (2 * (i * 1.0f / numOfNodes) - 1) * Mathf.PI;
                spawnPos = transform.position + new Vector3(offset.x * Mathf.Cos(nodesRange), offset.y, offset.z * Mathf.Sin(nodesRange)); 
            }
            else if (ropeType == RopeType.PATH)
            {
                follower.moveDistanceAlongPath(restLengthPerNode);
                spawnPos = follower.transform.position;
            }

            GameObject temp = Instantiate(ropeNode, spawnPos, new Quaternion());
            nodes.Add(temp.GetComponent<ParticleSpring>());
            nodesParts.Add(temp.GetComponent<SphereCollider>());


            //setup connections
            if (i != 0)
            {
                nodes[i].other.Add(nodes[i - 1].transform);
                nodes[i - 1].other.Add(nodes[i].transform);
            }

        }
        //Connect ends
        nodes[0].other.Add(nodes[numOfNodes - 1].transform);
        nodes[numOfNodes - 1].other.Add(nodes[0].transform);
    }

    void moveRope()
    {
        foreach(ParticleSpring node in nodes)
        {
            Particle3D part = node.GetComponent<Particle3D>();
            if(node.springs.Count > 0 && node.springs[0] != null)
                part.addForce(node.springs[0].transform.right * throttle);
        }
    }
}
