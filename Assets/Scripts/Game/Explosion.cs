using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        _explosionSound = GetComponent<AudioSource>();
        if (_explosionSound == null)
        {
            Debug.Log("Explosion sound not found.  Please try again.");
        }
        else {
            _explosionSound.Play();
        }
        Destroy(gameObject, 2.8f);   
    }
}
