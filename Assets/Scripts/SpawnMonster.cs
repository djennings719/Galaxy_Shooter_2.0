using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnMe;
    // Start is called before the first frame update
    void Start()
    {
        if (spawnMe == null) {
            Debug.Log("Spawn instance has not been selected.  Please select a spawn instance before continuing.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = Random.Range(-10, 10f);
        float y = Random.Range(0.2f, 100f);
        float z = Random.Range(-5f, 10f);
        Instantiate(spawnMe, new Vector3(x,y,z), Quaternion.identity);
    }
}
