using System.Collections.Generic;
using TMPro;
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
                    CollisionPickupShip(entityA, entityB);
                }
                else if (AllPickups.HasComponent(entityB) && AllShips.HasComponent(entityA))
                {
                    CollisionPickupShip(entityB, entityA);
                }
            }

            private void CollisionPickupShip(Entity entityA, Entity entityB)
            {
                var pointData = PowerPointDataFromEntity[entityA];
                var capacityData = CpacityDataFromEntity[entityB];

                capacityData.CapacityPower += pointData.Sustenance;
                EntityCommandBuffer.SetComponent(entityB, capacityData);
                EntityCommandBuffer.RemoveComponent<HasTarget>(entityB);
                EntityCommandBuffer.DestroyEntity(entityA);
            }
        }
    }
    
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class BaseOnTriggerSystem : JobComponentSystem
    {
        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;
        private ComponentDataFromEntity<CapacityData> _cpacityDataFromEntity;
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            _cpacityDataFromEntity = GetComponentDataFromEntity<CapacityData>();
            var _ecb = _commandBufferSystem.CreateCommandBuffer();
            var job = new BaseOnTriggerJob
            {
                AllBase = GetComponentDataFromEntity<TagBase>(true),
                AllShips = GetComponentDataFromEntity<TagShip>(true),
                EntityCommandBuffer = _ecb,
                CpacityDataFromEntity = _cpacityDataFromEntity,
                
            }.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
            
            job.Complete();
            
            _commandBufferSystem.AddJobHandleForProducer(job);
            return job;
        }
        
        private struct BaseOnTriggerJob : ITriggerEventsJob
        {
            [ReadOnly] 
            public ComponentDataFromEntity<TagBase> AllBase;
            [ReadOnly] 
            public ComponentDataFromEntity<TagShip> AllShips;
            public EntityCommandBuffer EntityCommandBuffer;
      
            public ComponentDataFromEntity<CapacityData> CpacityDataFromEntity;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                if (AllBase.HasComponent(entityA) && AllShips.HasComponent(entityB))
                {
                    CollisionShipWithBase(entityB);
                }
                else if (AllBase.HasComponent(entityB) && AllShips.HasComponent(entityA))
                {
                    CollisionShipWithBase(entityA);
                }
            }

            private void CollisionShipWithBase(Entity ship)
            {
                var capacityData = CpacityDataFromEntity[ship];
                if(capacityData.CapacityPower < capacityData.CapacityMax)
                    return;
                    
                capacityData.CapacityPower = 0;
                EntityCommandBuffer.SetComponent(ship, capacityData);
                EntityCommandBuffer.RemoveComponent<HasTarget>(ship);
            }
        }
    }
}