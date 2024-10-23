using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public struct BrushConfigurationInfo {
		//-----------------------------------------------------------------------------------------
		// Public Fields:
		//-----------------------------------------------------------------------------------------

		public float Size;
		public float Rotation;
		public float Alpha;
		public Texture2D Texture;
		public PaintCanvas.PaintApplicationBlendModes BlendMode;

		//-----------------------------------------------------------------------------------------
		// Static Constructors:
		//-----------------------------------------------------------------------------------------

		public static BrushConfigurationInfo DEFAULT => new BrushConfigurationInfo(1, 0, 1, Texture2D.whiteTexture, PaintCanvas.PaintApplicationBlendModes.Additive);

		//-----------------------------------------------------------------------------------------
		// Constructors:
		//-----------------------------------------------------------------------------------------

		public BrushConfigurationInfo(float size, float rotation, float alpha, Texture2D texture, PaintCanvas.PaintApplicationBlendModes blendMode) {
			Size = size;
			Rotation = rotation;
			Alpha = alpha;
			Texture = texture;
			BlendMode = blendMode;
		}

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public BrushConfigurationInfo WithSizeMultiplier(float multiplier) {
			Size *= multiplier;
			return this;
		}
	}
}