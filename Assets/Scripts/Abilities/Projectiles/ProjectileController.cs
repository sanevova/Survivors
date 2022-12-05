using UnityEngine;

public class ProjectileController : TargetedAbilityController {
    [SerializeField] private float speed;

    private bool _didProcDamage = false;
    private bool _shouldMove = true;

    void Update() {
        if (_shouldMove) {
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }
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
}
