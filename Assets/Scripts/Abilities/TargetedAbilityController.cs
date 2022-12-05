using UnityEngine;

public abstract class TargetedAbilityController : MonoBehaviour {
    public Ability Ability;

    public sealed class AbilityNotAssignedException : System.Exception { }

    protected void OnHit(UnitController target) {
        if (Ability == null) {
            throw new AbilityNotAssignedException();
        }
        target.OnHitByAbility(Ability);
    }

    private void OnHitAnimationEnd() {
        Destroy(gameObject);
    }
}
