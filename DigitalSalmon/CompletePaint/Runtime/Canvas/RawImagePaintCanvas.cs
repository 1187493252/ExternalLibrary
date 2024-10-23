using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.CompletePaint {
	public class RawImagePaintCanvas : PaintCanvas {
		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private RawImage rawImage;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected override void Awake() {
			base.Awake();
			rawImage = GetComponent<RawImage>();
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected override void AssignCanvasTexture(Texture canvasTexture) { rawImage.texture = canvasTexture; }
	}
}