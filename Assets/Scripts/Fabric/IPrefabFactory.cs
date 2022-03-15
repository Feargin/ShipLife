using DarckPower.Strategy;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Fabric
{
    public interface IPrefabFactory
    {
        FactoryStrategies Create(Entity prefab, LocalToParent parent);
        FactoryStrategies Create(Entity prefab, float3 position, quaternion rotation, LocalToParent parent);
        FactoryStrategies Create(Entity prefab, Vector3 position, Transform parent);
        T Create<T>(T prefab, LocalToParent parent) where T : IComponentData;
        T Create<T>(T prefab, float3 position, LocalToParent parent) where T : IComponentData;
        T Create<T>(T prefab, float3 position, quaternion rotation, LocalToParent parent) where T : IComponentData;
    }
}