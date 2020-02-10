using UnityEngine;

namespace Game.UserInput
{
    // See: https://bitbucket.org/drunkenoodle/rr-clone/src for original (and battle ideas).
    [RequireComponent(typeof(Rigidbody))]
    public class InputControllerRBody : MonoBehaviour, IDirectionInfo
    {
        public delegate void InputAction(INPUT_TYPE inputType);
        public static event InputAction OnConfirm;
        public static event InputAction OnCancel;

        public float maxSpeed = 5f;
        public bool InteractionsEnabled { get; private set; }
        public bool MovementEnabled { get; private set; }
        public Vector3 Facing { get; set; }
        public Vector3 CurrentVelocity => rbody.velocity;

        private Rigidbody rbody;

        private void OnSceneLoadComplete() => ToggleMovement(true);
        private void OnSceneLoadStarted() => ToggleMovement(false);

        private void Start()
        {
            rbody = GetComponent<Rigidbody>();
            rbody.freezeRotation = true;

            // rbody.useGravity = false;
            // rbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            Facing = Vector3.forward;

            InteractionsEnabled = OnConfirm != null || OnCancel != null;
            MovementEnabled = true;
        }

        private void Update()
        {
            rbody.velocity = Vector3.zero;

            if (InteractionsEnabled)
            {
                if (Input.GetKeyDown(KeyCodeConsts.CONFIRM))
                {
                    OnConfirm(INPUT_TYPE.USE);
                }

                if (Input.GetKeyDown(KeyCodeConsts.CANCEL))
                {
                    OnCancel(INPUT_TYPE.CANCEL);
                }
            }
            
            /*
            if (MovementEnabled)
            {
                float xAxis = Input.GetAxisRaw(KeyCodeConsts.Horizontal);
                float zAxis = Input.GetAxisRaw(KeyCodeConsts.Vertical);

                if (xAxis != 0 || zAxis != 0)
                {
                    Vector3 nm = new Vector3(xAxis * maxSpeed, 0f, zAxis * maxSpeed).normalized;
                    rbody.velocity = nm * maxSpeed;
                    Facing = CurrentVelocity.normalized;
                }
            }*/
        }

        private void FixedUpdate()
        {
            if (MovementEnabled)
            {
                float xAxis = Input.GetAxisRaw(KeyCodeConsts.Horizontal);
                float zAxis = Input.GetAxisRaw(KeyCodeConsts.Vertical);

                var nm = new Vector3(xAxis, 0, zAxis) * maxSpeed * Time.deltaTime;
                rbody.velocity = nm * maxSpeed;
                Facing = CurrentVelocity.normalized;

                // Face the movement direction (if velocity changed)
                if (xAxis != 0 || zAxis != 0)
                {
                    float heading = Mathf.Atan2(rbody.velocity.x, rbody.velocity.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, heading, 0f);
                }
            }

            /* Camera needs to know where the player is in relation to the cameras edges, this
             * gives you control over the tracking. */
        }

        public void ToggleInteractions(bool state) => InteractionsEnabled = state;

        public void ToggleMovement(bool state) => MovementEnabled = state;

        public Vector3 GetDirectionVector3D() => Facing;

        public float GetCurrentMagnitude() => CurrentVelocity.magnitude;
    }
}