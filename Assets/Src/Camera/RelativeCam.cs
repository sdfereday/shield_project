using UnityEngine;

namespace Game.GameCamera
{
    public class RelativeCam : MonoBehaviour
    {
        private Transform camTransform;
        private Vector3 relativeVelocity;
        private Vector3 relativePosition;
        private Vector2 inputVector;

        private Vector3 direction;
        private Vector3 pos;
        private float percent;

        private void Start() => camTransform = Camera.main.transform;

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            direction = new Vector3(h, 0, v);
            direction = direction.normalized;

            relativeVelocity = Camera.main.transform.TransformDirection(direction);
            relativePosition = Camera.main.transform.TransformPoint(transform.position);
        }

        public Vector3 GetRelativeVelocity() => relativeVelocity;
        public Vector3 GetRelativePosition() => relativePosition;
    }
}