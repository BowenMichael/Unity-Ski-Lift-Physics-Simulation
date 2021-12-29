using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManager : MonoBehaviour
{
    [Header("Lift Objects")]
    public List<GameObject> poles;
    public GameObject basePole;
    public GameObject topPole;

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
}
