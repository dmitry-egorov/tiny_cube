using System;
using Plugins.Systematiq;
using UnityEngine;
using UnityEngine.Assertions;

public class Subject: MonoBehaviour
{
    public bool Has<TC>() where TC : QComponent => GetComponent<TC>() != null;
    public bool Has(Type tc) => GetComponent(tc) != null;

    public bool TryGet<TC>(out TC tc) where TC : QComponent => (tc = GetComponent<TC>()) != null;
    
    public T Add<T>() where T: QComponent
    {
        Assert.IsFalse(Has<T>());
        return gameObject.AddComponent<T>();
    }

    public T GetOrAdd<T>() where T : QComponent => TryGet(out T c) ? c : gameObject.AddComponent<T>();

    public void OnEnable() => _index = QManager.Register(this);
    public void OnDisable() => QManager.Unregister(_index);

    private int _index;
}
