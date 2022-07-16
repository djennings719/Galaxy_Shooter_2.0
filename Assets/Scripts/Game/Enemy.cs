using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 1f;

    private float _minSpeed = 0.5f;
    private float _maxSpeed = 4.5f;

    private float _maxSpawnX = 9.5f;

    private float _maxSpawnY = 7.25f;
    private float _minSpawnY = 0f;

    private float _x = 1f;
    private float _y = 1f;
    private float _z = 0f;

    private float _minYValue = -6f;

    private Player _player;

    private Animator _animator;

    private float _deathAnimationTime = 2.8f;

    private AudioSource _explosionSound;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _laserOffset;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _explosionSound = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();


        if (_player == null) {
            Debug.Log("Player object not found. Please check and try again.");
        }
        if (_animator == null) {
            Debug.Log("Animator object not found.  Please check and try again.");
        }
        if (_explosionSound == null) {
            Debug.Log("Explosion sound not found. Please try again.");
        }
        if (_laserPrefab == null) {
            Debug.Log("Laser Prefab not found.  Please try again.");
        }

        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void SetRandomSpeed() {
        _speed = Random.Range(_minSpeed, _maxSpeed);
    }

    private void SetRandomLocation() {
        _x = Random.Range(-_maxSpawnX, _maxSpawnX);
        _y = Random.Range(_minSpawnY, _maxSpawnY);
        transform.position = new Vector3(_x, _y, _z);
    }

    private void Movement() {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        CheckBounds();
    }

    private void CheckBounds() {
        if (transform.position.y < _minYValue) {
            Init();
        }
    }

    private void Init()
    {
        SetRandomSpeed();
        SetRandomLocation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {

            Player player = other.GetComponent<Player>();
            if (player != null) {
                player.DamagePlayer();
                player.UpdateScore();
                player.KillEnemy();
            }
            WaitAndDestroyEnemy();
        }
        else if (other.tag == "Laser") {
            Laser laser = other.GetComponent<Laser>();
            if (laser.IsEnemyLaser)
            {
                return;
            }
            Destroy(other.gameObject);
           
            if (_player != null)
            {
                _player.UpdateScore();
                _player.KillEnemy();
            }
            WaitAndDestroyEnemy();
        }
    }

    private void WaitAndDestroyEnemy() {
        Destroy(GetComponent<Collider2D>());
        _speed = 0.4f;
        _animator.SetTrigger("OnEnemyDeath");
        _explosionSound.Play();
        Destroy(gameObject, _deathAnimationTime);
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            Vector3 offset = new Vector3(0, _laserOffset, 0);
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

}
