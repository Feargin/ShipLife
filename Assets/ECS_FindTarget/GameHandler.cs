/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Transforms;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Rendering;
//
// public class GameHandler : MonoBehaviour {
//
//     [SerializeField] private Material unitMaterial;
//     [SerializeField] private Material targetMaterial;
//     [SerializeField] private Mesh quadMesh;
//
//     private static EntityManager entityManager;
//
//     private void Start() {
//
//         for (int i = 0; i < 100; i++) {
//             SpawnUnitEntity();
//         }
//
//         for (int i = 0; i < 500; i++) {
//             SpawnTargetEntity();
//         }
//     }
//
//     private float spawnTargetTimer;
//
//     private void Update() {
//         spawnTargetTimer -= Time.deltaTime;
//         if (spawnTargetTimer < 0) {
//             spawnTargetTimer = .1f;
//             
//             for (int i = 0; i < 200; i++) {
//                 SpawnTargetEntity();
//             }
//         }
//     }
//
//     private void SpawnUnitEntity() {
//         SpawnUnitEntity(new float3(UnityEngine.Random.Range(-8, +8f), UnityEngine.Random.Range(-5, +5f), 0));
//     }
//
//     private void SpawnUnitEntity(float3 position) {
//         Entity entity = entityManager.CreateEntity(
//             typeof(Translation),
//             typeof(LocalToWorld),
//             typeof(RenderMesh),
//             typeof(Scale),
//             typeof(TagShip)
//         );
//         SetEntityComponentData(entity, position, quadMesh, unitMaterial);
//         entityManager.SetComponentData(entity, new Scale { Value = 1.5f });
//     }
//
//     private void SpawnTargetEntity() {
//         Entity entity = entityManager.CreateEntity(
//             typeof(Translation),
//             typeof(LocalToWorld),
//             typeof(RenderMesh),
//             typeof(Scale),
//             typeof(TagPickup)
//         );
//         SetEntityComponentData(entity, new float3(UnityEngine.Random.Range(-8, +8f), UnityEngine.Random.Range(-5, +5f), 0), quadMesh, targetMaterial);
//         entityManager.SetComponentData(entity, new Scale { Value = .5f });
//     }
//
//     private void SetEntityComponentData(Entity entity, float3 spawnPosition, Mesh mesh, Material material) {
//         entityManager.SetSharedComponentData<RenderMesh>(entity,
//             new RenderMesh {
//                 material = material,
//                 mesh = mesh,
//             }
//         );
//
//         entityManager.SetComponentData<Translation>(entity, 
//             new Translation { 
//                 Value = spawnPosition
//             }
//         );
//     }
//
// }
//
//
//
// public class HasTargetDebug : ComponentSystem {
//
//     protected override void OnUpdate() {
//         Entities.ForEach((Entity entity, ref Translation translation, ref HasTarget hasTarget) => {
//             if (EntityManager.Exists(hasTarget.targetEntity)) {
//                 Translation targetTranslation = EntityManager.GetComponentData<Translation>(hasTarget.targetEntity);
//                 Debug.DrawLine(translation.Value, targetTranslation.Value);
//             }
//         });
//     }
//
// }








