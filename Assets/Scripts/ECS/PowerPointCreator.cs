using System;
using System.Collections;
using System.Collections.Generic;
using Fabric;
using NativeQuadTree;
using NaughtyAttributes;
using Unity.Entities;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Mathematics;
using Unity.Properties.UI;
using Unity.Transforms;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using Zenject;
using Random = Unity.Mathematics.Random;

namespace ShipSimulator
{
    public sealed class PowerPointCreatorSystem //: SystemBase
    {
        // public NativeQuadTree<QuadElement<T>> Tree;
        //
        // private float _timer;
        // private Random _random;
        // private float _sizeMapX;
        // private float _sizeMapY;
        // private BeginInitializationEntityCommandBufferSystem EntityCommandBuffer;
        //
        // protected override void OnCreate()
        // {
        //     Entities.WithoutBurst().ForEach((in MapData _mapData) =>
        //     {
        //         _sizeMapY = _mapData.SizeMapY;
        //         _sizeMapX = _mapData.SizeMapX;
        //     }).Run();
        //     
        //     Tree = new NativeQuadTree<QuadElement<T>>(new AABB2D(float2.zero, new float2( _sizeMapX, _sizeMapY)));
        //     _random = Random.CreateFromIndex(99);
        //     var commandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        // }
        //
        // protected override void OnUpdate()
        // {
        //     _timer -= Time.DeltaTime;
        //     var pos = new float3(_random.NextFloat(), _random.NextFloat(), 0);
        //     var commandBuffer = EntityCommandBuffer.CreateCommandBuffer();
        //     
        //     if (_timer <= 0)
        //         Entities.WithoutBurst().ForEach((ref SpawnPowerPointData _spawnPower) =>
        //         {
        //             var pointEntity = commandBuffer.Instantiate(_spawnPower.PowerPoint);
        //             EntityManager.SetComponentData(pointEntity, new Translation
        //             {
        //                 Value = pos
        //             });
        //             EntityManager.SetComponentData(pointEntity, new PowerPoint
        //             {
        //                 Position = new float2(pos.x, pos.y), Size = _random.NextInt(1, 12), Sustenance = _random.NextInt(1, 12)
        //             });
        //             EntityManager.SetComponentData(pointEntity, new PowerPoint
        //             {
        //                 Position = new float2(pos.x, pos.y), Size = _random.NextInt(1, 12), Sustenance = _random.NextInt(1, 12)
        //             });
        //
        //             _timer = _random.NextFloat(_spawnPower.Delay.x, _spawnPower.Delay.y);
        //         }).Run();
        // }
    }
}