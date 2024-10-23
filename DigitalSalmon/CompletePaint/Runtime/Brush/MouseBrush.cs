using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public class MouseBrush : ProjectedBrush {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Assignment")]
		[SerializeField]
		protected Camera projectionCamera;

		[SerializeField]
		protected float emulateDistance = 0.1f;

		[Header("Mouse Settings")]
		[SerializeField]
		protected int mouseButton;

		//-----------------------------------------------------------------------------------------
		// Protected Properties

		//-----------------------------------------------------------------------------------------

		public override bool IsPainting => Input.GetMouseButton(mouseButton);

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected override Ray GetProjectionRay() => projectionCamera.ScreenPointToRay(Input.mousePosition);
		protected override Vector2 GetScreenProjectionRayPosition() { return Input.mousePosition; }
		protected override PaintBlob ModifyBlobs(PaintBlob blob) => blob.WithDistance(emulateDistance);
	}
}