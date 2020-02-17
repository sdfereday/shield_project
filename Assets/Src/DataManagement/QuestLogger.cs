using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Game.DataManagement
{
    [System.Serializable]
    public class LogEntry
    {
        public string Value;
    }

    public class QuestLogger : MonoBehaviour
    {
        public List<LogEntry> Entries;

        private void Awake()
        {
            Entries = new List<LogEntry>();
        }

        public bool HasEntry(string val)
            => Entries.Any(x => x.Value == val);

        public void AddEntry(LogEntry newEntry)
        {
            if (Entries.Any(x => x.Value == newEntry.Value))
            {
                throw new UnityException("You can't have a duplicate log Id.");
            }

            Entries.Add(newEntry);
        }
    }
}