using UnityEngine;

public class Ability {
    public AbilitySO AbilityScriptable;
    public AbilityCaster Caster;

    private float _lastCastTime;
    private TargetedAbilityController _projectile = null;
    public System.Action<Ability, AbilityCaster> OnHitEnemyHandler;

    public Ability(AbilitySO scriptable, AbilityCaster caster) {
        AbilityScriptable = scriptable;
        _lastCastTime = Time.time;
        Caster = caster;
        caster.OnCastAnimationHandler += OnCastAnimation;
    }

    public bool IsOnCooldown() {
        return Time.time - _lastCastTime < AbilityScriptable.cooldown;
    }

    public void CastOnCooldown(Animator animator = null) {
        if (IsOnCooldown()) {
            return;
        }
        Cast(animator);
    }

    private Quaternion LookAtRotation(Vector2 src, Vector2 dst) {
        float angle = AngleBetweenPoints(src, dst);
        return Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }

    public void Cast(Animator animator = null) {
        _lastCastTime = Time.time;
        if (AbilityScriptable.abilityType == AbilityType.Projectile) {
            AbilityCaster target = PickTarget();
            if (target == null) {
                return;
            }
            if (AbilityScriptable.IsOrbiting() && _projectile != null) {
                // don't spawn another orbiting projectile if one already exists
                // will wait for another cooldown
                return;
            }
            _projectile = Object.Instantiate(
                AbilityScriptable.prefab,
                Caster.transform.position,
                LookAtRotation(Caster.transform.position, target.transform.position));
            _projectile.Ability = this;
            return;
        }
        if (AbilityScriptable.abilityType == AbilityType.DirectlyTargeted) {
            AbilityCaster target = PickTarget();
            if (target == null) {
                return;
            }
            var colliderBounds = target.Collider.bounds;
            var colliderBottom = colliderBounds.min + Vector3.right * colliderBounds.extents.x;
            TargetedAbilityController abilityObject = Object.Instantiate(
                AbilityScriptable.prefab,
                colliderBottom,
                Quaternion.identity,
                parent: target.transform);
            abilityObject.Ability = this;
            return;
        }
        if (AbilityScriptable.abilityType == AbilityType.AroundCaster) {
            animator?.Play(AbilityScriptable.animationClip.name);
        }
    }

    private AbilityCaster PickTarget() {
        if (AbilityScriptable.targetingType == AbilityTargetingType.Closest) {
            return Caster.GetClosestEnemy();
        }
        if (AbilityScriptable.targetingType == AbilityTargetingType.Random) {
            return Caster.GetRandomEnemy();
        }
        return null;
    }

#if UNITY_EDITOR
    private static GameObject _debugHitbox = null;
#endif

    private void OnCastAnimation(AbilitySO castedAbilityScriptable) {
        if (castedAbilityScriptable.displayName != AbilityScriptable.displayName) {
            return;
        }
#if UNITY_EDITOR
        // draw debug rect
        if (_debugHitbox == null) {
            _debugHitbox = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _debugHitbox.GetComponent<Collider>().enabled = false;
            _debugHitbox.transform.localScale = AbilityScriptable.hitboxSize;
            _debugHitbox.GetComponent<Renderer>().material = PlayerController.DebugMaterial;
        }
        _debugHitbox.transform.position = Caster.transform.position + Vector3.back;
#endif
        if (AbilityScriptable.abilityType == AbilityType.AroundCaster) {
            foreach (var enemy in Caster.GetAllEnemies()) {
                Bounds aoe = new(Caster.transform.position, AbilityScriptable.hitboxSize);
                if (aoe.Intersects(enemy.Collider.bounds)) {
                    OnHitEnemyHandler?.Invoke(this, enemy);
                }
            }
        }
    }

}