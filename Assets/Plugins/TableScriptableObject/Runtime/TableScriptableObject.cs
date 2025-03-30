using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScriptableObject<T> : TableScriptableObject where T : new()
{
    [SerializeField] private List<T> _tableRecords = new();
    public override IList TableRecords => _tableRecords;

    public override void InitializedIndexAt(int index)
    {
        _tableRecords[index] = new();
    }

    public override void AddItem()
    {
        _tableRecords.Add(new());
    }
}

public abstract class TableScriptableObject : ScriptableObject
{
    public abstract IList TableRecords { get; }

    public abstract void InitializedIndexAt(int index);

    public abstract void AddItem();
}