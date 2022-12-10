using UnityEngine;

public enum AbilityType {
    AroundCaster,
    Projectile,
    DirectlyTargeted,
}

public enum AbilityTargetingType {
    Closest,
    Random,
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ability", order = 1)]
public class AbilitySO : ScriptableObject {
    public string displayName;
    public AbilityType abilityType;
    public AbilityTargetingType targetingType;
    public float cooldown;
    public int damage;
    public Vector2 hitboxSize;
    public AnimationClip animationClip;
    public TargetedAbilityController prefab;
    public float speed;
    public float orbitingRadius;

    public bool IsOrbiting() {
        return orbitingRadius > Mathf.Epsilon;
    }
}