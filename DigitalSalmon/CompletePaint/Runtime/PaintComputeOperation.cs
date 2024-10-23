using DigitalSalmon.Extensions;
using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public struct PaintComputeOperation {
		public Vector4 StampRect { get; }
		public Vector4 StampTint { get; }
		public float StampRotation { get; }
		public PaintCanvas.PaintApplicationBlendModes BlendMode { get; }

		public Texture2D StampTexture { get; }
		public Vector2 StampTextureSize => StampTexture.Size().ToVector2();

		public PaintComputeOperation(Vector4 stampRect, BrushConfigurationInfo brushConfiguration, Vector4 stampTint) {
			StampRect = stampRect;
			StampTint = stampTint;
			StampTexture = brushConfiguration.Texture;
			StampRotation = brushConfiguration.Rotation;
			BlendMode = brushConfiguration.BlendMode;
		}

		public static PaintComputeOperation Default => new PaintComputeOperation(Vector4.zero, BrushConfigurationInfo.DEFAULT, Vector4.zero);

		public override string ToString() =>
			$"Paint Operation. StampRect: {StampRect}, StampTint: {StampTint}, StampRotation:{StampRotation}, Blend: {BlendMode}, TextureSize: {StampTextureSize}";
	}
}