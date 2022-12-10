using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityCaster : AbilityCaster {
    internal override AbilityCaster GetClosestEnemy() {
        var aliveUnits = FindObjectsOfType<AbilityCaster>()
            .Where(enemy => enemy.IsAlive())
            .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position));
        if (aliveUnits.Any()) {
            return aliveUnits.First();
        }
        return null;
    }

    internal override AbilityCaster GetRandomEnemy() {
        var allEnemies = GetAllEnemies().ToArray();
        if (allEnemies.Any()) {
            return allEnemies[Random.Range(0, allEnemies.Count())];
        }
        return null;
    }

    internal override IEnumerable<AbilityCaster> GetAllEnemies() {
        return FindObjectsOfType<AbilityCaster>()
            .Where(enemy => enemy.IsUnit() && enemy.IsAlive());
    }

    internal override void OnHitByAbility(Ability ability) {
        base.OnHitByAbility(ability);
        Debug.Log($"player hit by {ability.AbilityScriptable.displayName} > hp={_hp.value}");
    }

    public override bool IsPlayer() {
        return true;
    }
}
