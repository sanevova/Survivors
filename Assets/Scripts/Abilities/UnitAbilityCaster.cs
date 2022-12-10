using System.Collections.Generic;

public class UnitAbilityCaster : AbilityCaster {
    private PlayerAbilityCaster _player;

    protected override void Awake() {
        base.Awake();
        _player = FindObjectOfType<PlayerAbilityCaster>();
    }

    internal override IEnumerable<AbilityCaster> GetAllEnemies() {
        yield return _player;
    }

    internal override AbilityCaster GetClosestEnemy() {
        return _player;
    }

    internal override AbilityCaster GetRandomEnemy() {
        return _player;
    }


    internal override void OnHitByAbility(Ability ability) {
        base.OnHitByAbility(ability);
        _animator.SetTrigger("Hurt");
    }

    public override bool IsPlayer() {
        return false;
    }
}
