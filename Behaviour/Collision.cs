using System;
using UnityEngine;

namespace MonkeModViewer.Behaviour
{
    public class Collision : MonoBehaviour
    {
        public float debounceTime = 0.25f;

        public float touchTime;

        public Action onClick;
        public void OnTriggerEnter(Collider collider)
        {
            if (!base.enabled || !(touchTime + debounceTime < Time.time) || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
            {
                return;
            }
            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
            touchTime = Time.time;
            onClick.Invoke();
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(65, component.isLeftHand, 0.05f);
            GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
        }
    }
}
