using System;
using Plugins.Systematiq;
using Plugins.UnityExtensions;
using UnityEngine;

public class Subject: MonoBehaviour
{
    public bool Has<TC>() where TC : QComponent => this.TryGetComponent<TC>(out var c) && c.isActiveAndEnabled;
    public bool Has(Type tc) => this.TryGetComponent(tc, out var c) && ((Behaviour)c).isActiveAndEnabled;

    public bool TryGet<TC>(out TC c) where TC : QComponent => this.TryGetComponent(out c) && c.isActiveAndEnabled;
    
    public TC Add<TC>() where TC: QComponent
    {
        if (!this.TryGetComponent<TC>(out var c)) 
            return gameObject.AddComponent<TC>();
        
        if (c.isActiveAndEnabled) 
            throw new InvalidOperationException("Component already exists");
            
        c.enabled = true;
        return c;
    }

    public TC GetOrAdd<TC>() where TC : QComponent
    {
        if (!this.TryGetComponent<TC>(out var c)) 
            return gameObject.AddComponent<TC>();

        if (!c.isActiveAndEnabled) c.enabled = true;

        return c;
    }

    public void OnEnable() => _index = QManager.Register(this);
    public void OnDisable() => QManager.Unregister(_index);

    private int _index;
}
