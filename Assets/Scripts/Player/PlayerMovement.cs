using DG.Tweening;
using EventManager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float originalSpeed;
        [SerializeField] private float minXPosition;
        [SerializeField] private float maxXPosition;
        [SerializeField] private float sensitivity;
        [SerializeField] private PlayerData_SO playerData;

        private float lastFrameMousePosition;
        private float currentSpeed;
        private readonly float fixedDuration = 0.5f;

        private Vector3 startPosition;
        private Quaternion startRotation;
        private Vector3 originalRotation;

        private PlayerDetector playerDetector;
        private TacticalDiceWars tacticalWars;
        private InputAction positionClicked;
        private InputAction deltaPositionClicked;
        private Rigidbody rb;

        private bool isSwiping;
        private bool canProcessInput = true;

        private void Awake()
        {
            isSwiping = false;
            canProcessInput = false;
            originalRotation = transform.eulerAngles;
            currentSpeed = originalSpeed;
            startPosition = transform.position;
            startRotation = transform.rotation;
            Initialize();
        }

        private void Initialize()
        {
            tacticalWars = new TacticalDiceWars();
            rb = GetComponent<Rigidbody>();

            tacticalWars.Player.OnClickHappened.performed += OnClickedHappened;
            tacticalWars.Player.OnClickHappened.canceled += OnClickedHappenedEnd;

            tacticalWars.Player.DeltaAction.performed += OnDeltaPosition;
        }

        private void OnEnable()
        {
            InternalEvents.BossDeath += OnBossFight;
            ExternalEvents.LevelStart += OnLevelStart;
            InternalEvents.BattleArmyArea += OnBattleArmyArea;
            InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
            InternalEvents.BossFightTriggered += OnBossFightTriggered;

            tacticalWars.Player.Enable();
        }
        
        private void OnDisable()
        {
            InternalEvents.BossDeath -= OnBossFight;
            ExternalEvents.LevelStart -= OnLevelStart;
            InternalEvents.BattleArmyArea -= OnBattleArmyArea;
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
            InternalEvents.BossFightTriggered -= OnBossFightTriggered;

            tacticalWars.Player.Disable();
        }

        private void OnBossFightTriggered()
        {
            canProcessInput = false;
        }

        private void OnAllEnemyDeath()
        {
            canProcessInput = true;
        }

        private void OnBattleArmyArea(bool isEnteringBattle)
        {
            canProcessInput = !isEnteringBattle;
        }

        private void FixedUpdate()
        {
            if (!rb.isKinematic)
                rb.velocity = new Vector3(0, 0, originalSpeed) * Time.fixedDeltaTime;
        }

        private void OnLevelStart()
        {
            isSwiping = true;
            canProcessInput = true;
            rb.isKinematic = false;
        }

        private void OnDeltaPosition(InputAction.CallbackContext context)
        {
            if (!canProcessInput) return;

            if (isSwiping)
            {
                Vector2 currentMousePosition = context.ReadValue<Vector2>();
                Vector2 deltaMousePosition = currentMousePosition;

                float moveFactorX = deltaMousePosition.x * sensitivity * Time.deltaTime;
                float newXPosition = transform.position.x + moveFactorX;
                newXPosition = Mathf.Clamp(newXPosition, minXPosition, maxXPosition);

                rb.MovePosition(new Vector3(newXPosition, transform.position.y, transform.position.z));
            }
        }

        private void OnClickedHappenedEnd(InputAction.CallbackContext context)
        {
            isSwiping = false;
        }

        private void OnClickedHappened(InputAction.CallbackContext context)
        {
            isSwiping = true;
        }

        private void OnBossFight()
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }

        
    }
}
