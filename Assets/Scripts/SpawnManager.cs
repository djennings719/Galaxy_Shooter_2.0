using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _spawnObject;
    [SerializeField]
    private GameObject _newParent;

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
        if (_spawnObject == null) {
            Debug.Log("You have not selected a GameObject to spawn.  Please supply an object and try again.");
        }
        if (_newParent == null) {
            Debug.Log("You have not selected a GameObject to parent.  Please supply an object and try again.");
        }

        StartCoroutine(SpawnAndWait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnAndWait()
    {
        while (_isAlive)
        {
            Instantiate(_spawnObject,_newParent.transform);
            yield return new WaitForSeconds(Random.Range(0.5f, 5f));        
        }
    }
}
