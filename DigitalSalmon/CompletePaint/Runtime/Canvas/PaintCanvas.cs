using System.Collections.Generic;
using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	/// <summary>
	/// A PaintCanvas is an IPaintable surface which stores RenderTextures to be painted on.
	/// </summary>
	public abstract class PaintCanvas : BaseBehaviour, IPaintable {
	//-----------------------------------------------------------------------------------------
		// Type Definitions:
		//-----------------------------------------------------------------------------------------

		public enum UvChannels {
			Uv0,
			Uv1
		}

		public enum PaintApplicationBlendModes {
			Additive,
			Subtractive
		}

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Canvas Settings")]
		[SerializeField]
		protected Vector2Int canvasSize;

		[SerializeField]
		protected Texture2D defaultFill;

		[SerializeField]
		protected float brushSizeMultiplier = 1;

		[Header("Advanced")]
		[SerializeField]
		protected UvChannels uvChannel;

		[SerializeField]
		protected int maxUndoSteps = 4;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private RenderTexture canvasRenderTexture;
		private RenderTexture canvasBuffer;

		private readonly List<PaintComputeOperation> paintOperations = new List<PaintComputeOperation>();

		private LimitedSizeRenderTextureStack undoStack;
		private LimitedSizeRenderTextureStack redoStack;

		//-----------------------------------------------------------------------------------------
		// Interface Properties:
		//-----------------------------------------------------------------------------------------

		UvChannels IPaintable.UvChannel => uvChannel;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected virtual void Awake() {
			undoStack = new LimitedSizeRenderTextureStack(maxUndoSteps);
			redoStack = new LimitedSizeRenderTextureStack(maxUndoSteps);
		}

		protected virtual void Start() {
			Clear();
			if (defaultFill != null) {
				FillCanvas(defaultFill);
			}
		}

		protected void Update() {
			if (paintOperations.Count > 0) DequeuePaintOperations();
		}

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IPaintable.ApplyPaint(PaintBrush.PaintBlob paintBlob, BrushConfigurationInfo brushConfiguration, Paint paint) {
			Vector4 stampRect = GetStampRect(paintBlob.Uv, brushConfiguration.Size * brushSizeMultiplier);

			Color paintColor = paint.GetPaintColor(paintBlob.Distance);
			paintColor.a *= brushConfiguration.Alpha;

			PaintComputeOperation paintOperation = new PaintComputeOperation(stampRect, brushConfiguration, paintColor);
			paintOperations.Add(paintOperation);
		}

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		/// <summary>
		/// Moves the 'Undo' history backwards one step.
		/// </summary>
		public void Undo() {
			if (undoStack.Count == 0) return;

			PushUndoStack(redoStack, GetCurrentUndoStepCopy());

			canvasRenderTexture = undoStack.Pop();
			Graphics.Blit(canvasRenderTexture, canvasBuffer);
			AssignCanvasTexture(canvasRenderTexture);
		}


		/// <summary>
		/// Moves the 'Undo' history forwards one step.
		/// </summary>
		public void Redo() {
			if (redoStack.Count == 0) return;

			PushUndoStack(undoStack, GetCurrentUndoStepCopy());

			canvasRenderTexture = redoStack.Pop();
			Graphics.Blit(canvasRenderTexture, canvasBuffer);
			AssignCanvasTexture(canvasRenderTexture);
		}

		/// <summary>
		/// Clears the 'Undo' history stack.
		/// </summary>
		public void ClearUndoHistory() {
			undoStack.ClearAndDestroy();
			redoStack.ClearAndDestroy();
		}

		/// <summary>
		/// Stores the current state of the canvas in the 'Undo' stack for later recollection.
		/// </summary>
		public void StoreUndoStep() {
			undoStack.Push(GetCurrentUndoStepCopy());
			redoStack.ClearAndDestroy();
		}

		/// <summary>
		/// Clears the canvas and resets its texture.
		/// </summary>
		public void Clear() {
			canvasRenderTexture = new RenderTexture(canvasSize.x, canvasSize.y, 24, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.sRGB) {enableRandomWrite = true};
			canvasRenderTexture.Create();

			canvasBuffer = new RenderTexture(canvasSize.x, canvasSize.y, 24, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.sRGB) {enableRandomWrite = true};
			canvasBuffer.Create();

			AssignCanvasTexture(canvasRenderTexture);
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected void DequeuePaintOperations() {
			PaintCompute.ApplyPaintBuffer(canvasRenderTexture, canvasBuffer, paintOperations);
			paintOperations.Clear();
			Graphics.Blit(canvasRenderTexture, canvasBuffer);
		}

		protected void FillCanvas(Texture2D texture) {
			Graphics.Blit(texture, canvasRenderTexture);
			Graphics.Blit(texture, canvasBuffer);
		}

		//-----------------------------------------------------------------------------------------
		// Abstract Methods:
		//-----------------------------------------------------------------------------------------

		protected abstract void AssignCanvasTexture(Texture canvasTexture);

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void PushUndoStack(LimitedSizeStack<RenderTexture> stack, RenderTexture value) { stack.Push(value); }

		private Vector4 GetStampRect(Vector2 normalisedPosition, float width) {
			float aspect = (float)canvasSize.y / canvasSize.x;
			float nX = width* aspect;
			float nY = width;
			Vector2 normalisedStampSize = new Vector2(nX, nY);
			Vector2 stampPosition = normalisedPosition - normalisedStampSize / 2;
			return new Vector4(stampPosition.x, stampPosition.y, normalisedStampSize.x, normalisedStampSize.y);
		}

		private RenderTexture GetCurrentUndoStepCopy() {
			RenderTexture currentStep = new RenderTexture(canvasRenderTexture) {enableRandomWrite = true};
			currentStep.Create();
			Graphics.CopyTexture(canvasRenderTexture, currentStep);
			return currentStep;
		}
	}
}