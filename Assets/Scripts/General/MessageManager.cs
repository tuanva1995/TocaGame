using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TeeMessageType
{
    OnDataChange = 0,
    /// <summary>
    /// data[0] = GameState
    /// </summary>
    OnChangeState = 1,
    OnBuyIAP = 18
}
public class Message
{
    public TeeMessageType type;
    public object[] data;
    public Message(TeeMessageType type)
    {
        this.type = type;
    }
    public Message(TeeMessageType type, object[] data)
    {
        this.type = type;
        this.data = data;
    }
}
public interface IMessageHandle
{
    void Handle(Message message);
}
public class MessageManager : MonoBehaviour, ISerializationCallbackReceiver
{
    private static MessageManager instance = null;
    [HideInInspector] public List<TeeMessageType> _keys = new List<TeeMessageType>();
    [HideInInspector] public List<List<IMessageHandle>> _values = new List<List<IMessageHandle>>();
    private Dictionary<TeeMessageType, List<IMessageHandle>> subscribers = new Dictionary<TeeMessageType, List<IMessageHandle>>();
    public static MessageManager Instance { get { return instance; } }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void AddSubscriber(TeeMessageType type, IMessageHandle handle)
    {
        if (!subscribers.ContainsKey(type))
            subscribers[type] = new List<IMessageHandle>();
        if (!subscribers[type].Contains(handle))
            subscribers[type].Add(handle);
    }
    public void RemoveSubscriber(TeeMessageType type, IMessageHandle handle)
    {
        if (subscribers.ContainsKey(type))
            if (subscribers[type].Contains(handle))
                subscribers[type].Remove(handle);
    }
    public void SendMessage(Message message)
    {
        if (subscribers.ContainsKey(message.type))
            for (int i = subscribers[message.type].Count - 1; i > -1; i--)
                subscribers[message.type][i].Handle(message);
    }
    public void SendMessageWithDelay(Message message, float delay)
    {
        StartCoroutine(_DelaySendMessage(message, delay));
    }
    private IEnumerator _DelaySendMessage(Message message, float delay)
    {
        yield return new WaitForSeconds(delay);
        SendMessage(message);
    }
    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (var element in subscribers)
        {
            _keys.Add(element.Key);
            _values.Add(element.Value);
        }
    }
    public void OnAfterDeserialize()
    {
    }
}