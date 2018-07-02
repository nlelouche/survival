using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AIController))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour, IPointerClickHandler
{
    public float moveSpeed = 2.0f;
    public Slider healthBar;

    private HealthComponent _healthComponent;
    private AIController _aiController;
    private Animator _animator;
    private Rigidbody2D _rigidBody;

    private string WalkAnimationState = "Walking";
    private string AttackAnimationState = "Attack";
    private string IdleAnimationState = "Idle";
    private string DeathAnimationState = "Death";

    private int WalkAnimationStateHash;
    private int AttackAnimationStateHash;
    private int IdleAnimationStateHash;
    private int DeathAnimationStateHash;

    // Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 8);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float order = this.transform.position.y * -100;
        renderer.sortingOrder = (int)order;

        _healthComponent = GetComponent<HealthComponent>();

        if (_healthComponent == null)
        {
            _healthComponent = gameObject.AddComponent<HealthComponent>();
        }
        if (_healthComponent == null)
        {
            Debug.LogError("Not Assigned Health Component!!!");
        }
        else
        {
            _healthComponent.onDeath += OnCharacterDied;
            _healthComponent.onHealthChanged += OnCharacterHealthChanged;
            healthBar.maxValue = _healthComponent.MaxHealth;
            healthBar.value = _healthComponent.MaxHealth;
        }

        _animator = GetComponent<Animator>();

        WalkAnimationStateHash = Animator.StringToHash(WalkAnimationState);
        AttackAnimationStateHash = Animator.StringToHash(AttackAnimationState);
        IdleAnimationStateHash = Animator.StringToHash(IdleAnimationState);
        DeathAnimationStateHash = Animator.StringToHash(DeathAnimationState);

        _aiController = GetComponent<AIController>();
        _aiController.onStateChanged += OnAIStateChanged;

        _aiController.SetAiState(AIController.AIState.Walking);

        _rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_aiController.currentState == AIController.AIState.Walking)
            transform.Translate(new Vector2((moveSpeed * Time.deltaTime) * -1, 0));
            //_rigidBody.velocity = new Vector2((moveSpeed * Time.deltaTime) * -1, 0);
        else
            _rigidBody.velocity = Vector2.zero;
        //transform.Translate(new Vector2((moveSpeed * Time.deltaTime) * -1, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _aiController.SetAiState(AIController.AIState.Attacking);
    }

    private void OnAIStateChanged(AIController.AIState newState)
    {
        if (newState == AIController.AIState.Walking)
            _animator.Play(WalkAnimationStateHash, -1, Random.Range(0.0f, 1.0f));
        else if (newState == AIController.AIState.Idle)
            _animator.Play(IdleAnimationStateHash, -1, Random.Range(0.0f, 1.0f));
        else if (newState == AIController.AIState.Attacking)
        {
            _animator.Play(AttackAnimationStateHash);
        }
            
    }

    private void OnCharacterDied()
    {
        _aiController.SetAiState(AIController.AIState.Dying);
        _animator.Play(DeathAnimationStateHash);
    }

    private void OnCharacterHealthChanged(float newAmount)
    {
        healthBar.value = newAmount;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_aiController.currentState != AIController.AIState.Dying)
            _healthComponent.RemoveHealth(10);
    }

    public void AnimationCallback_AttackDamage()
    {
        Debug.Log("Attack Damage Frame");
    }

    public void AnimationCallback_AttackFinished()
    {
        Debug.Log("Attack Finish Frame");
    }

    public void AnimationCallback_DeathFinished()
    {
        Debug.Log("Death Ended");
        Destroy(this.gameObject);
    }
}
