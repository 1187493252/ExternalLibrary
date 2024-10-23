using UnityEngine;

namespace DigitalSalmon.CompletePaint {
	public class MeshPaintCanvas : PaintCanvas {
		//-----------------------------------------------------------------------------------------
		// Type Definitions:
		//-----------------------------------------------------------------------------------------

		private static class Errors {
			public static readonly string MISSING_MESH_COLLIDER =
				$"Cannot find {nameof(MeshCollider)} on {nameof(MeshPaintCanvas)}. {nameof(MeshPaintCanvas)} will never recieve paint. Add a {nameof(MeshCollider)} component.";

			public static readonly string MISSING_RENDERER = $"Missing Renderer on {nameof(MeshPaintCanvas)}. If you don't want to use a renderer, you must assign a target material.";

			public static readonly string MISSING_MATERIAL = $"Missing Material on {nameof(MeshPaintCanvas)}. Does your renderer have a material assigned?";

			public static readonly string MISSING_TEXTURE_PROPERTY = $"Could not find target texture property on target material in {nameof(MeshPaintCanvas)}. Using 'mainTexture'.";
		}

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Mesh Canvas Settings")]
		[SerializeField]
		[Tooltip("The material you want to apply your paint texture to.\n\nDefault: The first material on your Renderer component.")]
		protected Material targetMaterial;

		[SerializeField]
		[Tooltip("The texture property you want to use to assign the canvas texture to your material.\n\nDefault: 'mainTexture' (_MainTex).")]
		protected string targetTextureProperty;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected override void Awake() {
			base.Awake();
			MeshCollider meshCollider = GetComponentInChildren<MeshCollider>();
			if (meshCollider == null) Debug.LogWarning(Errors.MISSING_MESH_COLLIDER);
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods
		//-----------------------------------------------------------------------------------------

		protected override void AssignCanvasTexture(Texture canvasTexture) {
			if (targetMaterial == null) {
				if (renderer == null) {
					Debug.LogWarning(Errors.MISSING_RENDERER);
					return;
				}
				targetMaterial = renderer.material;
			}
			if (targetMaterial == null) {
				Debug.LogWarning(Errors.MISSING_MATERIAL);
				return;
			}

			if (string.IsNullOrEmpty(targetTextureProperty)) {
				targetMaterial.mainTexture = canvasTexture;
			}
			else {
				if (!targetMaterial.HasProperty(targetTextureProperty)) {
					Debug.LogWarning(Errors.MISSING_TEXTURE_PROPERTY);
					targetMaterial.mainTexture = canvasTexture;
					return;
				}
				targetMaterial.SetTexture(targetTextureProperty, canvasTexture);
				CheckKeywords(targetMaterial, targetTextureProperty);
			}
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		/// <summary>
		/// Checks whether a texture property will require a keyword update. Only applies to Standard Shader.
		/// https://docs.unity3d.com/Manual/MaterialsAccessingViaScript.html
		/// </summary>
		private void CheckKeywords(Material targetMaterial, string property) {
			switch (property) {
				case "_BumpMap":
					targetMaterial.EnableKeyword("_NORMALMAP");
					break;
				case "_MetallicGlossMap":
					targetMaterial.EnableKeyword("_METALLICGLOSSMAP");
					break;
				case "_SpecGlossMap":
					targetMaterial.EnableKeyword("_SPECGLOSSMAP");
					break;
				case "_ParallaxMap":
					targetMaterial.EnableKeyword("_PARALLAXMAP");
					break;
				case "_EmissionMap":
					targetMaterial.EnableKeyword("_EMISSION");
					break;
			}
		}
	}
}