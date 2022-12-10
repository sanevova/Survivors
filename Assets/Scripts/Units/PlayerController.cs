using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerAbilityCaster))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _dashLength;

    [SerializeField] private Material _debugMaterial;

    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public static Material DebugMaterial { get; private set; }

    private void Awake() {
        DebugMaterial = _debugMaterial;
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update() {
        UpdateMovement();
    }

    private void UpdateMovement() {
        Vector2 movementDirection = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementDirection.Normalize();
        _body.velocity = movementDirection * _movementSpeed;
        if (movementDirection.x != 0) {
            _spriteRenderer.flipX = movementDirection.x < 0;
        }

        _animator.SetBool("IsRunning", movementDirection.magnitude > 0);

        if (Input.GetKeyDown(KeyCode.Space) && movementDirection != Vector2.zero) {
            transform.DOMove(transform.position + (Vector3)movementDirection * _dashLength, 0.3f);
        }
    }

}
