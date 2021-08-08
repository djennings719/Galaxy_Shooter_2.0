using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _newParentContainer;
    [SerializeField]
    private GameObject[] _powerUpPrefabs;

    private IEnumerator coroutine;

    private bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        set { _isAlive = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_enemyPrefab == null) {
            Debug.Log("You have not selected a GameObject for enemy.  Please supply an object and try again.");
        }
        if (_newParentContainer == null) {
            Debug.Log("You have not selected a GameObject for parent.  Please supply an object and try again.");
        }
        for (int i = 0; i < _powerUpPrefabs.Length; i++) {
            if (_powerUpPrefabs[i] == null) {
                Debug.Log("You have not selected a Prefab for slot number " + i + ". Please supply an object and try again.");
            }
        }
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyAndWait());
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnEnemyAndWait()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        while (_isAlive)
        {
            Instantiate(_enemyPrefab,_newParentContainer.transform);
            yield return new WaitForSeconds(Random.Range(0.5f, 5f));        
        }
    }

    IEnumerator SpawnPowerUps() {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        while (_isAlive)
        {
            Vector3 spawnLocation = new Vector3(Random.Range(-8f, 8f), Random.Range(8f, 0f), 0f);
            Instantiate(_powerUpPrefabs[Random.Range(0,_powerUpPrefabs.Length)], spawnLocation, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(1.5f, 8.0f));            
        }
    }
}