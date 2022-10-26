using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine;


internal abstract class SpawnRandomObjectsAuthoringBase<T> : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    where T : struct, IComponentData, ISpawnSettings
{
    #pragma warning disable 649
    public List<GameObject> Prefabs;
    public float2 Range = new float2(10f);
    public int count;
    public float Timer;
    #pragma warning restore 649

    public unsafe void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnSettings = new T
        {
            Prefabs = conversionSystem.GetPrimaryEntity(Prefabs[0]),
            Position = (Vector2)transform.position,
            Timer = 0,
            TimerDefault = Timer,
            Range = Range,
            Count = count
        };
        Configure(ref spawnSettings);
        dstManager.AddComponentData(entity, spawnSettings);
    }

    protected virtual void Configure(ref T spawnSettings) {}

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) => referencedPrefabs.AddRange(Prefabs);
}

internal unsafe interface ISpawnSettings
{
    Entity Prefabs { get; set; }
    float2 Position { get; set; }
    float2 Range { get; set; }
    int Count { get; set; }
    float Timer { get; set; }
    float TimerDefault { get; set; }
}


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BuildPhysicsWorld))]
internal abstract class SpawnRandomObjectsSystemBase<T> : SystemBase where T : struct, IComponentData, ISpawnSettings
{
    protected BeginInitializationEntityCommandBufferSystem EntityCommandBuffer;
    protected override void OnCreate()
    {
        EntityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate() { }

    protected static void RandomPointsInRange(
        float2 center, float2 range,
        ref NativeArray<float2> positions, int seed = 0)
    {
        var count = positions.Length;
        var random = new Unity.Mathematics.Random((uint)seed);
        for (int i = 0; i < count; i++)
        {
            var mul = math.mul(quaternion.identity, 
                new float3(random.NextFloat2(-range, range).x, random.NextFloat2(-range, range).y, 0));
            positions[i] = center + new float2(mul.x, mul.y);
        }
    }

    protected static void RandomScale(
        ref NativeArray<float> scale, float2 minMaxScale, int seed = 0)
    {
        var count = scale.Length;
        var random = new Unity.Mathematics.Random((uint)seed);
        
        for (int i = 0; i < count; i++)
            scale[i] = random.NextFloat(minMaxScale.x, minMaxScale.y);
    }

    protected static void RandomSustenance(
        ref NativeArray<int> sustenance, float2 minMaxScale, int seed = 0)
    {
        var count = sustenance.Length;
        var random = new Unity.Mathematics.Random((uint)seed);
        
        for (int i = 0; i < count; i++)
            sustenance[i] = (int)random.NextFloat(minMaxScale.x, minMaxScale.y);
    }
    protected unsafe static Entity GetRandomEntity(
        ref T settings, int seed = 0)
    {
        var random = new Unity.Mathematics.Random((uint)seed);
        return settings.Prefabs;
    }

    protected virtual int GetRandomSeed()
    {
        var seed = 0;
        seed = UnityEngine.Random.Range(-9999, 9999);
        return seed;
    }
}