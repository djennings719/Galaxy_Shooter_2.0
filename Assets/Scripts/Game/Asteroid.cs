using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
        if (_explosionPrefab == null) {
            Debug.Log("Explosion Prefab has not been set.  Please set the GameObject and try again.");
        }

        if (_spawnManager == null) {
            Debug.Log("SpawnManager not found.  Please try again.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate() {
        transform.Rotate(new Vector3(0f, 0f, 12.5f) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartRound();
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.1f);
        }    
    }
}
