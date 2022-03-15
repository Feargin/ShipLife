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
        public float2 Direction;
    }

    [UpdateAfter(typeof(Unity.Physics.Systems.EndFramePhysicsSystem))]
    public sealed class ShipMoveSystem : SystemBase
    {
        private quaternion _finalRotation;
        protected override void OnUpdate()
        {
            float _time = UnityEngine.Time.smoothDeltaTime;

            Entities.WithoutBurst().ForEach(
                (
                    Entity _entity,
                    ref LocalToWorld _localToWorld,
                    ref Rotation _rotation,
                    ref PhysicsVelocity _physicsVelocity,
                    ref PhysicsMass _physicsMass,
                    ref DirectionData _directionData,
                    in TargetData _targetData,
                    in MoveData _moveData) =>
                {
                    _physicsMass.InverseMass = _moveData.Mass;
  
                    var direction = _directionData.Direction;
                        var pos = new float2(_localToWorld.Position.x, _localToWorld.Position.y);
                        float2 rand = Random.insideUnitCircle;
                        if(_targetData.Target.Position.Equals(float2.zero))
                            direction = math.normalize(rand * _moveData.DeflectionForce) + direction;
                        else
                            direction = math.normalize(_targetData.Target.Position - pos);
                        
                        var forward = _localToWorld.Right;
                        PhysicsComponentExtensions.ApplyLinearImpulse(ref _physicsVelocity, _physicsMass,
                            forward * _moveData.Force * _time);

                        var angle = math.degrees(math.atan2(direction.y, direction.x));
                        //_finalRotation.Value = quaternion.Euler(0, 0, angle);
                        
                        //var y = quaternion.LookRotation(forward, angle);
                        //_rotation.Value = y;//math.mul(_rotation.Value, quaternion.RotateZ(math.radians(angle * 0.01f)));
                        //var rotVector = _finalRotation * new float3(0, 0, 1);
                        //if(rotVector.z >= angle)
                        
                        var q = Quaternion.Euler(0, 0, angle);
                        
                        var y = Quaternion.Lerp(_rotation.Value, q, _moveData.SpeedRotated * _time);

                        _directionData.Direction = direction;
                        //if(y.eulerAngles.z > angle + 3 && y.eulerAngles.y < angle - 3)
                        // float3 f = float3.zero;
                        // if (angle > 5)
                        //     f = new float3(0, 120 * _time, 0);
                        // else if (angle < -5)
                        //     f = new float3(0, -120 * _time, 0);
                        // else
                        //     f = float3.zero;
                        //
                        // Quaternion qu = _rotation.Value;
                         //if(qu.eulerAngles.z > angle + 5 || qu.eulerAngles.z < angle - 5)
                             // PhysicsComponentExtensions.ApplyAngularImpulse(ref _physicsVelocity, _physicsMass, f);
                        
                        // Debug.Log($"{angle} + {rand}");
                        _rotation.Value = y;
                }).Run();
        }
    }
}