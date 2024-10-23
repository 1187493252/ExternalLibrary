using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PaintableUv = System.Collections.Generic.KeyValuePair<DigitalSalmon.CompletePaint.IPaintable, DigitalSalmon.CompletePaint.PaintBrush.PaintBlob>;

namespace DigitalSalmon.CompletePaint {
	/// <summary>
	/// A PaintBrush is a physic (Or projected) object which applies Paint to a canvas.
	/// </summary>
	public abstract partial class PaintBrush : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Type Definitions:
		//-----------------------------------------------------------------------------------------

		public enum BrushStyles {
			Sweep,
			Constant
		}

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected Paint paint;

		[SerializeField]
		[Tooltip(
			"The maximum uv distance a paint stroke will move in a single frame.\n Reduce this value to avoid drawing lines between uv islands.\n Increase this value to reduce 'skipping' on fast strokes.\n When painting in 2D on a rectangular canvas this value can be set to 1.")]
		protected float maxPerFrameThreshold = 0.1f;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private IBrushConfigurationProvider brushConfigurationProvider;

		private Dictionary<IPaintable, PaintBlob> previousBlobLookup = new Dictionary<IPaintable, PaintBlob>();
		private readonly Dictionary<IPaintable, PaintBlob> currentBlobLookup = new Dictionary<IPaintable, PaintBlob>();
		private readonly List<PaintBlob> interpolatedPaintBlobs = new List<PaintBlob>();

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public Paint Paint => paint;

		//-----------------------------------------------------------------------------------------
		// Protected Properties:
		//-----------------------------------------------------------------------------------------

		public abstract bool IsPainting { get; }
		public bool IsPaintingOnCanvas { get; private set; }
		public IEnumerable<IPaintable> Paintables => currentBlobLookup.Keys;

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private float SizeMultiplier { get; set; }

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected virtual void Awake() {
			if (brushConfigurationProvider == null) {
				brushConfigurationProvider = GetComponent<IBrushConfigurationProvider>();
			}
			AssignSizeMultiplier(1);
		}

		protected virtual void Update() {
			if (!IsPainting) {
				IsPaintingOnCanvas = false;
				previousBlobLookup.Clear();
				return;
			}

			PaintBlob[] currentBlobs = GetPaintBlobs();

			currentBlobLookup.Clear();
			foreach (PaintBlob blob in currentBlobs) {
				if (!currentBlobLookup.ContainsKey(blob.Paintable)) currentBlobLookup.Add(blob.Paintable, blob);
			}

			foreach (IPaintable paintable in currentBlobLookup.Keys) {
				if (!previousBlobLookup.ContainsKey(paintable)) paintable.StoreUndoStep();
			}

			GetInterpolatedBlobs(previousBlobLookup, currentBlobLookup, interpolatedPaintBlobs);

			if (interpolatedPaintBlobs.Any()) {
				IsPaintingOnCanvas = true;
			}

			foreach (PaintBlob paintBlob in interpolatedPaintBlobs) {
				ApplyPaint(paintBlob);
			}

			previousBlobLookup = new Dictionary<IPaintable, PaintBlob>(currentBlobLookup);
		}

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public void AssignPaint(Paint newPaint) { paint = newPaint; }

		public void AssignCofigurationProvider(IBrushConfigurationProvider provider) { brushConfigurationProvider = provider; }

		public void AssignSizeMultiplier(float sizeMultiplier) { SizeMultiplier = sizeMultiplier; }

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected void ApplyPaint(PaintBlob paintBlob) { paintBlob.Paintable.ApplyPaint(paintBlob, GetConfiguration(paintBlob).WithSizeMultiplier(SizeMultiplier), paint); }

		protected BrushConfigurationInfo GetConfiguration(PaintBlob paintBlob) {
			if (brushConfigurationProvider == null) {
				Debug.LogWarning($"Painting without a configuration. Have you added a BrushConfiguration component, or called {nameof(AssignCofigurationProvider)} from a script?");
				return BrushConfigurationInfo.DEFAULT;
			}
			return brushConfigurationProvider.GetConfiguration(paintBlob);
		}

		protected abstract PaintBlob[] GetPaintBlobs();

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void GetInterpolatedBlobs(Dictionary<IPaintable, PaintBlob> previous, Dictionary<IPaintable, PaintBlob> current, List<PaintBlob> output) {
			output.Clear();

			foreach (PaintableUv paintableUv in current) {
				// Store the current PaintBlob for this PaintableUv.
				PaintBlob currentBlob = paintableUv.Value;

				// If there are no previous blobs, or the previous blobs don't match the IPaintable key then we don't want to interpolate.
				if (previous == null || !previous.ContainsKey(currentBlob.Paintable)) {
					// In that case, just add the current blob and move along.
					output.Add(currentBlob);
					continue;
				}

				// Store the previous PaintBlob for the corresponding CurrentBlob.
				PaintBlob previousBlob = previous[paintableUv.Key];

				// Store the UV positions of Current and Previous.
				Vector2 previousUv = previousBlob.Uv;
				Vector2 currentUv = currentBlob.Uv;

				// Find the minimum distance (The size of the 'gap' between sample and current that should break the interpolating loop.
				float minThreshold = GetMinThreshold(previousBlob);

				// Find the total distance covered from Previous to Current.
				float totalDistance = Vector2.Distance(previousUv, currentUv);

				if (totalDistance > maxPerFrameThreshold) continue;

				// Copy the previous position to the sampling position.
				Vector2 sampleUv = previousUv;

				// Find how far we have left until we reach the end.
				float remainingDistance = totalDistance;

				int index = 0;

				// sample from prev to current at fixed iteration threshold.
				while (remainingDistance >= minThreshold) {
					index++;

					// If we're looping more than 1024 times, assume something went wrong. This might cause
					// skipping with large movements of small brushes.
					const int MAX_LOOP_COUNT = 1024;
					if (index >= MAX_LOOP_COUNT) break;

					// Recalculate how far we've moved since we started looping.
					float traversedDistance = Vector2.Distance(sampleUv, previousUv);

					// Recalculate how far we have left until we reach the end.
					remainingDistance = totalDistance - traversedDistance;

					// Find the 0->1 value for how far we've moved.
					float alpha = traversedDistance / totalDistance;

					// Add the normalized 0->1 representation of the minimum distance to alpha.
					alpha += minThreshold / totalDistance;

					// Clamp alpha at 1 so we don't overshoot on high 'minThreshold's.
					if (alpha >= 1) alpha = 1;

					// Find an interpolated PaintBlob at the sample alpha.
					PaintBlob sampleBlob = PaintBlob.Lerp(previousBlob, currentBlob, alpha);

					// Update the current sample UV position.
					sampleUv = sampleBlob.Uv;

					// Add the interpolated sample blob to the output.
					output.Add(sampleBlob);

					// Update the min threshold for the new sample blob (Incase the size changes).
					minThreshold = GetMinThreshold(sampleBlob);
				}

				// Add the current blob.
				output.Add(currentBlob);
			}
		}

		private float GetMinThreshold(PaintBlob blob) {
			const float UV_SAMPLE_MIN_THRESHOLD_MUL = 0.1f;
			const float UV_SAMPLE_MIN_THRESHOLD = 0.001f;
			BrushConfigurationInfo config = GetConfiguration(blob);
			float threshold = Mathf.Max(UV_SAMPLE_MIN_THRESHOLD, UV_SAMPLE_MIN_THRESHOLD_MUL * config.Size);
			return threshold;
		}
	}
}