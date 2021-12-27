using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteOffScreen : MonoBehaviour
{

    private Vector2 screenSizeInPixels;
    private Vector3 worldSpaceScreenSizeTopRight;
    private Vector3 worldSpaceScreenSizeBottomLeft;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        screenSizeInPixels = new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
        worldSpaceScreenSizeTopRight = Camera.main.ViewportToWorldPoint(screenSizeInPixels);
        worldSpaceScreenSizeTopRight = new Vector3(worldSpaceScreenSizeTopRight.x, worldSpaceScreenSizeTopRight.y, 0.0f);
        worldSpaceScreenSizeBottomLeft = Camera.main.ViewportToWorldPoint(new Vector2());
        worldSpaceScreenSizeBottomLeft = new Vector3(worldSpaceScreenSizeBottomLeft.x, worldSpaceScreenSizeBottomLeft.y, 0.0f);
        worldSpaceScreenSizeTopRight += new Vector3(1.0f, 1.0f, 0.0f);
        worldSpaceScreenSizeBottomLeft -= new Vector3(1.0f, 1.0f, 0.0f);
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(worldSpaceScreenSizeTopRight.x < transform.position.x ||
            transform.position.x < worldSpaceScreenSizeBottomLeft.x || 
            transform.position.y > worldSpaceScreenSizeTopRight.y || 
            transform.position.y < worldSpaceScreenSizeBottomLeft.y)
        {
            gm.particles.Remove(gameObject.GetComponent<Particle3D>());
            Destroy(gameObject);

        }
    }
}
