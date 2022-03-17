using NaughtyAttributes;
using ShipSimulator;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

internal class SpawnPowerpointsAuthoring : SpawnRandomObjectsAuthoringBase<SpawnPowerPointsSettings>
{
    [MinMaxSlider(0.2f, 1f)]
    public Vector2 MinMaxScale;
    [MinMaxSlider(1, 12)]
    public Vector2 MinMaxSustenance;

    protected override void Configure(ref SpawnPowerPointsSettings spawnSettings)
    {
        spawnSettings.MinMaxScale = MinMaxScale;
        spawnSettings.MinMaxSustenance = MinMaxSustenance;
    }
}

internal class SpawnPowerPointSystem : SpawnRandomObjectsSystemBase<SpawnPowerPointsSettings>, ISingleJobDescription
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var rand = GetRandomSeed();
        var commandBuffer = EntityCommandBuffer.CreateCommandBuffer().AsParallelWriter();
        Entities.WithBurst().ForEach((Entity entity, int entityInQueryIndex, ref SpawnPowerPointsSettings spawnPowerPointsSettings) =>
        {
            spawnPowerPointsSettings.Timer -= deltaTime;
            if (spawnPowerPointsSettings.Timer <= 0)
            {
                var count = spawnPowerPointsSettings.Count;

                var positions = new NativeArray<float2>(count, Allocator.Temp);
                var scale = new NativeArray<float>(count, Allocator.Temp);
                var sustenance = new NativeArray<int>(count, Allocator.Temp);
                RandomPointsInRange(spawnPowerPointsSettings.Position,  spawnPowerPointsSettings.Range,
                    ref positions, rand);
                RandomScale(ref scale, spawnPowerPointsSettings.MinMaxScale, rand);
                RandomSustenance(ref sustenance, spawnPowerPointsSettings.MinMaxSustenance, rand);

                for (int i = 0; i < count; i++)
                {
                    var pos = positions[i];
                    var ent = commandBuffer.Instantiate(entityInQueryIndex, GetRandomEntity(ref spawnPowerPointsSettings, rand));

                    commandBuffer.SetComponent(entityInQueryIndex, ent,
                        new Translation {Value = new float3(pos.x, pos.y, 0)});
                    commandBuffer.AddComponent(entityInQueryIndex, ent, new Scale {Value = scale[i]});
                    var powerData = new PowerPointData
                        {Position = pos, Size = scale[i], Sustenance = sustenance[i]};
                    commandBuffer.SetComponent(entityInQueryIndex, ent, powerData);
                }
                spawnPowerPointsSettings.Timer = spawnPowerPointsSettings.TimerDefault;
            }
        }).ScheduleParallel();
        EntityCommandBuffer.AddJobHandleForProducer(Dependency);
    }
}
internal unsafe struct SpawnPowerPointsSettings : IComponentData, ISpawnSettings
{
    public Entity Prefabs { get; set; }
    public float2 Position { get; set; }
    public float2 MinMaxScale;
    public float2 MinMaxSustenance;
    public float2 Range { get; set; }
    public int Count { get; set; }
    public float Timer { get; set; }
    public float TimerDefault { get; set; }
}