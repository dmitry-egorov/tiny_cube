using System;
using System.Collections.Generic;
using System.Linq;
using ExtraCollections;
using Reflections;
using UnityEngine;
using UnityEngine.Assertions;

internal static class QManager
{
    public static Subject Subject {get; private set;}
    public static float DeltaTime { get; private set; }

    public static int Register(Subject subject)
    {
        if(_subjects == null) _subjects = new ShuffleList<Subject>();
        return _subjects.Add(subject);
    }
    
    public static void Unregister(int index)
    {
        Assert.IsNotNull(_subjects);
        _subjects.RemoveAt(index);
    }

    public static void Register<T1>(IEnumerable<Type> included, Action<T1> action)
        where T1 : QComponent
    {
        Assert.IsNotNull(_actions);
        
        var ts = included.ToArray();
        
        _actions.Add(() =>
        {
            if (ts.Any(t => !Subject.Has(t)))
                return;

            if (!Subject.TryGet(out T1 c1))
                return;

            action(c1);
        });
    }
    
    public static void Register<T1, T2>(IEnumerable<Type> included, Action<T1, T2> action)
        where T1 : QComponent
        where T2 : QComponent
    {
        Assert.IsNotNull(_actions);
        
        var ts = included.ToArray();
        
        _actions.Add(() =>
        {
            if (ts.Any(t => !Subject.Has(t)))
                return;

            if (!Subject.TryGet(out T1 c1))
                return;
            if (!Subject.TryGet(out T2 c2))
                return;

            action(c1, c2);
        });
    }

    public static void Execute()
    {
        Initialize();
        
        if (_subjects == null)
            return;
        
        foreach (var a in _actions)
        {
            foreach (var s in _subjects)
            {
                Subject = s;
                DeltaTime = Time.deltaTime;//TODO: change to contextual delta time
                a();
            }
        }
    }

    private static void Initialize()
    {
        if (_actions != null) return;
        
        _actions = new List<Action>();
        foreach (var s in AppDomain.CurrentDomain.InstantiateAllDerivedTypes<ISystem>())
        {
            s.Register();
        }
    }

    private static ShuffleList<Subject> _subjects;
    private static List<Action> _actions;

}