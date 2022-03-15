using UnityEngine;

namespace DarkPower.Infrastructure
{
    public abstract class MonoStateBase : MonoBehaviour
    {
        public virtual void Enter() => gameObject.SetActive(true);
        public virtual void Exit() => gameObject.SetActive(false);
    }
}