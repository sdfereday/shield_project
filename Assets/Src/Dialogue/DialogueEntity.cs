using UnityEngine;
using Game.Constants;

namespace Game.Dialogue
{
    public class DialogueEntity : MonoBehaviour
    {
        public string startDialogueById;

        private void Start()
        {
            if (string.IsNullOrEmpty(startDialogueById))
            {
                throw new UnityException(GlobalConsts.ERROR_NO_DIALOGUE_START_ID);
            }
        }
    }
}