using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public GameManager gm;
    public List<GameObject> projectiles;
    public List<float> projectileForce;
    public int projectileIndex;
    public float rotationSpeed;
    private float holdMultiple = 0;
    public bool holdToIncreaseProjectileSpeed;
    public float precentOfXY = .2f;
    public Transform barrelPoint;
    public Text weaponText;
    private string defaultWeaponText;
    public Slider changeBar;
    [Header("Spring Projectile Starting settings")]
    public float forceOfSecondProjectileSpring = 500.0f;
    public int alternateProjectileIndex;
    public Transform fixedSpringPoint;
    public GameObject mousePoint;
    public bool mouseControled;


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        initText();
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.scaledPixelWidth * precentOfXY, Camera.main.scaledPixelHeight * precentOfXY, Camera.main.nearClipPlane));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        transform.position = new Vector3();
    }

    void initText()
    {
        defaultWeaponText = weaponText.text;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        updateUI();
    }

    GameObject spawnParticale()
    {
        GameObject tmp = Instantiate(projectiles[projectileIndex], barrelPoint.position, new Quaternion());
        Particle3D tmpPart = tmp.GetComponent<Particle3D>();
        tmpPart.scalar += holdMultiple;
        tmpPart.setDirection(transform.up);
        tmpPart.addForce(tmpPart.getDirection() * projectileForce[projectileIndex]);
        if (projectileIndex == (int)Particle3D.ProjectileType.SPRING_FIXED)
        {
            tmp.GetComponent<ParticleSpring>().other.Add( fixedSpringPoint);

        }
        gm.particles.Add(tmpPart);
        return tmp;
    }

    void checkInput()
    {
        if (mouseControled)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            float distance = 0;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                mouseWorldPos = hit.point;
                distance = hit.distance;
                //Debug.Log(distance);
                distance = 1 / distance;

            }
            // mouseWorldPos.z = distance;
            mousePoint.transform.position = mouseWorldPos;// * 75.0f;
            //Debug.Log(mouseWorldPos);
            transform.up = (mouseWorldPos - transform.position);

        }
        else
        {
            //Rotation controls
            if (Input.GetKey(KeyCode.Alpha1))
            {
                transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f) * rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                transform.Rotate(new Vector3(0.0f, 0.0f, -1.0f) * rotationSpeed * Time.deltaTime);
            }

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            projectileIndex++;
            if (projectileIndex >= projectiles.Count)
            {
                projectileIndex = 0;
            }
        }
        if (holdToIncreaseProjectileSpeed && Input.GetKey(KeyCode.Return))
        {
            holdMultiple += .1f;
            //if (projectileIndex == (int)Particle3D.ProjectileType.LAZER)
            //{
            //    spawnParticale();
            //}
            
        }
        else if (Input.GetKeyUp(KeyCode.Return))
        {
            if (projectileIndex == (int)Particle3D.ProjectileType.SPRING)
            {
                //Spawn Spring
                GameObject tmp1 = spawnParticale();
                int tmp = projectileIndex;

                //Spawn alternate
                projectileIndex = alternateProjectileIndex;
                GameObject tmp2 = spawnParticale();

                //Attach spring to alternate
                tmp1.GetComponent<ParticleSpring>().other.Add( tmp2.transform);
                if (projectileIndex == (int)Particle3D.ProjectileType.SPRING)
                {
                    tmp2.GetComponent<ParticleSpring>().other.Add(tmp1.transform);
                    tmp2.GetComponent<Particle3D>().addForce(transform.right * forceOfSecondProjectileSpring);
                }
                projectileIndex = tmp;
            } 
            else
            {
                spawnParticale();
                
            }
            holdMultiple = 0;

        }
    }

    void updateText()
    {
        weaponText.text = defaultWeaponText + projectiles[projectileIndex].name;
    }

    void updateBar()
    {
        changeBar.value = holdMultiple * 1;
    }

    void updateUI()
    {
        updateText();
        updateBar();
    }
}
