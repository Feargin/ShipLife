using NaughtyAttributes;
using ShipSimulator;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

internal class SpawnShipAuthoring : SpawnRandomObjectsAuthoringBase<SpawnShipSettings>
{
    public float2 PointsSpawn;
    public Team Team;
    [Range(0.01f, 0.5f)]
    public float Mass;
    [Range(50, 300)]
    public float Force;
    [Range(30, 400)]
    public float MaxCapacity;
    [Range(1, 20)]
    public float DeflectionForce;
    [Range(0.2f, 10)]
    public float SpeedRotated;

    protected override void Configure(ref SpawnShipSettings spawnSettings)
    {
        spawnSettings.PointsSpawn = PointsSpawn;
        spawnSettings.Team = Team;
        spawnSettings.Force = Force;
        spawnSettings.Mass = Mass;
        spawnSettings.DeflectionForce = DeflectionForce;
        spawnSettings.SpeedRotated = SpeedRotated;
        spawnSettings.MaxCapacity = (int)MaxCapacity;
    }
}
public enum Team {RED_Team, BLUE_Team}

internal class SpawnShipSystem : SpawnRandomObjectsSystemBase<SpawnShipSettings>, ISingleJobDescription
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var rand = GetRandomSeed();
        var commandBuffer = EntityCommandBuffer.CreateCommandBuffer().AsParallelWriter();
        Entities.WithBurst().ForEach((Entity entity, int entityInQueryIndex, ref SpawnShipSettings spawnShipSettings) =>
        {
            spawnShipSettings.Timer -= deltaTime;
            if (spawnShipSettings.Timer <= 0)
            {
                var positions = new NativeArray<float2>(1, Allocator.Temp);
                var p = new float2(spawnShipSettings.Position.x + 3f, spawnShipSettings.Position.y);
                
                RandomPointsInRange(p, spawnShipSettings.Range, ref positions, rand);
                
                var pos = positions[0];
                var ent = commandBuffer.Instantiate(entityInQueryIndex, GetRandomEntity(ref spawnShipSettings, rand));
        
                commandBuffer.SetComponent(entityInQueryIndex, ent, new MoveData 
                    {
                        Mass = spawnShipSettings.Mass,
                        Force = spawnShipSettings.Force,
                        DeflectionForce = spawnShipSettings.DeflectionForce,
                        SpeedRotated = spawnShipSettings.SpeedRotated
                    });
                commandBuffer.SetComponent(entityInQueryIndex, ent, new CapacityData {CapacityMax = spawnShipSettings.MaxCapacity, Base = entity}); 
                commandBuffer.SetComponent(entityInQueryIndex, ent, new Translation {Value = new float3(pos.x, pos.y, 0)});
                
                spawnShipSettings.Timer = spawnShipSettings.TimerDefault;
            }
        }).ScheduleParallel();
        EntityCommandBuffer.AddJobHandleForProducer(Dependency);
    }
}

internal unsafe struct SpawnShipSettings : IComponentData, ISpawnSettings
{
    public Entity Prefabs { get; set; }
    public float2 Position { get; set; }
    public float2 Range { get; set; }
    public int Count { get; set; }
    public float Timer { get; set; }
    public float TimerDefault { get; set; }
    public float Mass;
    public float Force;
    public float DeflectionForce;
    public float SpeedRotated;
    public int MaxCapacity;
    public float2 PointsSpawn;
    public Team Team;
}