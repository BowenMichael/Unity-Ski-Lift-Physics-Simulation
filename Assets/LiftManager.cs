using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManager : MonoBehaviour
{
    [Header("Lift Objects")]
    public List<GameObject> poles;
    public GameObject basePole;
    public GameObject topPole;
    public GameObject polePrefab;

    [Header("Rope")]
    public RopeCreator rope;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject pole in poles)
        {
            rope.cylinderParts.Add(pole.GetComponent<PoleManager>().Wheels.GetComponent<SphereCollider>());
            if (pole.TryGetComponent(out SphereCollider sphere))
            {
                rope.cylinderParts.Add(sphere);
                
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 findSpotForNewTower(int index)
    {
        if(index == 0)
        {
            //Spawns at the bottom in the direction of the previous poles
            return (poles[0].transform.position + (poles[1].transform.position - poles[0].transform.position).normalized * 2.0f);//2.0f is the multiplier for distance from end pole
        }
        if(index == poles.Count - 1)
        {
            //Spawns at the top in the direction of the previous poles
            return (poles[index].transform.position + (poles[index - 1 ].transform.position - poles[index].transform.position).normalized * 2.0f);//2.0f is the multiplier for distance from end pole

        }
        //spawns in between poles at index
        return poles[index].transform.position + ((poles[index].transform.position -  poles[index - 1].transform.position) * .5f);//2.0f is the multiplier for distance from end pole

    }

    public void instantiateLiftPole(int index)
    {
        Vector3 SpawnPoint = findSpotForNewTower(index);
        //Add a new Tower
        GameObject newPole = Instantiate(polePrefab, SpawnPoint, new Quaternion(), this.transform);
        poles.Insert(index ,newPole);


        //Find where on the rope to add the new nodes
        //rc.path.path.GetClosestPointOnPath(SpawnPoint);
        //Find on the up side

        //Find on the down side

        //Add new nodes to path

        //Reset the rope with new path


    }

    public void instantiateLiftPole()
    {
        instantiateLiftPole(poles.Count / 2);
    }

    /// <summary>
    /// Whenever the poles are moved the rope needs to update
    /// </summary>
    public void updateRope()
    {

    }
}
