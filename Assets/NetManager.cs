using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    public GameObject fixedSpring;
    public GameObject spring;
    public float detachForce;
    public Vector2Int size;
    public Vector2 spaceing;
    public GameObject[,] netObjects;
    public GameManager gm;
    public bool fixedTopPoints;
    public bool fixedBottomPoints;
    
    // Start is called before the first frame update
    void Awake()
    {
        netObjects = new GameObject[size.x,size.y];
        for(int x = 0; x < size.x; x++)
        {
            for (int y =0; y < size.y; y++)
            {
                GameObject obj = null;
                if(x == 0 && y == size.y - 1 || x == size.x - 1 && y == size.y -1 || x == size.x - 1 && y == 0 || x == 0 && y == 0)
                    if(fixedBottomPoints)
                        obj = spawnSpring(fixedSpring, this.transform.localToWorldMatrix * (this.transform.localPosition + new Vector3(x * spaceing.x, y * spaceing.y, 0.0f)));
                    else if(fixedTopPoints)
                        obj = spawnSpring(fixedSpring, this.transform.localToWorldMatrix * (this.transform.localPosition + new Vector3(x * spaceing.x, y * spaceing.y, 0.0f)));
                    else
                        obj = spawnSpring(spring,  this.transform.localToWorldMatrix*(this.transform.localPosition + new Vector3(x * spaceing.x, y * spaceing.y, 0.0f)));
                else
                    obj = spawnSpring(spring,  this.transform.localToWorldMatrix*(this.transform.localPosition + new Vector3(x * spaceing.x, y * spaceing.y, 0.0f)));

                if (obj == null) continue;
                netObjects[x, y] = obj;
            }
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                //set up connections
                ParticleSpring pSpring = netObjects[x,y].GetComponent<ParticleSpring>();
                //Find Tranform
                if (x != 0)
                {
                    pSpring.other.Add(netObjects[x - 1, y].transform);
                }
                if (y != 0)
                {
                    pSpring.other.Add(netObjects[x, y - 1].transform);
                }
                if (x != size.x -1)
                {
                    pSpring.other.Add(netObjects[x + 1, y].transform);
                }
                if (y != size.y -1)
                {
                    pSpring.other.Add(netObjects[x, y + 1].transform);
                }

                //pSpring.setupVisual();

                netObjects[x, y].GetComponent<Particle3D>();

              
            }
        }
    }

    GameObject spawnSpring(GameObject spr, Vector3 pos)
    {
        GameObject tmp = Instantiate(spr, pos, new Quaternion());
        Particle3D tmpPart = tmp.GetComponent<Particle3D>();

        

        
        gm.particles.Add(tmpPart);
        return tmp;
    }
}
