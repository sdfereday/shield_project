using UnityEngine;
using Game.Constants;

namespace Game.Toolbox.Helpers
{
    public class DontDestroy : MonoBehaviour
    {
        private string TagName;

        private void Awake()
        {
            if (!string.IsNullOrEmpty(transform.tag))
            {
                TagName = transform.tag;
            }

            if (string.IsNullOrEmpty(TagName))
            {
                throw new UnityException(GlobalConsts.ERROR_STRING_EMPTY);
            }

            GameObject[] objs = GameObject.FindGameObjectsWithTag(TagName);

            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}