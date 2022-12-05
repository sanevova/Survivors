using UnityEngine;

public class ProjectileController : MonoBehaviour {
    [SerializeField] private float speed;
    public Ability Ability;

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
        GetComponent<Animator>().SetTrigger("Hit");
        transform.SetParent(other.transform);
        _shouldMove = false;
        other.GetComponent<UnitController>().OnHitByAbility(Ability);
    }

    private void OnHitAnimationEnd() {
        Destroy(gameObject);
    }
}
