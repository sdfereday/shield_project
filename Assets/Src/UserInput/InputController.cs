using UnityEngine;
using Game.GameCamera;
using Game.SceneManagement;
using Game.Interaction;

namespace Game.UserInput
{
    // See: https://bitbucket.org/drunkenoodle/rr-clone/src for original (and battle ideas).
    [RequireComponent(typeof(Rigidbody))]
    public class InputController : MonoBehaviour, IDirectionInfo
    {
        public delegate void InputAction(INPUT_TYPE inputType);
        public static event InputAction OnConfirm;
        public static event InputAction OnCancel;
        public static event InputAction OnInventory;

        public float maxSpeed = 5f;
        private bool interactionsCooling = false;
        public bool interactionsEnabled = true;
        public bool movementEnabled = true;
        public Vector3 facing = Vector3.forward;
        public RelativeCam relativeCam;

        public Vector3 CurrentVelocity => rbody.velocity;

        private Rigidbody rbody;
        private InteractionController interactionController;

        // TODO: Perhaps the scene manager should handle this?
        private void OnEnable() {
            SceneController.OnSceneLoadStarted += OnSceneLoadStarted;
            SceneController.OnSceneLoadComplete += OnSceneLoadComplete;
        }

        private void OnDisable()
        {
            SceneController.OnSceneLoadStarted -= OnSceneLoadStarted;
            SceneController.OnSceneLoadComplete -= OnSceneLoadComplete;
        }

        private void OnSceneLoadStarted() => ToggleMovement(false);
        private void OnSceneLoadComplete() => ToggleMovement(true);

        private void Start()
        {
            rbody = GetComponent<Rigidbody>();
            rbody.freezeRotation = true;

            relativeCam = GetComponent<RelativeCam>();
            interactionController = GetComponent<InteractionController>();
        }

        private void Update()
        {
            if (interactionsEnabled)
            {
                if (interactionsCooling)
                {
                    interactionsCooling = false;
                    return;
                }

                if (Input.GetButtonDown(KeyCodeConsts.Use))
                {
                    OnConfirm?.Invoke(INPUT_TYPE.USE);
                    interactionController.Interact();
                }

                if (Input.GetButtonDown(KeyCodeConsts.Cancel))
                {
                    OnCancel?.Invoke(INPUT_TYPE.CANCEL);
                    interactionController.Cancel();
                }

                if (Input.GetButtonDown(KeyCodeConsts.Inventory))
                {
                    OnInventory?.Invoke(INPUT_TYPE.INVENTORY);
                }
            }
        }

        private void FixedUpdate()
        {
            if (movementEnabled)
            {
                float xAxis = Input.GetAxisRaw(KeyCodeConsts.Horizontal);
                float zAxis = Input.GetAxisRaw(KeyCodeConsts.Vertical);

                // Time to fix it properly: https://medium.com/ironequal/unity-character-controller-vs-rigidbody-a1e243591483
                // var nm = new Vector3(xAxis, 0, zAxis) * maxSpeed * Time.deltaTime;
                var vel = new Vector3(relativeCam.GetRelativeVelocity().x, 0, relativeCam.GetRelativeVelocity().z) * (maxSpeed * maxSpeed) * Time.fixedDeltaTime;

                // Since we're directly setting velocity, we need to make sure gravity is being applie.
                vel.y = rbody.velocity.y;

                // This makes the gravity a bit chonkier and more believable.
                rbody.AddForce(Physics.gravity * 4f, ForceMode.Acceleration);

                // Apply it all
                rbody.velocity = vel;
                
                // Log which way we're facing
                facing = rbody.velocity.normalized;

                // Face the movement direction (if velocity changed)
                if (xAxis != 0 || zAxis != 0)
                {
                    float heading = Mathf.Atan2(rbody.velocity.x, rbody.velocity.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, heading, 0f);
                }
            }
        }
        
        public void ToggleInteractions(bool state)
        {
            if (state)
            {
                interactionsCooling = true;
            }

            interactionsEnabled = state;
        }

        public void ToggleMovement(bool state)
        {
            rbody.velocity = Vector3.zero;
            movementEnabled = state;
        }

        public float GetCurrentMagnitude() => rbody.velocity.magnitude;

        public Vector3 GetDirectionVector3D() => facing;
    }
}