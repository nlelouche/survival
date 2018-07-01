using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public delegate void OnStateChanged(AIState newState);
    public OnStateChanged onStateChanged;

    public enum AIState
    {
        Idle,
        Attacking,
        Walking,
        Dying
    }

    public AIState defaultState;

    private AIState _currentState;
    public AIState currentState { get { return _currentState; } }

	// Use this for initialization
	void Start () {
        SetAiState(defaultState);
	}

    public void SetAiState(AIState newState)
    {
        _currentState = newState;
        if (onStateChanged != null)
            onStateChanged(_currentState);
    }
}
