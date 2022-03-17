using Unity.Entities;

namespace ShipSimulator
{
    internal sealed class CollectionMaterialSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
        private EntityCommandBuffer.ParallelWriter _entityCommandBuffer;
        protected override void OnCreate()
        {
            base.OnCreate();
            _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            var entityCommandBuffer = _commandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            Entities.WithBurst().ForEach((Entity entity, int entityInQueryIndex, ref CapacityData capacityData) =>
            {
                if(capacityData.CapacityPower >= capacityData.CapacityMax)
                {entityCommandBuffer.AddComponent(entityInQueryIndex, entity, new HasTarget{TargetEntity = capacityData.Base});}
            }).ScheduleParallel();
            Entities.WithBurst().ForEach((Entity entity, int entityInQueryIndex, ref CapacityData capacityData, ref HasTarget hasTarget) =>
            {
                if (capacityData.CapacityPower < capacityData.CapacityMax &&
                    hasTarget.TargetEntity == capacityData.Base)
                    entityCommandBuffer.RemoveComponent<HasTarget>(entityInQueryIndex, entity);
            }).ScheduleParallel();
            _commandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}