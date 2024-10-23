using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.XR;

namespace DigitalSalmon.CompletePaint {
	public class TriggerAnimator : BaseBehaviour {

		[SerializeField]
		protected float minY;

		[SerializeField]
		protected float maxY;

		protected void Update() {
			InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out float trigger);
			transform.localPosition = transform.localPosition.WithZ(Mathf.Lerp(minY, maxY, trigger));
		}

	}
}
