using DigitalSalmon.Extensions;
using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public class BrushConfiguration : BaseBehaviour, IBrushConfigurationProvider {
		//-----------------------------------------------------------------------------------------
		// Type Definitions:
		//-----------------------------------------------------------------------------------------

		public enum BrushTextureStyles {
			Single,
			Sequential,
			Random
		}

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Brush Settings")]
		[SerializeField]
		protected float brushSize;

		[SerializeField]
		protected PaintCanvas.PaintApplicationBlendModes blendMode;

		[Header("Texture")]
		[SerializeField]
		protected BrushTextureStyles brushTextureStyle;

		[SerializeField]
		protected Texture2D[] brushTextures;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private int brushTextureIndex;

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public Texture2D BrushTexture { get { return GetBrushTexture(); } }
		public virtual bool Subtractive { get { return false; } }

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		public virtual float GetBrushRotation() { return 0; }

		public virtual float GetBrushSize(float distance) { return brushSize; }

		public virtual float GetPaintAlpha(float distance) {
			return 1;
		}
		
		private Texture2D GetBrushTexture() {
			switch (brushTextureStyle) {
				case BrushTextureStyles.Single:
					return brushTextures[0];
				case BrushTextureStyles.Sequential:
					return brushTextures[IncrementBrushTextureIndex()];
				case BrushTextureStyles.Random:
					return brushTextures.RandomOrDefault();
			}
			return Texture2D.whiteTexture;
		}

		private int IncrementBrushTextureIndex() {
			int currentIndex = brushTextureIndex;
			brushTextureIndex++;
			if (brushTextureIndex >= brushTextures.Length - 1) brushTextureIndex = 0;
			return currentIndex;
		}

		public BrushConfigurationInfo GetConfiguration(PaintBrush.PaintBlob paintBlob) {
			return new BrushConfigurationInfo(GetBrushSize(paintBlob.Distance), GetBrushRotation(), GetPaintAlpha(paintBlob.Distance), GetBrushTexture(), blendMode);
		}
	}
}