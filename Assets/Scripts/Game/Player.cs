using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _horizontalSpeed = 1.5f;

    [SerializeField]
    private float _verticalSpeed = 1.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    private float _canThrustBoost = 10f;
    private float _thrustBoostIncrement = .1f;
    private bool _refreshingBoost = false;

    private float _laserOffset = 1.05f;

    private float _playerMaxX = 11.5f;
    private float _playerMinX = -11.5f;

    private float _playerMaxY = 1f;
    private float _playerMinY = -4.5f;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speedBoost = 3.0f;

    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotEnabled = false;
    [SerializeField]
    private bool _isSpeedBoostEnabled = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shield;

    private Shield _theShield;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _leftDamage;

    [SerializeField]
    private GameObject _rightDamage;

    [SerializeField]
    private AudioClip _laserSound;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private float _boosterAdjustment = 1f;

    private Ammo _ammo;

    [SerializeField]
    private GameObject _multiDirectionalLaserPrefab;

    [SerializeField]
    private bool _isMultiDirectionalLaserEnabled;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _ammo = GetComponent<Ammo>();

        if (_spawnManager == null) {
            Debug.Log("SpawnManager not found.");
        }
        SetPlayerStartingLocation();
        if (_laserPrefab == null) {
            Debug.Log("Laser GameObject has not been set.  Please set GameObject and try again.");
        }

        if (_tripleShotPrefab == null) {
            Debug.Log("TripleShot GameObject has not been set.  Please set the GameObject and try again.");
        }
        if (_shield == null)
        {
            Debug.Log("Shield GameObject has not been set.  Please set the GameObject and try again.");
        }
        else {
            _theShield = _shield.GetComponent<Shield>();
        }

        if (_uiManager == null) {
            Debug.Log("UI Manager has not been set.  Please set the GameObject and try again.");
        }

        if (_leftDamage == null) {
            Debug.Log("Left damage object has not been set.  Please set and try again.");
        }
        if (_rightDamage == null) {
            Debug.Log("Right damage object has not been set.  Please set and try again.");
        }

        if (_laserSound == null) {
            Debug.Log("Laser sound clip has not been set.  Please set and try again.");
        }
        if (_audioSource == null)
        {
            Debug.Log("AudioSource not found.  Please try again.");
        }
        if (_explosion == null) {
            Debug.Log("Explosion not found.  Please try again.");
        }
        if (_multiDirectionalLaserPrefab == null) {
            Debug.Log("Multi Directional Laser Prefab not found.  Please try again.");
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

    //checks for Left-Shift key and adjusts booster as necessary
    private void BoosterCheck() {
        if (Input.GetKey(KeyCode.LeftShift) && _canThrustBoost > 0f) {
            _boosterAdjustment = 3.0f;
            _canThrustBoost -= _thrustBoostIncrement;
            _uiManager.UpdateThrustComponent(_canThrustBoost);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _boosterAdjustment = 1f;
        }

        if (_canThrustBoost <= 0f && !_refreshingBoost) {
            _refreshingBoost = true;
            _boosterAdjustment = 1f;
            StartCoroutine(RefreshThrustBoost());
        }
    }

    private IEnumerator RefreshThrustBoost() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.2f, 5.5f));
        _uiManager.UpdateThrustComponent(10f);
        _canThrustBoost = 10f;
        _refreshingBoost = false;
    }

    private void SetPlayerStartingLocation() {
        //set Player to starting position 
        transform.position = new Vector3(0f, -2f, 0f);
    }

    //Simple Player Movement
    private void Movement() {

        BoosterCheck();

        //horizontal input
        float horizontal = Input.GetAxis("Horizontal") * _horizontalSpeed;
        //vertical input 
        float vertical = Input.GetAxis("Vertical") * _verticalSpeed;

        Vector3 translateVector = new Vector3(horizontal, vertical, 0);
        transform.Translate(translateVector * Time.deltaTime * (_isSpeedBoostEnabled ? _speedBoost : 1) * _boosterAdjustment);

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
        if (_ammo.DoesPlayerHaveAmmo)
        {
            _canFire = Time.time + _fireRate;
            Vector3 offsetPosition = new Vector3(0, _laserOffset, 0);
            Instantiate(SpawnFinder(), transform.position + offsetPosition, Quaternion.identity);
            _audioSource.clip = _laserSound;
            _audioSource.Play();
            _ammo.UpdateAmmoCount();
            _uiManager.UpdateAmmo(_ammo.AmmoCount);
        }
        else {
            PlayerOutOfAmmo();
        }
    }

    private GameObject SpawnFinder() {
        if (_isTripleShotEnabled) {
            return _tripleShotPrefab;
        }
        else if (_isMultiDirectionalLaserEnabled) {
            return _multiDirectionalLaserPrefab;
        }
        return _laserPrefab;
    }

    private void PlayerOutOfAmmo() {
        EditorApplication.Beep();
        //TODO: add UI component to announce out of ammo
    }

    public void DamagePlayer()
    {
        //simple single use shield
        if (_theShield.IsShieldEnabled)
        {
            _theShield.SetShieldDamage();
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        SetPlayerDamage();
        CheckLives();
        _uiManager.ShakeCamera();
    }

    private void SetPlayerDamage() {
        if (_lives > 2)
        {
            _leftDamage.SetActive(false);
        }
        else if (_lives == 2)
        {
            _leftDamage.SetActive(true);
            _rightDamage.SetActive(false);
        }
        else if (_lives == 1)
        {
            _rightDamage.SetActive(true);
        }
    }

    private void CheckLives() {
        if (_lives == 0) {
            _spawnManager.IsAlive = false;
            _uiManager.GameOver();
            Destroy(gameObject);
        }
    }

    IEnumerator TripleShotPowerDown() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.2f, 5.5f));
        _isTripleShotEnabled = false;
    }

    IEnumerator SpeedBoostPowerDown() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.2f, 5.5f));
        _isSpeedBoostEnabled = false;
    }

    IEnumerator MultiDirectionalShotPowerDown() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.2f, 5.5f));
        _isMultiDirectionalLaserEnabled = false;
    }

    public void UpdateScore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Laser laser = other.GetComponent<Laser>();
            if (laser != null && laser.IsEnemyLaser)
            {
                DamagePlayer();
            }
        }
    }

    public void CollectHealth() {
        if (_lives < 3) {
            _lives++;
            _uiManager.UpdateLives(_lives);
            SetPlayerDamage();
        }
    }

    public void CollectAmmo() {
        _ammo.Reset();
        _uiManager.UpdateAmmo(_ammo.AmmoCount);
    }

    public void CollectShield() {
        _theShield.IsShieldEnabled = true;
    }

    public void CollectSpeed() {
        _isSpeedBoostEnabled = true;
        StartCoroutine(SpeedBoostPowerDown());
    }

    public void CollectTripleShot() {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDown());
    }

    public void CollectMultiDirection()
    {
        _isMultiDirectionalLaserEnabled = true;
        StartCoroutine(MultiDirectionalShotPowerDown());
    }
}
