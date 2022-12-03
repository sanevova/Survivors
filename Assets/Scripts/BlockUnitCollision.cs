using UnityEngine;

public class BlockUnitCollision : MonoBehaviour {
    public Collider2D BlockerCollider;

    void Start() {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), BlockerCollider, true);
    }
}
