using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _horizontalSpeed = 1.5f;

    [SerializeField]
    private float _verticalSpeed = 1.5f;

    [SerializeField]
    private GameObject _laser;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    private float _laserOffset = 1.05f;

    private float _playerMaxX = 11.5f;
    private float _playerMinX = -11.5f;

    private float _playerMaxY = 1f;
    private float _playerMinY = -4.5f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    private bool isTripleShotEnabled;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) {
            Debug.Log("SpawnManager not found.");
        }
        SetPlayerStartingLocation();
        if (_laser == null) {
            Debug.Log("Laser GameObject has not been set.  Please set GameObject and try again.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void SetPlayerStartingLocation() {
        //set Player to starting position 
        transform.position = new Vector3(0f, -2f, 0f);
    }

    //Simple Player Movement
    private void Movement() {
        //horizontal input
        float horizontal = Input.GetAxis("Horizontal") * _horizontalSpeed;
        //vertical input 
        float vertical = Input.GetAxis("Vertical") * _verticalSpeed;

        Vector3 translateVector = new Vector3(horizontal, vertical, 0);
        transform.Translate(translateVector * Time.deltaTime);

        BoundaryController();
    }

    //Checks and controls player bounds
    private void BoundaryController() {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _playerMinY, _playerMaxY), 0);

        if (transform.position.x > _playerMaxX)
        {
            transform.position = new Vector3(_playerMinX, transform.position.y, 0);
        }
        else if (transform.position.x < _playerMinX)
        {
            transform.position = new Vector3(_playerMaxX, transform.position.y, 0);
        }
    }

    private void FireLaser() {    
        _canFire = Time.time + _fireRate;
        Vector3 offsetPosition = new Vector3(0, _laserOffset, 0);
        Instantiate(_laser, transform.position + offsetPosition, Quaternion.identity);
    }

    public void DamagePlayer() {
        _lives--;
        //update UI once we have UI components
        CheckLives();
    }

    private void CheckLives() {
        if (_lives == 0) {
            _spawnManager.IsAlive = false;
            Destroy(gameObject);
        }        
    }
}
