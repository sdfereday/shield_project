namespace Game.Constants
{
    public static class GlobalConsts
    {
        public const string MAP_TAG = "Map";
        public const string PLAYER_TAG = "Player";
        public const string CONTEXT_TAG = "GameContext";
        public const string SCENE_CONTEXT_TAG = "SceneContext";
        public const string UNTAGGED_TAG = "Untagged";
        public const string DEFAULT_PLAYER_START = "PlayerStart";
        public const string ERROR_STRING_EMPTY = "You must specify a string for this value - Location: ";
        public const string ERROR_COMPONENT_NULL = "You must set a component here - Location: ";
        public const string ERROR_CAM_CONTAINER_NULL = "You need to specify a container object for the related camera position - Location: ";
        public const string ERROR_NO_PLAYER_START = "You are missing a player start, at least one should be added - Location: ";
        public const string ERROR_KEYITEM_UNIQUE = "You cannot hold more than one key item - Location: ";
        public const string SESSION_CACHE_ERROR_DUPLICATE = "You're trying to add an item of the same id to the session cache, this isn't allowed - Location:";
        public const string LIST_WAS_EMPTY = "This list that was passed to the method should not be empty - Location: ";
        public const string UI_TAG_DIALOGUE_BOX = "UI.DialogueBox";
        public const string UI_NAME_FIELD = "Name";
        public const string UI_DIALOGUE_FIELD = "DialogueText";
        public const string UI_BUTTON_CONTAINER = "ButtonBox";
        public const string DEFAULT_CAM_CONTAINER_TAG = "Scene.DefaultCamContainer";
        public const string ERROR_NO_DIALOGUE_START_ID = "This entity needs a dialogue Id before it can start talking.";
    }
}