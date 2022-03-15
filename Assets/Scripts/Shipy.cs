using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ShipSimulator
{
    public abstract class Shipу : MonoBehaviour
    {
        public float Speed { get; set; } = 8;
        public float Capacity { get; set; } = 5;
        public float Size { get; set; } = 1;
        public float Acceleration { get; set; } = 1;
        public float RangeVisibility { get; set; } = 30;
        public float DepthVisibility { get; set; } = 4;
        public float Age { get; set; }
        public float Energy { get; set; } = 10;
        public PowerPointData Target;
        [SerializeField]
        protected Transform Heat;
        protected Move Move;
        protected LifeCycle LifeCycle;
        protected FindTarget FindTarget;
        protected AI AI;
        // [Inject]
        // private PowerPointCreator _pointCreator;

        private void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            // Move = new Move(transform, Speed, Acceleration, _pointCreator, Heat);
            // LifeCycle = new LifeCycle();
            // FindTarget = new FindTarget(transform, RangeVisibility, DepthVisibility, _pointCreator);
            // AI = new AI();
        }

        protected void Update()
        {
            Move.CalculateTargetPosition(ref Target);
            // if(Target == null)
                // Target = FindTarget.Find();
            // else if (Target.IsInside)
                // Target = null;
        }
        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.yellow;
        //     Vector2 pos = Heat.position;
        //     if(FindTarget != null)
        //     {
        //         for (var index = 0; index < FindTarget.readyToCheck.Count; index++)
        //         {
        //             var p = FindTarget.readyToCheck[index];
        //             Gizmos.DrawLine(pos, p.transform.position);
        //         }
        //     }
        //
        //     Gizmos.color = Color.blue;
        //     if(Target != null)
        //         // Gizmos.DrawLine(pos, Target.GetPosition());
        //     Gizmos.color = new Color(0.79f, 1f, 0.56f, 0.15f);
        //     Gizmos.DrawSphere(transform.position, RangeVisibility);
        // }
    }

    public sealed class LifeCycle
    {
        
    }

    public sealed class AI
    {
        
    }
}
