using UnityEngine;
using Zenject;

namespace ShipSimulator
{
    public sealed class Move
    {
        // private Transform _transform;
        // private float _speed;
        // private float _acceleration;
        // private float _timeTirgetMemory;
        // private float _maxTimeMemory = 5;
        // private Vector2 _moveTarget;
        // // private PowerPointCreator _pointCreator;
        // private Vector2 _direction;
        // private float steerStrenght = 8;
        // private float _deflectionForce = 8;
        // private Vector2 _velosity;
        // private Vector2 _position;
        // private Transform _head;
        // public Move(Transform transform,  float speed, float acceleration, PowerPointCreator pointCreator, Transform head)
        // {
        //     _transform = transform;
        //     _speed = speed;
        //     _acceleration = acceleration;
        //     _pointCreator = pointCreator;
        //     _position = transform.position;
        //     _head = head;
        // }

        public void CalculateTargetPosition(ref PowerPointData target)
        {
            // if (target != null)
            // {
            //     Vector2 headPos = _head.position;
            //     Vector2 targetPos = target.GetPosition();
            //     _direction = (targetPos - headPos).normalized;
            //     
            //     const float pickupRadius = 1f;
            //     Vector2 dir = headPos - targetPos;
            //     if (CheckDistanсe(dir, pickupRadius))
            //     {
            //         target.transform.parent = _transform;
            //         _pointCreator.Tree.Remove(target);
            //         target.IsInside = true;
            //         target = null;
            //     }
            // }
            //
            // _direction = (Random.insideUnitCircle * _deflectionForce).normalized + _direction;
            // Vector2 desiredVelosity = _direction * _speed;
            // Vector2 steeringForce = (desiredVelosity - _velosity) * steerStrenght;
            // Vector2 acceleration = Vector2.ClampMagnitude(steeringForce, steerStrenght) / 1;
            //
            // _velosity = Vector2.ClampMagnitude(_velosity + acceleration * Time.smoothDeltaTime, _speed);
            // _position += _velosity * Time.smoothDeltaTime;
            //
            // var angle = Mathf.Atan2(_velosity.y, _velosity.x) * Mathf.Rad2Deg;
            // _transform.SetPositionAndRotation(_position, Quaternion.Euler(0, 0, angle));
        }

        private static bool CheckDistanсe(Vector2 dir, float range)
        {
            return dir.x * dir.x + dir.y * dir.y < range * range;
        }

    }
}