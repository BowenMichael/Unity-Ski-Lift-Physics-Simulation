using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public void onCollidableCollision()
    {
        Destroy(this.gameObject);
    }
}
