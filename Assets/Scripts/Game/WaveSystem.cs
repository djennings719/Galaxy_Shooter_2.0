using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private int _enemiesPerRound;

    private int _round;

    public int Round {
        get { return _round; }
        private set { }
    }

    private int _currentEnemies;

    public int CurrentEnemies {
        get { return _currentEnemies; }
        private set { }
    }

    private int _deadEnemies;

    public int DeadEnemies {
        get { return _deadEnemies; }
        private set { }
    }

    private int _maxEnemiesThisRound;

    public int MaxEnemiesThisRound {
        get { return _maxEnemiesThisRound; }
        private set { }
    }
    
    public bool IsRoundOver {
        get { return _deadEnemies == _maxEnemiesThisRound; }
    }

    public void StartRound() {
        _currentEnemies = 0;
        _deadEnemies = 0;
        _maxEnemiesThisRound = ++_round * _enemiesPerRound;
    }

    public void SpawnEnemy() {
        _currentEnemies++;
    }

    public void KillEnemy() {
        _deadEnemies++;
    }

    public bool CanSpawn(){
        return _currentEnemies < _maxEnemiesThisRound;
    }
}
