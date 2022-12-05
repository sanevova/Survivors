using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _dashLength;

    [SerializeField] private List<AbilitySO> _abilitieScriptables;
    [SerializeField] private Material _debugMaterial;

    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private List<Ability> _abilities = new();

    public static event Action<AbilitySO> OnCastAnimationHandler;
    public static Material DebugMaterial { get; private set; }

    private void Awake() {
        DebugMaterial = _debugMaterial;
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        InitAbilities();
    }

    private void InitAbilities() {
        foreach (var abilityScriptable in _abilitieScriptables) {
            _abilities.Add(new(abilityScriptable, this));
        }
    }

    void Update() {
        UpdateMovement();
        UpdateAbilities();
    }


    private void UpdateMovement() {
        Vector2 movementDirection = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementDirection.Normalize();
        _body.velocity = movementDirection * _movementSpeed;
        if (movementDirection.x != 0) {
            _spriteRenderer.flipX = movementDirection.x < 0;
        }

        _animator.SetBool("IsRunning", movementDirection.magnitude > 0);

        if (Input.GetKeyDown(KeyCode.Space) && movementDirection != Vector2.zero) {
            transform.DOMove(transform.position + (Vector3)movementDirection * _dashLength, 0.3f);
        }
    }

    private void UpdateAbilities() {
        foreach (var ability in _abilities) {
            ability.CastOnCooldown(_animator);
        }
    }

    public void OnAbilityAnimation(AbilitySO scriptableAbility) {
        OnCastAnimationHandler?.Invoke(scriptableAbility);
    }

    public UnitController GetClosestEnemy() {
        var aliveUnits = FindObjectsOfType<UnitController>()
            .Where(enemy => enemy.IsAlive())
            .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position));
        if (aliveUnits.Any()) {
            return aliveUnits.First();
        }
        return null;
    }

    public UnitController GetRandomEnemy() {
        var allEnemies = GetAllEnemies().ToArray();
        if (allEnemies.Any()) {
            return allEnemies[Random.Range(0, allEnemies.Count())];
        }
        return null;
    }

    public IEnumerable<UnitController> GetAllEnemies() {
        return FindObjectsOfType<UnitController>()
            .Where(enemy => enemy.IsAlive());
    }
}
