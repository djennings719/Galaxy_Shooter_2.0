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
    private GameObject _laserPrefab;

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
    [SerializeField]
    private float _speedBoost = 3.0f;

    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotEnabled = false;
    [SerializeField]
    private bool _isSpeedBoostEnabled = false;
    private bool _isShieldEnabled = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shield;

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
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

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

    private void BoosterCheck() {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _boosterAdjustment = 3.0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _boosterAdjustment = 1f;
        }
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
        _canFire = Time.time + _fireRate;
        Vector3 offsetPosition = new Vector3(0, _laserOffset, 0);
        Instantiate(_isTripleShotEnabled ? _tripleShotPrefab : _laserPrefab, transform.position + offsetPosition, Quaternion.identity);
        _audioSource.clip = _laserSound;
        _audioSource.Play();
    }

    public void DamagePlayer() {
        //simple single use shield
        if (_isShieldEnabled) {
            SetShieldEnabled(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        SetDamage();
        CheckLives();
    }

    private void SetDamage() {
        if (_lives == 2)
        {
            _leftDamage.SetActive(true);
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

    public void SetPowerUp(PowerUp.PowerUpTags powerUp) {

        switch (powerUp) {
            case PowerUp.PowerUpTags.TripleShotPowerUp:
                _isTripleShotEnabled = true;
                StartCoroutine(TripleShotPowerDown());
                break;
            case PowerUp.PowerUpTags.SpeedBoostPowerUp:
                _isSpeedBoostEnabled = true;
                StartCoroutine(SpeedBoostPowerDown());
                break;
            case PowerUp.PowerUpTags.ShieldsPowerUp:
                SetShieldEnabled(true);
                StartCoroutine(ShieldPowerDown());
                break;
            default:
                break;
        }
    }

    IEnumerator TripleShotPowerDown() {
        yield return new WaitForSeconds(Random.Range(1.2f, 5.5f));        
        _isTripleShotEnabled = false;
    }

    IEnumerator SpeedBoostPowerDown() {
        yield return new WaitForSeconds(Random.Range(1.2f, 5.5f));
        _isSpeedBoostEnabled = false;
    }

    IEnumerator ShieldPowerDown() {
        yield return new WaitForSeconds(Random.Range(1.2f, 5.5f));
        SetShieldEnabled(false);
    }

    private void SetShieldEnabled(bool isEnabled) {
        _isShieldEnabled = isEnabled;
        _shield.SetActive(isEnabled);
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
}
