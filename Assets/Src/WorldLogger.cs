using UnityEngine;
using System.Linq;
using System.Collections.Generic;

// TODO: Maybe just use this for quest logging? Leave the rest like inventory to its self.
// TODO: Change Id to value.
[System.Serializable]
public class LogEntry
{
    public string Id;
    public string Desc;
}

public class WorldLogger : MonoBehaviour
{
    public List<LogEntry> Entries;

    private void Awake()
    {
        Entries = new List<LogEntry>();
    }

    public bool HasEntry(string val)
        => Entries.Any(x => x.Id == val);

    public void AddEntry(LogEntry newEntry)
    {
        if (Entries.Any(x => x.Id == newEntry.Id))
        {
            throw new UnityException("You can't have a duplicate log Id.");
        }

        Entries.Add(newEntry);
    }
}
