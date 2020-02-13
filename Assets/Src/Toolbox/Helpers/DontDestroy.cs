using UnityEngine;
using Game.Constants;

/* It's worth noting that any 'assigned' fields in the inspector
 * that reference to any components attached to objects that use
 * this script will not function correctly, presumably due to
 * memory assignment. So you must get them via GetComponent
 * on the script that needs access to them.
 */
namespace Game.Toolbox.Helpers
{
    public class DontDestroy : MonoBehaviour
    {
        private string TagName;

        private void Awake()
        {
            if (string.IsNullOrEmpty(transform.tag) || transform.CompareTag(GlobalConsts.UNTAGGED_TAG))
            {
                throw new UnityException(GlobalConsts.ERROR_STRING_EMPTY + transform.name);
            }

            TagName = transform.tag;

            GameObject[] objs = GameObject.FindGameObjectsWithTag(TagName);

            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}