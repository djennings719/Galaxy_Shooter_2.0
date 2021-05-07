using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 3.5f;

    // Update is called once per frame
    void Update()
    {
        Movement();

    }

    private void Movement() {
        transform.Translate(Vector3.up * Time.deltaTime * _laserSpeed);
        DestroyIfOutOfBounds();
    }

    private void DestroyIfOutOfBounds() {
        if (transform.position.y > 8) {
            Destroy(gameObject);
        }
    }


}
