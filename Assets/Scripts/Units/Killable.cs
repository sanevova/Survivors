using TMPro;
using UnityEngine;

[RequireComponent(typeof(Hp))]
public class Killable : MonoBehaviour {
    [Header("General")]

    public uint killRewardGold;
    [HideInInspector]
    public Hp hp;
    private Collider coll;

    void Awake() {
        hp = GetComponent<Hp>();
        coll = GetComponent<Collider>();
    }

    void Update() {

    }

    public bool IsAlive() {
        return hp?.value > 0;
    }

    public bool IsDead() {
        return !IsAlive();
    }

    public virtual void Die() {
    }

    public void DidKill(Killable deadGuy) {
    }

}
