using System.Collections.Generic;
using UnityEngine;

namespace ShipSimulator
{
    public sealed class FindTarget
    {
        private Transform _transform;
        public Rect _rect;
        private float _powR;
        // private PowerPointCreator _pointCreator;
        private float _deph;
        public List<PowerPointData> readyToCheck = new List<PowerPointData>(); //TODO debug

        // public FindTarget(Transform transform, float powR, float deph, PowerPointCreator pointCreator)
        // {
        //     // _pointCreator = pointCreator;
        //     // _transform = transform;
        //     // _deph = deph;
        //     // _powR = powR * powR;
        //     // _rect = new Rect(Vector2.zero, new Vector2(powR * 2f, powR * 2f));
        // }

        // public PowerPoint Find()
        // {
        //     var pos = new Vector2(_transform.position.x + 0.5f, _transform.position.y);
        //     _rect.center = pos;
			     //
        //     readyToCheck = _pointCreator.Tree.RetrieveObjectsInArea(_rect);
        //     if (readyToCheck == null || readyToCheck.Count == 0)
        //     {
        //         return null;
        //     }
        //
        //     var index = 0;
        //     var nearest = 0f;
        //     var resultIndex = 0;
        //     while (index < readyToCheck.Count)
        //     {
        //         var pointPos = readyToCheck[index].GetPosition();
        //         var dir = (pos - pointPos).normalized;
        //         var angl = Vector2.Angle(_transform.forward, dir);
        //         if(angl > _deph / 2)
        //             continue;
        //         var distance = dir.x * dir.x + dir.y * dir.y;
        //         if (distance <= _powR && nearest < distance)
        //         {
        //             nearest = distance;
        //             resultIndex = index;
        //         }
        //         ++index;
        //     }
        //
        //     return readyToCheck[resultIndex];
        // }
    }
}