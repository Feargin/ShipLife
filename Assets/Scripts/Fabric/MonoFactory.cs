using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using ModestTree;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Fabric
{
    public sealed class MonoFactory //: IPrefabFactory
    {
        private readonly DiContainer _container;

        // private Dictionary<Type, IFactoryStrategy> _strategy = new Dictionary<Type, IFactoryStrategy>();

        public MonoFactory(DiContainer container)
        {
            // var types = Assembly.GetExecutingAssembly().GetTypes().Where(type => 
            //     typeof (IPrefabFactory).IsAssignableFrom(type) && type != typeof (IFactoryStrategy)).ToArray();
            //
            // foreach (var v in types)
            // {
            //     var obj = Activator.CreateInstance(v);
            //     _strategy.Add(typeof(IFactoryStrategy), obj as IFactoryStrategy);
            // }
            _container = container;
        }

        // public FactoryStrategies Create(Entity prefab, Transform parent = null)
        // {
        //     var obj = (parent != null) ? _container.InstantiatePrefab(prefab, parent) : _container.InstantiatePrefab(prefab);
        //     FactoryStrategies factoryStrategies = new FactoryStrategies(obj);
        //     return factoryStrategies;
        // }
        //
        // public FactoryStrategies Create(Entity prefab, Vector3 position, Transform parent = null)
        // {
        //     var obj = (parent != null) ? _container.InstantiatePrefab(prefab, parent) : _container.InstantiatePrefab(prefab);
        //     obj.transform.position = position;
        //     FactoryStrategies factoryStrategies = new FactoryStrategies(obj);
        //     return factoryStrategies;
        // }
        //
        // public FactoryStrategies Create(Entity prefab, Vector3 position, Quaternion rotation, Transform parent)
        // {
        //     var obj = _container.InstantiatePrefab(prefab, position, rotation, parent);
        //     FactoryStrategies factoryStrategies = new FactoryStrategies(obj);
        //     return factoryStrategies;
        // }
        //
        // public T Create<T>(T prefab, Transform parent = null) where T : MonoBehaviour
        // {
        //     var obj = (parent != null) ? _container.InstantiatePrefab(prefab, parent) : _container.InstantiatePrefab(prefab);
        //     return obj.GetComponent<T>();
        // }
        //
        // public T Create<T>(T prefab, Vector3 position, Transform parent = null) where T : MonoBehaviour
        // {
        //     var obj = (parent != null) ? _container.InstantiatePrefab(prefab, parent) : _container.InstantiatePrefab(prefab);
        //     obj.transform.position = position;
        //     return obj.GetComponent<T>();
        // }
        //
        // public T Create<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : MonoBehaviour
        // {
        //     var obj = _container.InstantiatePrefab(prefab, position, rotation, parent);
        //     return obj.GetComponent<T>();
        // }
    }
}