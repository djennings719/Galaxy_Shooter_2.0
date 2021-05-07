using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _spawnMe;

    enum CountIds { Capsules, Cubes, Spheres}

    private int[] counts;

    private SpawnMonsterUIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<SpawnMonsterUIManager>();
        counts = new int[3];

        if (_spawnMe == null) {
            Debug.Log("Spawn instance has not been selected.  Please select a spawn instance before continuing.");
        }
        if (_uiManager == null)
        {
            Debug.Log("UIManager instance not found.  Please try again.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = Random.Range(-10, 10f);
        float y = Random.Range(0.2f, 100f);
        float z = Random.Range(-5f, 10f);
        int id = Random.Range(0, 3);
        Instantiate(_spawnMe[id], new Vector3(x,y,z), Quaternion.identity);
        counts[id]++;
        _uiManager.UpdateCount(id, counts[id]);
    }
}
