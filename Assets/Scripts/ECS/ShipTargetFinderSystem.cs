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
    
    [Serializable]
    public struct CapacityData : IComponentData
    {
        public float CapacityPower;
    }

    [Serializable]
    public struct TargetData : IComponentData
    {
        public PowerPointData Target;
    }

    [UpdateAfter(typeof(ShipMoveSystem))]
    public sealed class ShipTargetFinderSystem : SystemBase
    {
        private float _timerFind = 0;
        // private float _timerReset = 0;
        private const float findStep = 0.5f;
        // private const float resetStep = 5f;
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
        public EntityCommandBuffer EntityCommandBuffer;

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
            //_timerReset += deltaTime;
            EntityCommandBuffer = _commandBufferSystem.CreateCommandBuffer();
            if (_timerFind > findStep)
            {
                Entities.WithoutBurst().ForEach(
                    (Entity _entity, LocalToWorld _transform, ref SearchData _searchData, ref TargetData _targetData) =>
                    {
                        // if (_targetData.Target.Position.Equals(float2.zero) && !reg)
                        // {
                            _targetData.Target = Find(ref _transform, ref _searchData);
                        // }
                        // else if(_timerReset > resetStep)
                        // {
                        //     EntityCommandBuffer.RemoveComponent<TargetData>(_entity);
                        //     EntityCommandBuffer.AddComponent(_entity, new TargetData());
                        //     Debug.Log($"{_entity.ToString()} --- {_targetData.Target.Position}");
                        //     reg = !reg;
                        //     _timerReset = 0;
                        // }
                    }).Run();
                _timerFind = 0;
            }
        }

        private PowerPointData Find(ref LocalToWorld transform, ref SearchData data)
        {
            var pos = new float2(transform.Position.x + 0.5f, transform.Position.y);
            using (var readyToCheck = GetEntityQuery(new ComponentType[] {typeof(PowerPointData)})
                .ToEntityArray(Allocator.TempJob))
            {
                var count = readyToCheck.Length;

                var nReadyToCheck = new NativeArray<PowerPointData>(count, Allocator.TempJob);
                for (var i = 0; i < count; i++)
                {
                    var pointData = EntityManager.GetComponentData<PowerPointData>(readyToCheck[i]);
                    nReadyToCheck[i] = pointData;
                }

                if (readyToCheck.Length == 0)
                    return default;

                var resultIndex = new NativeArray<int>(count, Allocator.TempJob);
                var job = new CalculationOfAcceptableGoals
                {
                    ReadyToCheck = nReadyToCheck,
                    Pos = pos,
                    Forward = new float2(transform.Forward.x, transform.Forward.y),
                    Data = data,
                    ResultIndex = resultIndex
                }.Schedule(count, 60);
                job.Complete();

                var e = (int)data.Radius + 1;
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

                return index > 0 ? nReadyToCheck[index] : default;
            }
        }

        [BurstCompile]
        private struct CalculationOfAcceptableGoals : IJobParallelFor
        {
            [ReadOnly] public NativeArray<PowerPointData> ReadyToCheck;
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

                var pointPos = ReadyToCheck[index].Position;
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

