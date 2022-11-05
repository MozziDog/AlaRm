[System.Serializable]
public class SerializablePair<TKey, TValue>
{
    public SerializablePair()
    {
    }

    public SerializablePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    public TKey Key;
    public TValue Value;
}