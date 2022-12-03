using UnityEngine;

[RequireComponent(typeof(Hp))]
public class UnitController : MonoBehaviour {
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Vector2 _playerProximityLimit = new(1f, 0.5f);
    public Material FlashMaterial;

    private Rigidbody2D _body;
    private PlayerController _player;
    [HideInInspector] public Collider2D Collider;

    private Hp _hp;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;
    private Vector3 _previousFramePosiotion;

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _hp = GetComponent<Hp>();
        _hp.OnDeathCallback += OnDeath;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
        _player = FindObjectOfType<PlayerController>();
        Collider = GetComponent<Collider2D>();
    }


    void Update() {
        if (_hp.hpOwner.IsDead()) {
            return;
        }
        var calculatedSpeed = (transform.position - _previousFramePosiotion).magnitude / Time.deltaTime;
        Vector2 movementDirection = _player.transform.position - transform.position;
        if (movementDirection.magnitude < Mathf.Min(_playerProximityLimit.x, _playerProximityLimit.y)) {
            if (Mathf.Abs(movementDirection.x) <= _playerProximityLimit.x) {
                movementDirection.x = 0;
            }
            if (Mathf.Abs(movementDirection.y) <= _playerProximityLimit.y) {
                movementDirection.y = 0;
            }
        }
        movementDirection.Normalize();
        _body.velocity = movementDirection * _movementSpeed;

        if (movementDirection.x != 0) {
            _spriteRenderer.flipX = movementDirection.x < 0;
        }
        _animator.SetBool("IsRunning", calculatedSpeed > 0.1f);
        _previousFramePosiotion = transform.position;
    }

    public void OnHitByAbility(Killable caster, Ability ability) {
        _hp.TakeDamageFrom(caster, ability.AbilityScriptable.damage);
        _animator.SetTrigger("Hurt");
    }

    public void StartFlash() {
        _spriteRenderer.material = FlashMaterial;
    }

    public void StopFlash() {
        _spriteRenderer.material = _defaultMaterial;
    }

    public void OnDeath() {
        Destroy(gameObject, 5f);
        _animator.SetTrigger("Die");
        _body.velocity = Vector2.zero;
        _spriteRenderer.sortingOrder -= 1;
        Collider.enabled = false;
        GetComponent<BlockUnitCollision>().BlockerCollider.enabled = false;
    }

    public bool IsAlive() {
        return _hp.hpOwner.IsAlive();
    }
}
