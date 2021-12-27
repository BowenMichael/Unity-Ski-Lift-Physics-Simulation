using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    public GameObject ropeNode;
    public int numOfNodes;
  
    public bool isCircle = true;
    public float radius1;

    public List<ParticleSpring> nodes = new List<ParticleSpring>();
    public List<SphereCollider> nodesParts = new List<SphereCollider>();
    public Particle3D center;
    public SphereCollider centerCollider;
    public GameObject groundPlane;
    Plane slope;
    Vector4 plane;

    float restLengthPerNode = 1;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        plane = -new Vector4(groundPlane.transform.up.x, groundPlane.transform.up.y, groundPlane.transform.up.z, 0);
        if (isCircle)
            centerCollider.radius = radius1;
        
        if (ropeNode.TryGetComponent(out ParticleSpring partSpring))
        {
            //set partSpring vars
    
            if (isCircle)
            {
                //radius1 = numOfNodes;
                //Restlength from num of nodes
                float circumfrence =  2 * Mathf.PI * radius1;
                restLengthPerNode = numOfNodes / circumfrence;
                partSpring.restLength = restLengthPerNode;
                offset = new Vector3(radius1, 1, radius1) ;
            }

            //calc numOfNodes


            createRope();
            
            //Create Centera sphere


        }
        else
        {
            Debug.LogError("RopeNode needs Particle Spring Component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(SphereCollider part in nodesParts)
        {
            CollisionFunctions.sphereCylyinderHandler(part, centerCollider);
            CollisionFunctions.spherePlaneColisionHandler(part, plane);
        }
    }

    void createRope()
    {
        for (int i = 0; i < numOfNodes; ++i)
        {
            //populate nodes
            float nodesRange = (2 * (i * 1.0f / numOfNodes) - 1)*Mathf.PI;
            //nodesRange *= radius1;
            GameObject temp = Instantiate(ropeNode, transform.position + new Vector3 (offset.x * Mathf.Cos(nodesRange), offset.y , offset.z * Mathf.Sin(nodesRange)), new Quaternion());
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
}
