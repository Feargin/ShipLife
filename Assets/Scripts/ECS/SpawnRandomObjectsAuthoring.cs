using System.Collections.Generic;
using NativeQuadTree;
using NaughtyAttributes;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine;

internal class SpawnRandomObjectsAuthoring : SpawnRandomObjectsAuthoringBase<SpawnSettings>
{
}

internal abstract class SpawnRandomObjectsAuthoringBase<T> : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    where T : struct, IComponentData, ISpawnSettings
{
    #pragma warning disable 649
    public GameObject prefab;
    public float2 Range = new float2(10f);
    [MinMaxSlider(0.2f, 2.5f)]
    public Vector2 MinMaxScale;
    [MinMaxSlider(1, 12f)]
    public Vector2 MinMaxSustenance;
    public int count;
    public float Timer;
    #pragma warning restore 649

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnSettings = new T
        {
            Prefab = conversionSystem.GetPrimaryEntity(prefab),
            Position = (Vector2)transform.position,
            Rotation = transform.rotation,
            MinMaxScale = MinMaxScale,
            Timer = 0,
            TimerDefault = Timer,
            MinMaxSustenance = MinMaxSustenance,
            Range = Range,
            Count = count
        };
        Configure(ref spawnSettings);
        dstManager.AddComponentData(entity, spawnSettings);
    }

    internal virtual void Configure(ref T spawnSettings) {}

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) => referencedPrefabs.Add(prefab);
}

internal interface ISpawnSettings
{
    Entity Prefab { get; set; }
    float2 Position { get; set; }
    float2 MinMaxScale { get; set; }
    float2 MinMaxSustenance { get; set; }
    quaternion Rotation { get; set; }
    float2 Range { get; set; }
    int Count { get; set; }
    float Timer { get; set; }
    float TimerDefault { get; set; }
}

internal struct SpawnSettings : IComponentData, ISpawnSettings
{
    public Entity Prefab { get; set; }
    public float2 Position { get; set; }
    public float2 MinMaxScale { get; set; }
    public float2 MinMaxSustenance { get; set; }
    public quaternion Rotation { get; set; }
    public float2 Range { get; set; }
    public int Count { get; set; }
    public float Timer { get; set; }
    public float TimerDefault { get; set; }
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BuildPhysicsWorld))]
internal abstract class SpawnRandomObjectsSystemBase<T> : SystemBase where T : struct, IComponentData, ISpawnSettings
{
    //public QuadTree<PowerPointData> Tree = new QuadTree<PowerPointData>(10, new Rect(float2.zero, new float2( 400, 400)));
    protected BeginInitializationEntityCommandBufferSystem EntityCommandBuffer;
    protected override void OnCreate()
    {
        EntityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        
    }

    protected static void RandomPointsInRange(
        float2 center, quaternion orientation, float2 range,
        ref NativeArray<float2> positions, int seed = 0)
    {
        var count = positions.Length;
        var random = new Unity.Mathematics.Random((uint)seed);
        for (int i = 0; i < count; i++)
        {
            var mul = math.mul(orientation,
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

    protected virtual int GetRandomSeed()
    {
        var seed = 0;
        seed = UnityEngine.Random.Range(-9999, 9999);
        return seed;
    }
    //protected virtual void OnBeforeInstantiatePrefab(ref T spawnSettings) {}
    //protected virtual void ConfigureInstance(Entity instance, ref T spawnSettings) {}
}