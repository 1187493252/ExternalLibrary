using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public class SprayCanConfiguration : BrushConfiguration {
		//-----------------------------------------------------------------------------------------
		// Constants:
		//-----------------------------------------------------------------------------------------

		private const float ALPHA_MULTIPLIER = 0.25f;

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Spray Can Settings")]
		[SerializeField]
		protected float maxDistance;

		[Header("Size")]

		[SerializeField]
		protected AnimationCurve brushSizeFalloff;

		[SerializeField]
		protected float maxBrushSizeMultiplier;

		[Header("Alpha")]
		[SerializeField]
		protected AnimationCurve brushAlphaFalloff;

		[SerializeField]
		protected float maxAlphaMultiplier;

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public override float GetBrushRotation() {
			return Random.Range(0, 360);
		}

		public override float GetBrushSize(float distance) {
			return brushSizeFalloff.Evaluate(GetDistanceDelta(distance)) * maxBrushSizeMultiplier * brushSize;
		}

		public override float GetPaintAlpha(float distance) {
			return brushAlphaFalloff.Evaluate(GetDistanceDelta(distance)) * maxAlphaMultiplier * ALPHA_MULTIPLIER;
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private float GetDistanceDelta(float distance) { return MathUtilities.LinearStep(0, maxDistance, distance); }
	}
}