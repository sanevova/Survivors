using UnityEngine;

public enum AbilityType {
    AroundCaster,
    Projectile,
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ability", order = 1)]
public class AbilitySO : ScriptableObject {
    public string displayName;
    public AbilityType abilityType;
    public float cooldown;
    public int damage;
    public Vector2 hitboxSize;
}