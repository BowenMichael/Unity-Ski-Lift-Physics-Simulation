using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpring : ForceGenerator
{
    public float springConstant;
    public float dampingConstant;
    public float restLength;
    public List<Transform> other = new List<Transform>(); // needs to interate through the list to apply netfroce
    public List<GameObject> springs = new List<GameObject>(); // needs to interate through the list to apply netfroce
    public GameObject springVisual;
    public float breakForce = 1; //Needs to be implemented

    public void setupVisual()
    {
        for(int i = 0; i < other.Count; i++)
        {
            Vector3 diff = transform.position + ((other[i].position - transform.position) * .5f);
            GameObject temp = Instantiate(springVisual, diff, new Quaternion());
            temp.transform.localScale = new Vector3(restLength, temp.transform.localScale.y, temp.transform.localScale.z);
            temp.transform.up = (transform.position - other[i].position).normalized;
            springs.Add(temp);
        }
    }

    void updateVisual()
    {
        for(int i = 0; i < springs.Count && i < other.Count; i++)
        {
            Vector3 diff = transform.position + ((other[i].position - transform.position)*.5f);
            GameObject temp = springs[i];
            temp.transform.position = diff;
            temp.transform.localScale = new Vector3(restLength, temp.transform.localScale.y, temp.transform.localScale.z);
            temp.transform.right = (transform.position - other[i].position).normalized;
        }
    }

    protected override void updateForce(Particle3D part)
    {
        if (other.Count > 0 && springs.Count <= 0)
            setupVisual();

        if(springs.Count > 0)
            updateVisual();

        Vector3 netForce = new Vector3();//should add this

        for (int i = 0; i < other.Count; i++)
        {
            //Should be for loop
            if (other == null)
                return;
            Vector3 force;
            force = part.transform.position;
            force -= other[i].position;

            //Magnitude of force

            float magnitude = force.magnitude; //sqr root function?
            magnitude = Mathf.Abs(magnitude - restLength); //distance compared to rest length
            magnitude *= springConstant; //multiplyed by the springyness
            float dampingManitude = magnitude * dampingConstant;

            if(magnitude >= breakForce)
            {
                foreach(Transform t in other)
                {
                    //int index = t.gameObject.GetComponent<ParticleSpring>().other.FindIndex(gameObject.transform);
                    if(t.gameObject.TryGetComponent(out ParticleSpring ps))
                        ps.findAndBreak(this.transform);
                   
                    //t.gameObject.GetComponent<ParticleSpring>().other.Remove(this.transform);
                }
                //other[i].GetComponent<ParticleSpring>().breakAtIndex(i);

                breakAtIndex(i);
                //Destroy(this.gameObject);

            }

            //normalize force times magnitude
            force = force.normalized;
            force *= -magnitude + dampingManitude; // why negative?
            
            netForce += force;

        }

        part.addForce(netForce);
    }

    public void breakAtIndex(int i)
    {
        //if (i >=0 && i < other.Count || i >= 0 && i < springs.Count) return;
        Debug.Log("Break");
        other.RemoveAt(i);
        if (springs.Count < 0) return;
        Destroy(springs[i]);
        springs.RemoveAt(i);
        


    }

    public void findAndBreak(Transform t)
    {
        for(int i = 0; i < other.Count; i++)
        {
            if(t == other[i])
            {
                breakAtIndex(i);
            }
        }
    }

    public void breakAll()
    {
        for(int i = 0; other.Count > 0; i++)
        {
            breakAtIndex(0);
        }
        other.Clear();
        springs.Clear();
    }
}
