using UnityEngine;

public class BlockUnitCollision : MonoBehaviour {
    [SerializeField] private Collider2D _blockerCollider;

    void Start() {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _blockerCollider, true);
    }
}
