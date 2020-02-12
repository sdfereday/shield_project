using UnityEngine;

/* Ref: http://wiki.unity3d.com/index.php?title=OffsetVanishingPoint */
namespace Game.GameCamera
{
    public class KBCamera : MonoBehaviour
    {
        // Do a pan from left to right
        // float panSpeed = .15f;
        private float panLimit = .1f;
        private float tx = 0f;
        private float ty = 0f;
        private float x = 0f;
        private float y = 0f;
        private float colSizeX = 0f;
        private float colSizeY = 0f;
        private Vector3 colSize;

        public Transform player;
        public GameObject map;
        public RelativeCam rCam;

        private void Start()
        {
            /*
             * Note to self: Currently not using the 'y' axis ('y' becomes 'z' in this 3D space usage),
             * this may become a problem in future so keep note.
             */
            colSize = map.GetComponent<BoxCollider>().size;
            colSizeX = colSize.x / 2;
            colSizeY = colSize.z / 2;
        }

        private void Update()
        {
            //Debug.Log(Camera.main.transform.forward); // z
            //Debug.Log(Camera.main.transform.right); // x

            Vector3 camF = Camera.main.transform.forward;
            Vector3 camR = Camera.main.transform.right;

            // Reset y as we don't care about it right now
            camF.y = 0;
            camR.y = 0;

            // Convert all relative transforms to local space so they don't fall out of positional sync
            Vector3 testF = transform.InverseTransformDirection(camF);
            Vector3 testR = transform.InverseTransformDirection(camR);

            Vector3 pT = transform.InverseTransformDirection(player.transform.position);

            // Normalize for calculations
            camF = testF.normalized;
            camR = testR.normalized;

            var rel = (camF * pT.z + camR * pT.x);

            tx = Mathf.InverseLerp(-colSizeX, colSizeX, -rel.x);
            x = Mathf.Lerp(-panLimit, panLimit, tx);

            ty = Mathf.InverseLerp(-colSizeY, colSizeY, -rel.z);
            y = Mathf.Lerp(-panLimit, panLimit, ty);

            SetVanishingPoint(Camera.main, new Vector2(x, y));
        }

        private void SetVanishingPoint(Camera cam, Vector2 perspectiveOffset)
        {
            var m = cam.projectionMatrix;

            float w = 2 * cam.nearClipPlane / m.m00;
            float h = 2 * cam.nearClipPlane / m.m11;

            float left = -w / 2 - perspectiveOffset.x;
            float right = left + w;
            float bottom = -h / 2 - perspectiveOffset.y;
            float top = bottom + h;

            cam.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
        }

        private static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
        {
            float x = (2.0f * near) / (right - left);
            float y = (2.0f * near) / (top - bottom);
            float a = (right + left) / (right - left);
            float b = (top + bottom) / (top - bottom);
            float c = -(far + near) / (far - near);
            float d = -(2.0f * far * near) / (far - near);
            float e = -1.0f;

            Matrix4x4 m;
            m = Matrix4x4.zero;

            m[0, 0] = x; m[0, 1] = 0.0f; m[0, 2] = a; m[0, 3] = 0.0f;
            m[1, 0] = 0.0f; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0.0f;
            m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = c; m[2, 3] = d;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = e; m[3, 3] = 0.0f;

            return m;
        }
    }
}