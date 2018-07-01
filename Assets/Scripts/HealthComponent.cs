using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthComponent : MonoBehaviour {

    public float MaxHealth = 100;
    private float _currentHealth;

    public delegate void OnDeath();
    public OnDeath onDeath;

    public delegate void OnHealthChanged(float newAmount);
    public OnHealthChanged onHealthChanged;

	// Use this for initialization
	void Start ()
    {
        _currentHealth = MaxHealth;
	}

    public void RemoveHealth(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
            if (onDeath != null)
                onDeath();

        if (onHealthChanged != null)
            onHealthChanged(_currentHealth);
    }
	
}
