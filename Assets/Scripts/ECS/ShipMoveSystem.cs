using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShipSimulator
{
    [System.Serializable]
    public struct MoveData : IComponentData
    {
        public float Mass;
        public float Force;
        public float DeflectionForce;
        public float SpeedRotated;
    }
    public struct DirectionData : IComponentData
    {
        public float3 Direction;
    }

    [UpdateAfter(typeof(Unity.Physics.Systems.EndFramePhysicsSystem))]
    public sealed class ShipMoveSystem : SystemBase
    {
        private quaternion _finalRotation;
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
        private EntityCommandBuffer EntityCommandBuffer;
        protected override void OnCreate()
        {
            base.OnCreate();
            _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            float _time = UnityEngine.Time.smoothDeltaTime;
            EntityCommandBuffer = _commandBufferSystem.CreateCommandBuffer();

            Entities.WithoutBurst().ForEach(
                (
                    Entity _entity,
                    ref LocalToWorld _localToWorld,
                    ref Rotation _rotation,
                    ref PhysicsVelocity _physicsVelocity,
                    ref PhysicsMass _physicsMass,
                    ref DirectionData _directionData,
                    ref Translation translation,
                    in MoveData _moveData) =>
                {
                    _physicsMass.InverseMass = _moveData.Mass;
                    
                    var direction = _directionData.Direction;
                        var pos = _localToWorld.Position;
                        if (pos.z != null)
                            translation.Value.z = 0;
                        float3 rand = new float3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0);
                        if (!EntityManager.HasComponent(_entity, typeof(HasTarget)))
                            direction = math.normalize(rand * _moveData.DeflectionForce) + direction;
                        else
                        {
                            var targetData = EntityManager.GetComponentData<HasTarget>(_entity);
                            if(EntityManager.Exists(targetData.TargetEntity) && EntityManager.HasComponent(targetData.TargetEntity, typeof(LocalToWorld)))
                            {
                                var targetTranslation =
                                    EntityManager.GetComponentData<LocalToWorld>(targetData.TargetEntity);
                                direction = math.normalize(targetTranslation.Position - pos);
                            }
                            else
                                EntityCommandBuffer.RemoveComponent<HasTarget>(_entity);
                        }
                        
                        var forward = _localToWorld.Right;
                        PhysicsComponentExtensions.ApplyLinearImpulse(ref _physicsVelocity, _physicsMass,
                            forward * _moveData.Force * _time);

                        var angle = math.degrees(math.atan2(direction.y, direction.x));
                        
                        var q = Quaternion.Euler(0, 0, angle);
                        
                        var y = Quaternion.Lerp(_rotation.Value, q, _moveData.SpeedRotated * _time);

                        _directionData.Direction = direction;
                        
                        _rotation.Value = y;
                }).Run();
        }
    }
}