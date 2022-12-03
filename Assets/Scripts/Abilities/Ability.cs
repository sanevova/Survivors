using UnityEngine;

public class Ability {
    public AbilitySO AbilityScriptable;
    private float _lastCastTime;
    private PlayerController _caster;

    public Ability(AbilitySO scriptable, PlayerController caster) {
        AbilityScriptable = scriptable;
        _lastCastTime = Time.time;
        _caster = caster;
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

    public void Cast(Animator animator = null) {
        _lastCastTime = Time.time;
        if (AbilityScriptable.abilityType == AbilityType.Projectile) {
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
        _debugHitbox.transform.position = _caster.transform.position + Vector3.back;

#endif
        if (AbilityScriptable.abilityType == AbilityType.AroundCaster) {
            foreach (var unit in Object.FindObjectsOfType<UnitController>()) {
                Bounds aoe = new(_caster.transform.position, AbilityScriptable.hitboxSize);
                if (aoe.Intersects(unit.Collider.bounds)) {
                    unit.OnHitByAbility(_caster.GetComponent<Killable>(), this);
                }
            }
        }
    }

}