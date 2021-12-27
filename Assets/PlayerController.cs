using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Player
    {
        UNKOWN = -1,
        PLAYER1,
        PLAYER2
    }
    public Player role;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (role == Player.PLAYER1)
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection(Vector3.left);
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection(Vector3.right);
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection(Vector3.back);
            }
        }
        else if(role == Player.PLAYER2)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDirection(Vector3.left);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDirection(Vector3.right);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveDirection(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDirection(Vector3.back);
            }
        }
    }

    void moveDirection(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
