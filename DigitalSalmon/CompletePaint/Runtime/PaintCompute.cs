using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalSalmon.Extensions;

namespace DigitalSalmon.CompletePaint {
	public static class PaintCompute {
		//-----------------------------------------------------------------------------------------
		// Constants:
		//-----------------------------------------------------------------------------------------

		private const string MAIN_KERNEL = "CSMain";
		private const string RESULT_TEXTURE = "Result";
		private const string BUFFER_TEXTURE = "CanvasBuffer";

		private const int BUFFER_SIZE = 2048;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private static readonly ComputeShader computeShader;
		private static readonly int mainKernel;

		//-----------------------------------------------------------------------------------------
		// Constructors:
		//-----------------------------------------------------------------------------------------

		static PaintCompute() {
			computeShader = Resource.Load<ComputeShader>("PaintCompute");
			mainKernel = computeShader.FindKernel(MAIN_KERNEL);
		}

		//-----------------------------------------------------------------------------------------
		// Static Methods:
		//-----------------------------------------------------------------------------------------

		public static void ApplyPaintBuffer(RenderTexture renderTexture, RenderTexture canvasBuffer, List<PaintComputeOperation>paintOperations) {
		
			const string RECT_BUFFER = "StampRect";
			ComputeBuffer rectBuffer = new ComputeBuffer(BUFFER_SIZE, sizeof(float) * 4, ComputeBufferType.Default);
			computeShader.SetBuffer(mainKernel, RECT_BUFFER, rectBuffer);

			const string ROTATION_BUFFER = "StampRotation";
			ComputeBuffer rotationBuffer = new ComputeBuffer(BUFFER_SIZE, sizeof(float), ComputeBufferType.Default);
			computeShader.SetBuffer(mainKernel, ROTATION_BUFFER, rotationBuffer);
			
			// constants
			const string CANVAS_SIZE = "CanvasSize";

			const string STAMP_TINT = "StampTint";
			const string STAMP_TEXTURE = "StampTexture";
			const string STAMP_TEXTURE_SIZE = "StampTextureSize";
			const string SUBTRACTIVE = "Subtractive";

			const string BUFFER_SIZE_PROP = "StampBufferSize";

			
			// property assignment
			computeShader.SetTexture(mainKernel, RESULT_TEXTURE, renderTexture);
			computeShader.SetTexture(mainKernel, BUFFER_TEXTURE, canvasBuffer);

			computeShader.SetVector(CANVAS_SIZE, renderTexture.Size().ToVector2());
			computeShader.SetInt(BUFFER_SIZE_PROP, paintOperations.Count);
			
			computeShader.SetVector(STAMP_TINT, paintOperations[0].StampTint);
			computeShader.SetTexture(mainKernel, STAMP_TEXTURE, paintOperations[0].StampTexture);
			computeShader.SetVector(STAMP_TEXTURE_SIZE, paintOperations[0].StampTextureSize);
			computeShader.SetBool(SUBTRACTIVE, paintOperations[0].BlendMode == PaintCanvas.PaintApplicationBlendModes.Subtractive);

			rectBuffer.SetData(paintOperations.Select(p => p.StampRect).ToArray());
			rotationBuffer.SetData(paintOperations.Select(p => p.StampRotation).ToArray());
			
			computeShader.Dispatch(mainKernel, renderTexture.width / 16, renderTexture.height / 16, 1);

			rectBuffer.Release();
			rotationBuffer.Release();
		}
	}
}