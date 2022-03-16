using System;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Zenject;
using Object = System.Object;

namespace ShipSimulator
{
    [Serializable]
    public struct SearchData : IComponentData
    {
        public float Radius;
        public float ViewingAngle;
    }
    
    public struct HasTarget : IComponentData 
    {
        public Entity TargetEntity;
    }
    
    [Serializable]
    public struct CapacityData : IComponentData
    {
        public float CapacityPower;
    }

    [UpdateAfter(typeof(ShipMoveSystem))]
    public sealed class ShipTargetFinderSystem : SystemBase
    {
    //     private struct EntityWithPosition 
    //     {
    //         public Entity entity;
    //         public float3 position;
    //     }
    //     
    //     private EntityQuery _group;
    //     protected override void OnCreate()
    //     {
    //         var query = new EntityQueryDesc
    //         {
    //             None = new ComponentType[] {typeof(Frozen)},
    //             All = new ComponentType[] {typeof(TargetData), ComponentType.ReadOnly<TargetData>()}
    //         };
    //         
    //         _group = GetEntityQuery(query);
    //     }
    //     
    //     protected override void OnUpdate()
    //     {
    //         var findTargetChunkJob = new FindTargetChunkJob
    //         {
    //             DeltaTime = Time.DeltaTime,
    //             PowerPointData = GetComponentTypeHandle<PowerPointData>(),
    //             CapacityData = GetComponentTypeHandle<CapacityData>(),
    //             ShipEntities = GetComponentTypeHandle<ShipTag>(),
    //             PickupEntities = GetComponentTypeHandle<PickupTag>()
    //         };
    //
    //         Dependency = findTargetChunkJob.ScheduleParallel(_group, 1, Dependency);
    //     }
    //     
    //     
    //     private struct FindTargetChunkJob : IJobEntityBatch
    //     {
    //         public float DeltaTime;
    //         public ComponentTypeHandle<PowerPointData> PowerPointData;
    //         public ComponentTypeHandle<CapacityData> CapacityData;
    //         [ReadOnly] public ComponentTypeHandle<ShipTag> ShipEntities;
    //         [ReadOnly] public ComponentTypeHandle<PickupTag> PickupEntities;
    //
    //         [BurstCompile]
    //         public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
    //         {
    //         }
    //     }
    





    private float _timerFind = 0;
    private const float findStep = 0.2f;
    private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
    private EntityCommandBuffer EntityCommandBuffer;

    protected override void OnCreate()
    {
        base.OnCreate();
        _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var reg = false;
        _timerFind += deltaTime;
        
        EntityCommandBuffer = _commandBufferSystem.CreateCommandBuffer();
        if (true)
        {
            Entities.WithoutBurst().WithNone<HasTarget>().WithAll<TagShip>().ForEach(
                (Entity _entity, LocalToWorld _transform, ref SearchData _searchData) =>
                {
                    Find(ref _entity, ref _transform, ref _searchData);
                }).Run();
            _timerFind = 0;
        }
    }

    private void Find(ref Entity entity, ref LocalToWorld transform, ref SearchData data)
    {
        var pos = new float2(transform.Position.x + 0.5f, transform.Position.y);
        using var readyToCheck = GetEntityQuery(ComponentType.ReadOnly<PowerPointData>(),
            ComponentType.ReadOnly<TagPickup>()).ToEntityArray(Allocator.TempJob);
        var count = readyToCheck.Length;

        var posReadyToCheck = new NativeArray<float2>(count, Allocator.TempJob);
        for (int i = 0; i < count; i++)
        {
            var p = EntityManager.GetComponentData<Translation>(readyToCheck[i]).Value;
            posReadyToCheck[i] = new float2(p.x, p.y);
        }

        if (readyToCheck.Length == 0)
            return;

        var resultIndex = new NativeArray<int>(count, Allocator.TempJob);
        var job = new CalculationOfAcceptableGoals
        {
            ReadyToCheck = posReadyToCheck,
            Pos = pos,
            Forward = new float2(transform.Forward.x, transform.Forward.y),
            Data = data,
            ResultIndex = resultIndex
        }.Schedule(count, 60);
        job.Complete();

        var e = (int) data.Radius + 1;
        var index = 0;
        for (var i = 0; i < count; i++)
        {
            var res = resultIndex[i];
            if (e > res && res != 0)
            {
                e = res;
                index = i;
            }
        }
        if(index > 0)
            EntityCommandBuffer.AddComponent(entity, new HasTarget {TargetEntity = readyToCheck[index]});
        
        resultIndex.Dispose();
        posReadyToCheck.Dispose();
    }

    [RequireComponentTag(typeof(TagPickup))]
    [ExcludeComponent(typeof(HasTarget))]
    [BurstCompile]
    private struct CalculationOfAcceptableGoals : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float2> ReadyToCheck;
        [ReadOnly] public float2 Pos;
        [ReadOnly] public float2 Forward;
        [ReadOnly] public SearchData Data;

        [WriteOnly] public NativeArray<int> ResultIndex;

        public void Execute(int index)
        {
            ResultIndex[index] = 0;
            var radius = Data.Radius;
            var radiusSq = radius * radius;
            var viewingAngle = Data.ViewingAngle;

            var pointPos = ReadyToCheck[index];
            var left = Pos.x - radius;
            var right = Pos.x + radius;
            var up = Pos.y + radius;
            var down = Pos.y - radius;

            var dir = Pos - pointPos;
            var angl = Forward.Angle(dir);

            if (pointPos.x < left
                || pointPos.y < down
                || pointPos.x > right
                || pointPos.y > up)
                return;

            if (angl > viewingAngle)
                return;
            var distance = dir.x * dir.x + dir.y * dir.y;
            if (distance <= radiusSq)
                ResultIndex[index] = (int) math.sqrt(distance);
        }
    }
}


}


