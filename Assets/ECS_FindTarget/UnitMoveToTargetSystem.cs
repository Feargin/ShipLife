/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using ShipSimulator;
using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

// public class UnitMoveToTargetSystem : ComponentSystem 
// {

    // protected override void OnUpdate()
    // {
    //     Entities.ForEach((Entity unitEntity, ref HasTarget hasTarget, ref Translation translation) => 
    //     {
    //         if (EntityManager.Exists(hasTarget.TargetEntity))
    //         {
    //             Translation targetTranslation = EntityManager.GetComponentData<Translation>(hasTarget.TargetEntity);
    //
    //             float3 targetDir = math.normalize(targetTranslation.Value - translation.Value);
    //             float moveSpeed = 5f;
    //             translation.Value += targetDir * moveSpeed * Time.DeltaTime;
    //
    //             if (math.distance(translation.Value, targetTranslation.Value) < .2f)
    //             {
    //                 // Close to target, destroy it
    //                 PostUpdateCommands.DestroyEntity(hasTarget.TargetEntity);
    //                 PostUpdateCommands.RemoveComponent(unitEntity, typeof(HasTarget));
    //             }
    //         }
    //         else 
    //         {
    //             // Target Entity already destroyed
    //             PostUpdateCommands.RemoveComponent(unitEntity, typeof(HasTarget));
    //         }
    //     });
    // }

// }
