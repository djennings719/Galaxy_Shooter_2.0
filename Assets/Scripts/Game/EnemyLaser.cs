using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DestroyUtil.CheckForDestroy(gameObject);        
    }
}
