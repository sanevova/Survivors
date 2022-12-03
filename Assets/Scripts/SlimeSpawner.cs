using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour {
    private PlayerController _player;

    [SerializeField] private GameObject _slime;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnCooldown;
    private float _lastSpawnTime = 0f;


    private void Awake() {
        _player = FindObjectOfType<PlayerController>();
    }

    void Start() {

    }

    void Update() {
        SpawnOnCooldown();
    }

    private void SpawnOnCooldown() {
        if (Time.time - _lastSpawnTime < _spawnCooldown) {
            return;
        }
        _lastSpawnTime = Time.time;
        Vector3 slimePosition = _player.transform.position + RandomSpawnOffset();
        Instantiate(_slime, slimePosition, Quaternion.identity);
    }

    private Vector3 RandomSpawnOffset() {
        float x = Random.Range(-_spawnRadius, _spawnRadius);
        int ySign = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        float y = ySign * Mathf.Sqrt(_spawnRadius * _spawnRadius - x * x);
        return new(x, y, 0);
    }

}
