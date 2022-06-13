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
    [SerializeField]
    private GameObject[] _rarePowerUpPrefabs;

    [SerializeField]
    private float commonEnemySpawnMin = 0.5f;
    [SerializeField]
    private float commonEnemySpawnMax = 3f;
    [SerializeField]
    private float commonPowerUpSpawnMin = 1.5f;
    [SerializeField]
    private float commonPowerUpSpawnMax = 8f;
    [SerializeField]
    private float rarePowerUpSpawnMin = 10f;
    [SerializeField]
    private float rarePowerUpSpawnMax = 15f;

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
                Debug.Log("You have not selected a PowerUpPrefab for slot number " + i + ". Please supply an object and try again.");
            }
        }
        for (int i = 0; i < _rarePowerUpPrefabs.Length; i++)
        {
            if (_rarePowerUpPrefabs[i] == null)
            {
                Debug.Log("You have not selected a RarePowerUpPrefab for slot number " + i + ". Please supply an object and try again.");
            }
        }
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyAndWait());
        StartCoroutine(SpawnPowerUps());
        StartCoroutine(RareSpawner());
    }

    IEnumerator SpawnEnemyAndWait()
    {
        yield return new WaitForSeconds(Random.Range(commonEnemySpawnMin, commonEnemySpawnMax));
        while (_isAlive)
        {
            Instantiate(_enemyPrefab,_newParentContainer.transform);
            yield return new WaitForSeconds(Random.Range(commonEnemySpawnMin, commonEnemySpawnMax));        
        }
    }

    IEnumerator SpawnPowerUps() {
        yield return new WaitForSeconds(Random.Range(commonPowerUpSpawnMin, commonPowerUpSpawnMax));
        while (_isAlive)
        {
            Vector3 spawnLocation = new Vector3(Random.Range(-8f, 8f), Random.Range(8f, 0f), 0f);
            Instantiate(_powerUpPrefabs[Random.Range(0,_powerUpPrefabs.Length)], spawnLocation, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(commonPowerUpSpawnMin, commonPowerUpSpawnMax));            
        }
    }

    IEnumerator RareSpawner() {
        yield return new WaitForSeconds(Random.Range(rarePowerUpSpawnMin, rarePowerUpSpawnMax));
        while (_isAlive) {
            Vector3 spawnLocation = new Vector3(Random.Range(-8f, 8f), Random.Range(8f, 0f), 0f);
            Instantiate(_rarePowerUpPrefabs[Random.Range(0, _rarePowerUpPrefabs.Length)], spawnLocation, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(rarePowerUpSpawnMin, rarePowerUpSpawnMax));
        }
    }
}