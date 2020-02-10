using UnityEngine;

namespace Game.UserInput
{
    public class InputController : MonoBehaviour
    {
        CharacterController characterController;

        public float speed = 6.0f;
        public float gravity = 20.0f;

        private Vector3 moveDirection = Vector3.zero;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            // Face the movement direction
            float heading = Mathf.Atan2(characterController.velocity.x, characterController.velocity.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, heading, 0f);

            // We are grounded, so recalculate
            // move direction directly from axes
            if (characterController.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection *= speed;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}