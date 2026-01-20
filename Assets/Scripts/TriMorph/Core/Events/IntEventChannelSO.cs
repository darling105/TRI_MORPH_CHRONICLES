using UnityEngine;
using UnityEngine.Events;

namespace TriMorph.Core.Events
{
    [CreateAssetMenu(menuName = "TriMorph/Events/Int Event Channel")]
    public class IntEventChannelSO : ScriptableObject
    {
        public UnityAction<int> OnEventRaised;

        public void RaiseEvent(int value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
