using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace ShipSimulator
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class PickupOnTriggerSystem : JobComponentSystem
    {
        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;
        private ComponentDataFromEntity<PowerPointData> _powerPointDataFromEntity;
        private ComponentDataFromEntity<CapacityData> _cpacityDataFromEntity;
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
        // private TargetData _targetData;

        protected override void OnCreate()
        {
            base.OnCreate();
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            _powerPointDataFromEntity = GetComponentDataFromEntity<PowerPointData>();
            _cpacityDataFromEntity = GetComponentDataFromEntity<CapacityData>();
            var _ecb = _commandBufferSystem.CreateCommandBuffer();
            
            var job = new PickupOnTriggerJob
            {
                AllPickups = GetComponentDataFromEntity<TagPickup>(true),
                AllShips = GetComponentDataFromEntity<TagShip>(true),
                EntityCommandBuffer = _ecb,
                PowerPointDataFromEntity = _powerPointDataFromEntity,
                CpacityDataFromEntity = _cpacityDataFromEntity,
                
            }.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
            
            job.Complete();

            // Entities.WithoutBurst().ForEach((Entity _entity, ref TargetData _targetData) =>
            // {
            //     if (!this._targetData.Target.Position.Equals(float2.zero) && _targetData.Target.Position.Equals( this._targetData.Target.Position))
            //     {
            //         Debug.Log(2222);
            //         _ecb.SetComponent(_entity, new TargetData());
            //     }
            // }).Run();
            
            _commandBufferSystem.AddJobHandleForProducer(job);
            return job;
        }
        
        private struct PickupOnTriggerJob : ITriggerEventsJob
        {
            [ReadOnly] 
            public ComponentDataFromEntity<TagPickup> AllPickups;
            [ReadOnly] 
            public ComponentDataFromEntity<TagShip> AllShips;
            public EntityCommandBuffer EntityCommandBuffer;
      
            [ReadOnly] 
            public ComponentDataFromEntity<PowerPointData> PowerPointDataFromEntity;
            [ReadOnly] 
            public ComponentDataFromEntity<CapacityData> CpacityDataFromEntity;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                
                Entity entityA = triggerEvent.EntityA; 
                Entity entityB = triggerEvent.EntityB;

                if (AllPickups.HasComponent(entityA) && AllPickups.HasComponent(entityB))
                    return;

                if (AllPickups.HasComponent(entityA) && AllShips.HasComponent(entityB))
                {
                    var pointData = PowerPointDataFromEntity[entityA];
                    var capacityData = CpacityDataFromEntity[entityB];

                    capacityData.CapacityPower += pointData.Sustenance;
                    EntityCommandBuffer.SetComponent(entityB, capacityData);
                    EntityCommandBuffer.RemoveComponent<HasTarget>(entityB);
                    EntityCommandBuffer.DestroyEntity(entityA);
                    Debug.Log(11111);
                }
                else if (AllPickups.HasComponent(entityB) && AllShips.HasComponent(entityA))
                {
                    var pointData = PowerPointDataFromEntity[entityB];
                    var capacityData = CpacityDataFromEntity[entityA];
                    
                    capacityData.CapacityPower += pointData.Sustenance;
                    EntityCommandBuffer.SetComponent(entityA, capacityData);
                    EntityCommandBuffer.RemoveComponent<HasTarget>(entityA);
                    EntityCommandBuffer.DestroyEntity(entityB);
                }
            }
        }
    }
}