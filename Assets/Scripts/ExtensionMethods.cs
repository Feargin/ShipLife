﻿using System.Numerics;
using Unity.Mathematics;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityObject = UnityEngine.Object;

namespace ShipSimulator
{
    public static class ExtensionMethods
    {
        public static float2 Normalize(this float2 fl2, out float2 r)
        {
            float lengthSq = math.lengthsq(fl2);
            float invLength = math.rsqrt(lengthSq);
            r = fl2 * invLength;
            return lengthSq * invLength;
        }

        public static float Angle(this float2 from, float2 to)
        {
            var num = math.sqrt((from.x * from.x + from.y * from.y) * (to.x * to.x + to.y * to.y));
            return num < 1.00000000362749E-15 ? 0.0f : math.acos(math.clamp(from.Dot(to) / num, -1f, 1f)) * 57.29578f;
        }

        public static float Dot(this float2 value1, float2 value2)
        {
            return value1.x * value2.x + value1.y * value2.y;
        }

        public static float2 mul(quaternion q, float2 v)
        {
            float2 t = 2 * cross(q.value.xy, v);
            return v + q.value.w * t + cross(q.value.xy, t);
        }

        public static float2 cross(float2 x, float2 y)
        {
            return (x * y.yx - x.yx * y).yx;
        }


        public static void Reverse<T>(this NativeList<T> list)
            where T : struct
        {
            var length = list.Length;
            var index1 = 0;

            for (var index2 = length - 1; index1 < index2; --index2)
            {
                var obj = list[index1];
                list[index1] = list[index2];
                list[index2] = obj;
                ++index1;
            }
        }

        /// <summary>
        /// Insert an element into a list.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The element.</param>
        /// <param name="index">The index.</param>
        public static unsafe void Insert<T>(this NativeList<T> list, T item, int index)
            where T : struct
        {
            if (list.Length == list.Capacity - 1)
            {
                list.Capacity *= 2;
            }

            // Inserting at end same as an add
            if (index == list.Length)
            {
                list.Add(item);
                return;
            }

            if (index < 0 || index > list.Length)
            {
                throw new IndexOutOfRangeException();
            }

            // add a default value to end to list to increase length by 1
            list.Add(default);

            int elemSize = UnsafeUtility.SizeOf<T>();
            byte* basePtr = (byte*) list.GetUnsafePtr();

            var from = (index * elemSize) + basePtr;
            var to = (elemSize * (index + 1)) + basePtr;
            var size = elemSize * (list.Length - index - 1); // -1 because we added an extra fake element

            UnsafeUtility.MemMove(to, from, size);

            list[index] = item;
        }

        /// <summary>
        /// Remove an element from a <see cref="NativeList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of NativeList.</typeparam>
        /// <typeparam name="TI">The type of element.</typeparam>
        /// <param name="list">The NativeList.</param>
        /// <param name="element">The element.</param>
        /// <returns>True if removed, else false.</returns>
        public static bool Remove<T, TI>(this NativeList<T> list, TI element)
            where T : struct, IEquatable<TI>
            where TI : struct
        {
            var index = list.IndexOf(element);
            if (index < 0)
            {
                return false;
            }

            list.RemoveAt(index);
            return true;
        }
        
        

        /// <summary>
        /// Remove an element from a <see cref="NativeList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list to remove from.</param>
        /// <param name="index">The index to remove.</param>
        public static void RemoveAt<T>(this NativeList<T> list, int index)
            where T : struct
        {
            list.RemoveRange(index, 1);
        }

        /// <summary>
        /// Removes a range of elements from a <see cref="NativeList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list to remove from.</param>
        /// <param name="index">The index to remove.</param>
        /// <param name="count">Number of elements to remove.</param>
        public static unsafe void RemoveRange<T>(this NativeList<T> list, int index, int count)
            where T : struct
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if ((uint) index >= (uint) list.Length)
            {
                throw new IndexOutOfRangeException(
                    $"Index {index} is out of range in NativeList of '{list.Length}' Length.");
            }
#endif

            int elemSize = UnsafeUtility.SizeOf<T>();
            byte* basePtr = (byte*) list.GetUnsafePtr();

            UnsafeUtility.MemMove(basePtr + (index * elemSize), basePtr + ((index + count) * elemSize),
                elemSize * (list.Length - count - index));

            // No easy way to change length so we just loop this unfortunately.
            for (var i = 0; i < count; i++)
            {
                list.RemoveAtSwapBack(list.Length - 1);
            }
        }

        /// <summary>
        /// Resizes a <see cref="NativeList{T}"/> and then clears the memory.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="buffer">The <see cref="NativeList{T}"/> to resize.</param>
        /// <param name="length">Size to resize to.</param>
        public static unsafe void ResizeInitialized<T>(this NativeList<T> buffer, int length)
            where T : struct
        {
            buffer.ResizeUninitialized(length);
            UnsafeUtility.MemClear(buffer.GetUnsafePtr(), length * UnsafeUtility.SizeOf<T>());
        }

        public static void ResizeInitializeNegativeOne(this NativeList<int> buffer, int length)
        {
            buffer.ResizeUninitialized(length);

#if UNITY_2019_3_OR_NEWER
            unsafe
            {
                UnsafeUtility.MemSet(buffer.GetUnsafePtr(), byte.MaxValue, length * UnsafeUtility.SizeOf<int>());
            }
#else
            for (var i = 0; i < length; i++)
            {
                buffer[i] = -1;
            }
#endif
        }
        
        // public static Entity GetPrimaryEntityUnsafe(UnityObject uobject)
        // {
        //     var entity = TryGetPrimaryEntityUnsafe(uobject);
        //     return entity;
        // }
        //
        // public static Entity TryGetPrimaryEntityUnsafe(UnityObject uobject)
        // {
        //     if (uobject == null)
        //         return Entity.Null;
        //     
        //     if (!m_JournalData.TryGetPrimaryEntity(uobject.GetInstanceID(), out var entity))
        //         uobject.CheckObjectIsNotComponent();
        //
        //     return entity;
        // }
        //
        // [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        // public static void CheckObjectIsNotComponent(this UnityObject @this)
        // {
        //     if (@this is Component)
        //     {
        //         // should not be possible to get here (user-callable API's will always auto request the Component's GameObject, unless we have a bug)
        //         throw new InvalidOperationException();
        //     }
        // }
    }
}


    
 
   
