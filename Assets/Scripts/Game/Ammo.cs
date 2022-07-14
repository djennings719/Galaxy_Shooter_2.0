using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private int _ammoCount;

    private const int AmmoAmount = 15;

    private void Start()
    {
        Reset();
    }

    public int AmmoCount {
        get { return _ammoCount; }
        set { _ammoCount = value; }
    }

    public bool DoesPlayerHaveAmmo {
        get { return _ammoCount > 0; }
        private set { }
    }

    public void UpdateAmmoCount() {
        _ammoCount--;        
    }

    public void Reset()
    {
        _ammoCount = AmmoAmount;
    }
}
