using UnityEngine;

public class ProjectileController : TargetedAbilityController {
    private bool _didProcDamage = false;
    private bool _shouldMove = true;

    void Update() {
        if (!_shouldMove) {
            return;
        }
        var speed = Ability.AbilityScriptable.speed;
        if (Ability.AbilityScriptable.IsOrbiting()) {
            OrbitCaster(speed);
            return;
        }
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (_didProcDamage || !other.CompareTag("Unit")) {
            return;
        }
        _didProcDamage = true;
        OnHit(other.GetComponent<UnitController>());
        GetComponent<Animator>().SetTrigger("Hit");
        transform.SetParent(other.transform);
        _shouldMove = false;
    }

    private void OrbitCaster(float speed) {
        var casterPosition = Ability.Caster.transform.position;
        // '-' sign for clockwize direction
        var orbitingRadius = Ability.AbilityScriptable.orbitingRadius;
        var x = orbitingRadius * Mathf.Cos(-Time.time * speed);
        var y = orbitingRadius * Mathf.Sin(-Time.time * speed);
        transform.position = casterPosition + new Vector3(x, y, 0);
        var rotationAngle = Ability.AngleBetweenPoints(transform.position, casterPosition) + 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
    }
}
