using ShipSimulator;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Mathematics;
using Unity.Transforms;

internal class SpawnRandomObjectsSystem : SpawnRandomObjectsSystemBase<SpawnSettings>, ISingleJobDescription
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var rand = GetRandomSeed();
        var commandBuffer = EntityCommandBuffer.CreateCommandBuffer().AsParallelWriter();
        Entities.WithBurst().ForEach((Entity _entity, int entityInQueryIndex, ref SpawnSettings _spawnSettings) =>
        {
            _spawnSettings.Timer -= deltaTime;
            if (_spawnSettings.Timer <= 0)
            {
                var count = _spawnSettings.Count;

                //OnBeforeInstantiatePrefab(ref _spawnSettings);


                var positions = new NativeArray<float2>(count, Allocator.Temp);
                var scale = new NativeArray<float>(count, Allocator.Temp);
                var sustenance = new NativeArray<int>(count, Allocator.Temp);
                RandomPointsInRange(_spawnSettings.Position, _spawnSettings.Rotation, _spawnSettings.Range,
                    ref positions, rand);
                RandomScale(ref scale, _spawnSettings.MinMaxScale, rand);
                RandomSustenance(ref sustenance, _spawnSettings.MinMaxSustenance, rand);

                for (int i = 0; i < count; i++)
                {
                    var pos = positions[i];
                    var ent = commandBuffer.Instantiate(entityInQueryIndex, _spawnSettings.Prefab);

                    commandBuffer.SetComponent(entityInQueryIndex, ent,
                        new Translation {Value = new float3(pos.x, pos.y, 0)});
                    commandBuffer.AddComponent(entityInQueryIndex, ent, new Scale {Value = scale[i]});
                    var powerData = new PowerPointData
                        {Position = pos, Size = scale[i], Sustenance = sustenance[i]};
                    commandBuffer.SetComponent(entityInQueryIndex, ent, powerData);
                }
                _spawnSettings.Timer = _spawnSettings.TimerDefault;
            }
        }).ScheduleParallel();
        EntityCommandBuffer.AddJobHandleForProducer(Dependency);
    }
}