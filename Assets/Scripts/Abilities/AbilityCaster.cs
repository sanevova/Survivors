using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hp))]
public abstract class AbilityCaster : MonoBehaviour {
    internal abstract AbilityCaster GetClosestEnemy();
    internal abstract AbilityCaster GetRandomEnemy();
    internal abstract IEnumerable<AbilityCaster> GetAllEnemies();
    internal virtual void OnHitByAbility(Ability ability) {
        _hp.TakeDamageFrom(ability.AbilityScriptable.damage);
    }
    internal void OnHitEnemy(Ability ability, AbilityCaster target) {
        target.OnHitByAbility(ability);
    }
    public abstract bool IsPlayer();
    public bool IsUnit() => !IsPlayer();

    [HideInInspector] public Collider2D Collider { get; private set; }
    public event System.Action<AbilitySO> OnCastAnimationHandler;


    [SerializeField] private List<AbilitySO> _abilityScriptables;
    private readonly List<Ability> _abilities = new();
    protected Hp _hp;
    protected Animator _animator;


    protected virtual void Awake() {
        _hp = GetComponent<Hp>();
        _animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();
        InitAbilities();
    }

    private void InitAbilities() {
        foreach (var abilityScriptable in _abilityScriptables) {
            Ability ability = new(abilityScriptable, this) {
                OnHitEnemyHandler = OnHitEnemy
            };
            _abilities.Add(ability);
        }
    }

    private void Update() {
        foreach (var ability in _abilities) {
            ability.CastOnCooldown(_animator);
        }
    }

    private void OnAbilityAnimation(AbilitySO abilityScriptable) {
        OnCastAnimationHandler?.Invoke(abilityScriptable);
    }

    public bool IsAlive() {
        return _hp.IsAlive();
    }

    public bool IsDead() {
        return _hp.IsDead();
    }
}
