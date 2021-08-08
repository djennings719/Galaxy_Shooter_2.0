using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private int _ammoCount = 15;

    public int AmmoCount {
        get { return _ammoCount; }
        private set { }
    }

    public bool DoesPlayerHaveAmmo {
        get { return _ammoCount > 0; }
        private set { }
    }

    public void UpdateAmmoCount() {
        _ammoCount--;        
    }
}
