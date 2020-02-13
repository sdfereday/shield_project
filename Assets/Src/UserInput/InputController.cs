﻿using UnityEngine;
using Game.GameCamera;
using Game.SceneManagement;
using Game.Interaction;
using Game.Dialogue;

namespace Game.UserInput
{
    // See: https://bitbucket.org/drunkenoodle/rr-clone/src for original (and battle ideas).
    [RequireComponent(typeof(Rigidbody))]
    public class InputController : MonoBehaviour, IDirectionInfo
    {
        public delegate void InputAction(INPUT_TYPE inputType);
        public static event InputAction OnConfirm;
        public static event InputAction OnCancel;

        public float maxSpeed = 5f;
        public bool InteractionsEnabled { get; private set; }
        public bool MovementEnabled { get; private set; }
        public Vector3 Facing { get; set; }
        public Vector3 CurrentVelocity => rbody.velocity;
        public RelativeCam relativeCam;

        private Rigidbody rbody;
        private InteractionManager interactionManager;

        private void OnEnable() {
            SceneController.OnSceneLoadStarted += OnSceneLoadStarted;
            SceneController.OnSceneLoadComplete += OnSceneLoadComplete;
            DialogueManager.OnConversationStarted += OnConversationStarted;
            DialogueManager.OnConversationComplete += OnConversationComplete;
        }

        private void OnDisable()
        {
            SceneController.OnSceneLoadStarted -= OnSceneLoadStarted;
            SceneController.OnSceneLoadComplete -= OnSceneLoadComplete;
            DialogueManager.OnConversationStarted -= OnConversationStarted;
            DialogueManager.OnConversationComplete -= OnConversationComplete;
        }

        private void OnSceneLoadStarted() => ToggleMovement(false);
        private void OnSceneLoadComplete() => ToggleMovement(true);

        private void OnConversationStarted() {
            ToggleMovement(false);
            InteractionsEnabled = false;
        }
        private void OnConversationComplete() {
            ToggleMovement(true);
            InteractionsEnabled = true;
        }

        private void Start()
        {
            rbody = GetComponent<Rigidbody>();
            rbody.freezeRotation = true;

            relativeCam = GetComponent<RelativeCam>();

            interactionManager = GetComponent<InteractionManager>();

            Facing = Vector3.forward;

            // Do not liiiikkeee
            InteractionsEnabled = interactionManager != null;
            MovementEnabled = true;
        }

        private void Update()
        {
            // Can this be used with the handler service?
            if (InteractionsEnabled)
            {
                // TODO: Is it worth interaction manager listening to input? I'm not sure I'm
                // happy with all this back and forth between them...
                if (Input.GetKeyDown(KeyCodeConsts.CONFIRM) || Input.GetButtonDown(KeyCodeConsts.Use))
                {
                    OnConfirm?.Invoke(INPUT_TYPE.USE);
                    interactionManager.Interact();
                }

                if (Input.GetKeyDown(KeyCodeConsts.CANCEL))
                {
                    OnCancel?.Invoke(INPUT_TYPE.CANCEL);
                    interactionManager.Cancel();
                }
            }
        }

        private void FixedUpdate()
        {
            if (MovementEnabled)
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
                Facing = rbody.velocity.normalized;

                // Face the movement direction (if velocity changed)
                if (xAxis != 0 || zAxis != 0)
                {
                    float heading = Mathf.Atan2(rbody.velocity.x, rbody.velocity.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, heading, 0f);
                }
            }
        }

        public void ToggleInteractions(bool state) => InteractionsEnabled = state;

        public void ToggleMovement(bool state) => MovementEnabled = state;

        public Vector3 GetDirectionVector3D() => Facing;

        public float GetCurrentMagnitude() => rbody.velocity.magnitude;
    }
}