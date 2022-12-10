using System;
using UnityEngine;

public class Hp : MonoBehaviour {
    public float maxHp = 100;
    public event Action OnDeathCallback;

    public float value;
    [HideInInspector]
    public UnitController hpOwner;

    void Awake() {
        value = maxHp;
        hpOwner = GetComponent<UnitController>();
    }

    void Update() {

    }

    public virtual void TakeDamageFrom(float damageValue) {
        value = Mathf.Max(0, value - damageValue);
        if (value == 0) {
            OnDeathCallback?.Invoke();
        }
    }

    public bool IsAlive() {
        return value > 0;
    }

    public bool IsDead() {
        return !IsAlive();
    }
}
