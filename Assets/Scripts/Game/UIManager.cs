using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;

    private Color[] ammoTextColors;

    [SerializeField]
    private Sprite[] _lifeSprites;

    [SerializeField]
    private Image _lifeImage;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartGameText;

    private float _blinkerTimer = .5f;

    private GameManager _gameManager;

    [SerializeField]
    private Slider _thrustSlider;

    [SerializeField]
    private Text _thrustText;

    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _camera = Camera.main;

        InitAmmoTextColors();

        if (_scoreText == null) {
            Debug.Log("ScoreText component has not been set.  Please set the Score Text component and try again.");
        }

        if (_ammoText == null) {
            Debug.Log("AmmoText component has not been set.  Please set the Ammo Text component and try again.");
        }

        if (_gameOverText == null) {
            Debug.Log("Game Over Text not found.  Please check and try again.");
        }

        if (_restartGameText == null) {
            Debug.Log("Restart Game Text not found.  Please check and try again.");
        }

        if (_gameManager == null) {
            Debug.Log("Game Manager not found.  Please check and try again.");
        }

        if (_thrustSlider == null) {
            Debug.Log("Thrust Slider not found.  Please check and try again.");
        }

        if (_thrustText == null) {
            Debug.Log("Thrust Text not found.  Please check and try again.");
        }
    }

    private void InitAmmoTextColors() {
        ammoTextColors = new Color [] { Color.white, Color.magenta, Color.red};
    }

    private enum AmmoLevels {
        Good,
        Warning,
        Bad
    }

    public void UpdateScore(int score) {
        _scoreText.text = score.ToString();
    }

    public void UpdateAmmo(int ammoCount) {
        _ammoText.text = ammoCount.ToString();
        if (ammoCount > 5) {
            _ammoText.color = ammoTextColors[(int)AmmoLevels.Good];
        }
        else if (ammoCount > 0) {
            _ammoText.color = ammoTextColors[(int)AmmoLevels.Warning];
        }
        else {
            _ammoText.color = ammoTextColors[(int)AmmoLevels.Bad];
        }
    }

    public void UpdateLives(int life) {
        if (life < 0 || life > 3) { return; }
        _lifeImage.sprite = _lifeSprites[life];
    }

    public void FlickerText() {
        EnableGameOverText(true);
        StartCoroutine(FlickerTextRoutine());
    }

    private void EnableGameOverText(bool enabled) {
        _gameOverText.gameObject.SetActive(enabled);
    }

    public void EnableRestartText(bool enabled)
    {
        _restartGameText.gameObject.SetActive(enabled);
    }

    public void GameOver() {
        FlickerText();
        EnableRestartText(true);
        _gameManager.IsGameOver = true;
    }

    IEnumerator FlickerTextRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_blinkerTimer);
            EnableGameOverText(!_gameOverText.gameObject.activeInHierarchy);
        }
    }

    public void UpdateThrustComponent(float thrustValue) {
        _thrustSlider.value = thrustValue;
        _thrustText.text = (thrustValue > 0 ? thrustValue : 0f).ToString();
    }

    public void ShakeCamera() {
        StartCoroutine(ShakeCameraCoRoutine());
    }

    private IEnumerator ShakeCameraCoRoutine()
    {
        Vector3 startingLocation = _camera.transform.position;
        _camera.transform.Translate(Vector3.up * 1);
        yield return new WaitForSeconds(.05f);
        _camera.transform.Translate(Vector3.left * 1);
        yield return new WaitForSeconds(.05f);
        _camera.transform.Translate(Vector3.right * 1);
        yield return new WaitForSeconds(.05f);
        _camera.transform.Translate(Vector3.down * 1);
    }


}
