[System.Serializable]
public class KeyValuePair<TKey, TValue>
{
    public KeyValuePair()
    {
    }

    public KeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    public TKey Key;
    public TValue Value;
}