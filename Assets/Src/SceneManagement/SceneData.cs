using UnityEngine;
using Game.Constants;

namespace Game.SceneManagement
{
    public class SceneData : MonoBehaviour
    {
        public GameObject defaultCameraContainer;

        private void Awake()
        {
            defaultCameraContainer = defaultCameraContainer == null ?
                GameObject.FindGameObjectWithTag(GlobalConsts.DEFAULT_CAM_CONTAINER_TAG) : defaultCameraContainer;
        }
    }
}
