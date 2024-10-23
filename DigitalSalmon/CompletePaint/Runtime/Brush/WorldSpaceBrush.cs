using UnityEngine;
using UnityEngine.XR;

namespace DigitalSalmon.CompletePaint {
	public class WorldSpaceBrush : ProjectedBrush {
		//-----------------------------------------------------------------------------------------
		// Proptected Properties:
		//-----------------------------------------------------------------------------------------

		private float trigger;

		public override bool IsPainting => trigger > 0.6f;

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected override Ray GetProjectionRay() => new Ray(transform.position, transform.forward);
		protected override Vector2 GetScreenProjectionRayPosition() { return Vector2.zero; }
		protected override PaintBlob ModifyBlobs(PaintBlob blob) => blob;

		protected override void Update() {
			base.Update();
			InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out trigger);
		}
	}
}