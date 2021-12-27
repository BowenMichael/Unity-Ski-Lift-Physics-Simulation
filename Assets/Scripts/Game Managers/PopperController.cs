using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperController : MonoBehaviour
{
    public int numberOfPoppers = 5;
    public GameObject self;
    private Particle3D selfParticle;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        selfParticle = GetComponent<Particle3D>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 selfDirection = selfParticle.getDirection();
            float multiple =  (numberOfPoppers - 1) / (Mathf.PI);
            float theta = 0;
            for(int i = 0; i < numberOfPoppers; i++)
            {
                if (numberOfPoppers > 19)
                {
                    break;
                }
                else
                {
                    GameObject tmp = Instantiate(self, transform.position, new Quaternion());
                    Particle3D tmpPart = tmp.GetComponent<Particle3D>();
                    theta += multiple;
                    tmpPart.setDirection(new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)));
                    tmp.GetComponent<PopperController>().numberOfPoppers = numberOfPoppers * 2;
                    tmp.transform.localScale *= 1.0f / numberOfPoppers;
                    gm.particles.Add(tmpPart);

                }
            }
            Destroy(gameObject);
        }
    }
}
