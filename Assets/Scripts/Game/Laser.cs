using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 3.5f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    public bool IsEnemyLaser {
        get { return _isEnemyLaser; }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement() {
        transform.Translate((IsEnemyLaser ? Vector3.down : Vector3.up) * Time.deltaTime * _laserSpeed);
        DestroyIfOutOfBounds();
    }

    private void DestroyIfOutOfBounds() {
        if (!IsEnemyLaser && transform.position.y > 8) {
            Destroy(gameObject);
        }
        else if (IsEnemyLaser && transform.position.y < -8) {
            Destroy(gameObject);
        }
    }    
}
