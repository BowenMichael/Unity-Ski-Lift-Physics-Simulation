using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    Vector2 startingPos;
    Vector3 startingWorldPos;
    Vector2 endingPos;
    public Text scoreText;
    public Text output;
    private string outputDesc;
    private int score;
    private string scoreDesc;
    public List<Particle3D> particles;
    [Range(0.0f,10.0f)]
    public float intensity;
    public int spawnNumberOfParticles = 10;
    public GameObject smallParticle;
    private CollisionManager cm;
    private BSPManager bspm;
    public float randomSpeed = 2.0f;
    public bool paused = false;
    public int numberOfBoxesToSpawn = 5;
    public GameObject imovableBox;


    //public GameObject premadeSphere;//uncomment for falling net scene

    private int activeCollisionIndex=0;

    //Planes
    public List<Vector4> planes = new List<Vector4>();
    public Mesh planeMesh;
    public Material planeMat;
    public float planeDistanceFromOrigin;
    public Vector3 mScreenSize;
    public float wallDepth = 20.0f;

    private void Awake()
    {
        Vector3 screenPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
        Vector3 screenSize = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.scaledPixelWidth, Camera.main.scaledPixelWidth, Camera.main.nearClipPlane));
        screenSize *= planeDistanceFromOrigin;
        mScreenSize = screenSize;
    }

    private void Start()
    {
        bspm = GetComponent<BSPManager>();
        cm = GetComponent<CollisionManager>();
        scoreDesc = scoreText.text;
        outputDesc = output.text;
        for(int i = 0; i < spawnNumberOfParticles; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-mScreenSize.x, mScreenSize.x), Random.Range(-mScreenSize.y, mScreenSize.y), Random.Range(-20.0f, 20.0f));
            GameObject temp = Instantiate(smallParticle, randomPosition, new Quaternion());
            Particle3D part = temp.GetComponent<Particle3D>();
            part.velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * randomSpeed;
            particles.Add(part);

        }

        //Premade shere to simulate the net falling on sphere
        //particles.Add(premadeSphere.GetComponent<Particle3D>());

        //Planes

        Debug.Log(mScreenSize + "; ");
        planes.Add(new Vector4(0.0f, 1.0f, 0.0f, -mScreenSize.y)); //Top Wall
        planes.Add(new Vector4(0.0f, -1.0f, 0.0f, -mScreenSize.y)); //Bottom Wall
        planes.Add(new Vector4(1.0f, 0.0f, 0.0f, -mScreenSize.x)); //Right Wall
        planes.Add(new Vector4(-1.0f, 0.0f, 0.0f, -mScreenSize.x)); //Left Wall
        planes.Add(new Vector4(0.0f, 0.0f, 1.0f, -wallDepth));
        planes.Add(new Vector4(0.0f, 0.0f, -1.0f, -wallDepth));
        int j = 0;

        foreach (Vector4 plane in planes)
        {
            GameObject wall = new GameObject("Plane" + j++, typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider));
            wall.tag = "plane";
            Vector3 normal = new Vector3(plane.x, plane.y, plane.z);
            wall.transform.position = new Vector3(plane.x, plane.y, plane.z) * plane.w;
            wall.transform.up = normal;
            wall.transform.localScale *= 200;
            wall.GetComponent<MeshFilter>().mesh = planeMesh;
            wall.GetComponent<MeshRenderer>().material = planeMat;
            wall.layer = 9;
            wall.GetComponent<MeshCollider>().sharedMesh = planeMesh;
        }

        spawnBoxes();
    }

    public void spawnBoxes()
    {
        for(int i = 0; i < numberOfBoxesToSpawn; i++)
        {
           GameObject tmp = Instantiate(imovableBox, 
                new Vector3(Random.Range(-mScreenSize.x, mScreenSize.x), 
                            Random.Range(-mScreenSize.y, mScreenSize.y), 
                            Random.Range(0, wallDepth)),
                new Quaternion());
            particles.Add(tmp.GetComponent<Particle3D>());

        }
    }

    public bool checkWithingWalls(Transform trans)
    {
        return trans.position.y < planes[0].w ||
               trans.position.y > -planes[1].w ||
               trans.position.x < planes[2].w ||
               trans.position.x > -planes[3].w ||
               trans.position.z < planes[4].w ||
               trans.position.z > -planes[5].w;

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            startingPos = Input.mousePosition;
            startingWorldPos = Camera.main.ScreenToWorldPoint(startingPos);
            
            if (Input.GetMouseButton(0))
            { 
                 addForceToAllParticles(intensity, startingWorldPos);
            }
            else
            {
                addForceToAllParticles(-intensity, startingWorldPos);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach(Particle3D part in particles)
            {
                if(part != null)
                    GameObject.Destroy(part.gameObject);
            }
            particles.Clear();
            spawnBoxes();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (paused)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
            paused = !paused;

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            activeCollisionIndex++;
            if (activeCollisionIndex == 2) activeCollisionIndex = 0; //2 is the max number of collision managers on game managaer

            cm.active = false;
            bspm.active = false;
            
        }

        if (activeCollisionIndex == 0)
        {
            cm.active = true;
        }
        else if (activeCollisionIndex == 1)
        {
            bspm.active = true;
        }

        scoreText.text = scoreDesc + score;
        output.text = outputDesc + "\nPaused: " + paused + "\nCollision Checks(0,1): " + cm.collisionChecks +", "+bspm.bspCollisionCount + "\nActive Collision Index(0=normal, 1=BSP): " + activeCollisionIndex;
    }

    void addForceToAllParticles(float intensity, Vector3 target)
    {
        foreach(Particle3D part in particles)
        {
            if (part != null)
            {
                Vector2 diff = target - part.transform.position;
                part.addForce(diff * intensity);
            }
        }
    }

    public void incrementScore(int increment)
    {
        score++;

    }
}
