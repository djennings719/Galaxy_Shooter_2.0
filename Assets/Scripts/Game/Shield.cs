using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private int _shieldLives = 0;

    private bool _isShieldEnabled;

    public bool IsShieldEnabled {
        get { return _isShieldEnabled; }
        set {
            SetShieldEnabled(value);
        }
    }

    public void SetShieldDamage()
    {
        _shieldLives--;

        //change color
        if (_shieldLives == 0)
        {
            SetShieldEnabled(false);
        }
        else
        {
            SetShieldColor();
        }
    }

    private void SetShieldColor()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (_shieldLives == 3)
        {
            renderer.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (_shieldLives == 2)
        {
            renderer.color = new Color(.7f, .7f, .7f, 1f);
        }
        else if (_shieldLives == 1)
        {
            renderer.color = new Color(.3f, .3f, .3f, 1f);
        }
    }

    private void SetShieldEnabled(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
        _isShieldEnabled = isEnabled;
        if (isEnabled)
        {
            _shieldLives = 3;
            SetShieldColor();
        }
    }
}
