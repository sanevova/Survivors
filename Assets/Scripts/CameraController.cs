using UnityEngine;

public class CameraController : MonoBehaviour {
    private PlayerController _playerController;

    private void Awake() {
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update() {
        transform.position = _playerController.transform.position - 10 * Vector3.forward;
    }
}
