using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public partial class PaintBrush {
		/// <summary>
		/// A Paint Blob is a struct containing a pointer to a particular UV position on a set IPaintable.
		/// It is used to know where to apply paint.
		/// </summary>
		public struct PaintBlob {
			//-----------------------------------------------------------------------------------------
			// Public Properties:
			//-----------------------------------------------------------------------------------------

			public IPaintable Paintable { get; }
			public Vector2 Uv => Paintable.UvChannel == PaintCanvas.UvChannels.Uv0 ? uv0 : uv1;
			public bool Valid => Paintable != null && Uv != Vector2.zero;
			public float Distance { get; private set; }

			//-----------------------------------------------------------------------------------------
			// Private Fields:
			//-----------------------------------------------------------------------------------------

			private Vector2 uv0;
			private Vector2 uv1;

			//-----------------------------------------------------------------------------------------
			// Static Properties:
			//-----------------------------------------------------------------------------------------

			public static PaintBlob NULL => new PaintBlob(null, Vector2.zero, Vector2.zero, 0);

			//-----------------------------------------------------------------------------------------
			// Constructors:
			//-----------------------------------------------------------------------------------------

			public PaintBlob(IPaintable paintable, Vector2 uv0, Vector2 uv1, float distance) {
				Paintable = paintable;
				this.uv0 = uv0;
				this.uv1 = uv1;
				Distance = distance;
			}

			public PaintBlob(IPaintable paintable, Vector2 uv, float distance) {
				Paintable = paintable;
				uv0 = uv;
				uv1 = uv;
				Distance = distance;
			}

			//-----------------------------------------------------------------------------------------
			// Public Methods:
			//-----------------------------------------------------------------------------------------
			
			public PaintBlob WithUv(Vector2 uv) {
				uv0 = uv;
				uv1 = uv;
				return this;
			}

			public static PaintBlob Lerp(PaintBlob a, PaintBlob b, float t) {
				PaintBlob o = a;
				o.uv0 = Vector2.Lerp(a.uv0, b.uv0, t);
				o.uv1 = Vector2.Lerp(a.uv1, b.uv1, t);
				o.Distance = Mathf.Lerp(a.Distance, b.Distance, t);
				return o;
			}

			public PaintBlob WithDistance(float distance) {
				Distance = distance;
				return this;
			}
		}
	}
}