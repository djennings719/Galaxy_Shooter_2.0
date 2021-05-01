using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotLaser : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        CheckForDestroy();
    }

    private void CheckForDestroy()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
