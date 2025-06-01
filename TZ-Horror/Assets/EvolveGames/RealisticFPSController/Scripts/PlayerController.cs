using System.Collections;
using UnityEngine;

namespace EvolveGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerController")]
        [SerializeField] public Transform _camera;
        [SerializeField, Range(1, 10)] float _walkingSpeed = 3.0f;
        [Range(0.1f, 5)] public float CroughSpeed = 1.0f;
        [SerializeField, Range(2, 20)] float _runningSpeed = 4.0f;
        [SerializeField, Range(0, 20)] float _jumpSpeed = 6.0f;
        [SerializeField, Range(0.5f, 10)] float _lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)] float _lookXLimit = 80.0f;

        [Space(20)]
        [Header("Advance")]
        [SerializeField] float _runningFOV = 65.0f;
        [SerializeField] float _speedToFOV = 4.0f;
        [SerializeField] float _croughHeight = 1.0f;
        [SerializeField] float _gravity = 20.0f;
        [SerializeField] float _timeToRunning = 2.0f;

        [Space(20)]
        [Header("Input")]
        [SerializeField] KeyCode _croughKey = KeyCode.LeftControl;

        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool CanRunning = true;
        [HideInInspector] public bool isRunning = false;
        [HideInInspector] public bool Moving;
        [HideInInspector] public float vertical;
        [HideInInspector] public float horizontal;
        [HideInInspector] public float Lookvertical;
        [HideInInspector] public float Lookhorizontal;
        [HideInInspector] public Vector3 moveDirection = Vector3.zero;
        [HideInInspector] public CharacterController characterController;

        private float _rotationX;
        private float _defaultHeight;
        private float _defaultFOV;
        private float _currentSpeed;
        private float _walkingValue;
        private bool _isCrough;
        private Camera _cameraComponent;


        private Vector3 _savedCameraPosition;
        private Quaternion _savedCameraRotation;
        private float _savedCameraFOV;
        [SerializeField] private Transform _dialogueFocusTarget;
        [SerializeField] private float _dialogueFOV = 30.0f;
        [SerializeField] private float _focusDuration = 0.75f;
        private Coroutine _focusRoutine;

        private void OnEnable()
        {
            NewDialogueSystem.DialogueSystem.OnDialogueStart += OnDialogueStart;
            NewDialogueSystem.DialogueSystem.OnDialogueFinished += OnDialogueFinished;
        }

        private void OnDisable()
        {
            NewDialogueSystem.DialogueSystem.OnDialogueStart -= OnDialogueStart;
            NewDialogueSystem.DialogueSystem.OnDialogueFinished -= OnDialogueFinished;
        }

        private void OnDialogueStart(string _, Transform speaker)
        {
            _dialogueFocusTarget = speaker;
            if (_focusRoutine != null) StopCoroutine(_focusRoutine);
            _focusRoutine = StartCoroutine(FocusOnDialogueTarget());
        }

        private void OnDialogueFinished(string _)
        {
            _dialogueFocusTarget = null;
            if (_focusRoutine != null) StopCoroutine(_focusRoutine);
            _focusRoutine = StartCoroutine(ResetCameraFocus());
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            _cameraComponent = GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _defaultHeight = characterController.height;
            _defaultFOV = _cameraComponent.fieldOfView;

            _currentSpeed = _runningSpeed;
            _walkingValue = _walkingSpeed;
        }

        private void Update()
        {
            HandleGravity();
            HandleMovement();
            HandleJump();
            ApplyMovement();
            HandleRotationAndFOV();
            HandleCrough();
        }

        private void HandleGravity()
        {
            if (!characterController.isGrounded)
            {
                moveDirection.y -= _gravity * Time.deltaTime;
            }
        }

        private void HandleMovement()
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            isRunning = !_isCrough && CanRunning && Input.GetKey(KeyCode.LeftShift);
            float moveSpeed = isRunning ? _currentSpeed : _walkingValue;

            vertical = canMove ? moveSpeed * Input.GetAxis("Vertical") : 0;
            horizontal = canMove ? moveSpeed * Input.GetAxis("Horizontal") : 0;

            if (isRunning)
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, _runningSpeed, _timeToRunning * Time.deltaTime);
            }
            else
            {
                _currentSpeed = _walkingValue;
            }

            float y = moveDirection.y;
            moveDirection = (forward * vertical) + (right * horizontal);
            moveDirection.y = y;

            Moving = horizontal != 0 || vertical != 0;
        }

        private void HandleJump()
        {
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = _jumpSpeed;
            }
        }

        private void ApplyMovement()
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleRotationAndFOV()
        {
            if (Cursor.lockState != CursorLockMode.Locked || !canMove) return;

            Lookvertical = -Input.GetAxis("Mouse Y");
            Lookhorizontal = Input.GetAxis("Mouse X");

            _rotationX += Lookvertical * _lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);

            _camera.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Lookhorizontal * _lookSpeed, 0);

            float targetFOV = isRunning && Moving ? _runningFOV : _defaultFOV;
            _cameraComponent.fieldOfView = Mathf.Lerp(_cameraComponent.fieldOfView, targetFOV, _speedToFOV * Time.deltaTime);
        }

        private void HandleCrough()
        {
            RaycastHit hit;
            if (Input.GetKey(_croughKey))
            {
                _isCrough = true;
                characterController.height = Mathf.Lerp(characterController.height, _croughHeight, 5 * Time.deltaTime);
                _walkingValue = Mathf.Lerp(_walkingValue, CroughSpeed, 6 * Time.deltaTime);
            }
            else if (!Physics.Raycast(_cameraComponent.transform.position, transform.up, out hit, 0.8f, 1))
            {
                if (characterController.height != _defaultHeight)
                {
                    _isCrough = false;
                    characterController.height = Mathf.Lerp(characterController.height, _defaultHeight, 6 * Time.deltaTime);
                    _walkingValue = Mathf.Lerp(_walkingValue, _walkingSpeed, 4 * Time.deltaTime);
                }
            }
        }

        private IEnumerator FocusOnDialogueTarget()
        {
            canMove = false;

            _savedCameraPosition = _camera.position;
            _savedCameraRotation = _camera.rotation;
            _savedCameraFOV = _cameraComponent.fieldOfView;

            Quaternion targetRotation = Quaternion.LookRotation(_dialogueFocusTarget.position - _camera.position);

            float elapsed = 0f;
            while (elapsed < _focusDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _focusDuration;

                _camera.rotation = Quaternion.Slerp(_savedCameraRotation, targetRotation, t);
                _cameraComponent.fieldOfView = Mathf.Lerp(_savedCameraFOV, _dialogueFOV, t);

                yield return null;
            }
        }

        private IEnumerator ResetCameraFocus()
        {
            Quaternion startRot = _camera.rotation;
            float startFOV = _cameraComponent.fieldOfView;

            float elapsed = 0f;
            while (elapsed < _focusDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _focusDuration;

                _camera.rotation = Quaternion.Slerp(startRot, _savedCameraRotation, t);
                _cameraComponent.fieldOfView = Mathf.Lerp(startFOV, _savedCameraFOV, t);

                yield return null;
            }

            canMove = true;
        }
    }
}
