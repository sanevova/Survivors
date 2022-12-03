using UnityEngine;

public class Ability {
    public AbilitySO AbilityScriptable;
    private float _lastCastTime;
    public PlayerController Caster;

    public Ability(AbilitySO scriptable, PlayerController caster) {
        AbilityScriptable = scriptable;
        _lastCastTime = Time.time;
        Caster = caster;
        PlayerController.OnCastAnimationHandler += OnCastAnimation;
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

    private float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }

    public void Cast(Animator animator = null) {
        _lastCastTime = Time.time;
        if (AbilityScriptable.abilityType == AbilityType.Projectile) {
            Vector2 closestEnemyPosition = Caster.GetClosestEnemy().transform.position;
            ProjectileController projectile = Object.Instantiate(
                AbilityScriptable.projectile,
                Caster.transform.position,
                LookAtRotation(Caster.transform.position, closestEnemyPosition));
            projectile.Ability = this;
            return;
        }
        if (AbilityScriptable.abilityType == AbilityType.AroundCaster) {
            animator?.Play($"Animation_Player_{AbilityScriptable.displayName}");
        }
    }

#if UNITY_EDITOR
    private static GameObject _debugHitbox = null;

#endif

    private void OnCastAnimation(AbilitySO castedAbilityScriptable) {
        if (castedAbilityScriptable.displayName != AbilityScriptable.displayName) {
            return;
        }
#if UNITY_EDITOR
        if (_debugHitbox == null) {
            _debugHitbox = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _debugHitbox.GetComponent<Collider>().enabled = false;
            _debugHitbox.transform.localScale = AbilityScriptable.hitboxSize;
            _debugHitbox.GetComponent<Renderer>().material = PlayerController.DebugMaterial;
        }
        _debugHitbox.transform.position = Caster.transform.position + Vector3.back;

#endif
        if (AbilityScriptable.abilityType == AbilityType.AroundCaster) {
            foreach (var unit in Object.FindObjectsOfType<UnitController>()) {
                Bounds aoe = new(Caster.transform.position, AbilityScriptable.hitboxSize);
                if (aoe.Intersects(unit.Collider.bounds)) {
                    unit.OnHitByAbility(Caster.GetComponent<Killable>(), this);
                }
            }
        }
    }

}