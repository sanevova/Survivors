using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectlyTargetedAbilityController : TargetedAbilityController {
    private void OnHitAnimation() {
        // ability object is attached to its target
        // so look for the target component in the ability object parent
        OnHit(GetComponentInParent<UnitController>());
    }
}
