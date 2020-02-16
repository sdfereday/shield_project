using System.Collections.Generic;
using Game.DataManagement;

/* A mock collection of entities that, if game saved, will appear here. Usually
 * you'd see these in a JSON file or something. They only get updated when you
 * actually save the game though. You'll find there's no schema right now
 * for the different items but there 'are' objects pre-built that the system
 * will look for. So basically you can't just throw any old thing in here
 * and expect it to work. It has to be an item that exists in the game. */
namespace Game.MockServices
{
    public static class MockNPCMeta
    {
        public static List<NPCMeta> items = new List<NPCMeta>()
        {
            new NPCMeta()
            {
                Id = "npcId",
                Name = "Jade",
                Description = "Jade loves hats, a clean pair of slacks."
            },
            new NPCMeta()
            {
                Id = "npcId2",
                Name = "Stan",
                Description = "Stan usually sells hats and not much else."
            }
        };
    }
}