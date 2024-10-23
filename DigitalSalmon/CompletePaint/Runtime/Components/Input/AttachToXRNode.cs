using UnityEngine;
using UnityEngine.XR;

namespace DigitalSalmon.CompletePaint {
	public class AttachToXRNode : BaseBehaviour {
		[SerializeField]
		protected XRNode node = XRNode.RightHand;

		protected void LateUpdate() {
			transform.localPosition = InputTracking.GetLocalPosition(XRNode.RightHand);
			transform.localRotation = InputTracking.GetLocalRotation(XRNode.RightHand);
		}
	}
}