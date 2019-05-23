using System;
using UnityEngine;

public class Subject: MonoBehaviour
{
    public bool Has<TC>() where TC : QComponent => GetComponent<TC>() != null;
    public bool Has(Type tc) => GetComponent(tc) != null;

    public bool TryGet<TC>(out TC tc) where TC : QComponent => (tc = GetComponent<TC>()) != null;

    public void OnEnable() => _index = QManager.Register(this);
    public void OnDisable() => QManager.Unregister(_index);

    private int _index;
}
