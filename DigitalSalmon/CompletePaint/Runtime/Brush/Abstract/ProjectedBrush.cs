using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using DigitalSalmon.Extensions;

namespace DigitalSalmon.CompletePaint {
	public abstract class ProjectedBrush : PaintBrush {
		//-----------------------------------------------------------------------------------------
		// Constants:
		//-----------------------------------------------------------------------------------------

		protected const int MAX_RAYCAST_HITS = 8;

		//-----------------------------------------------------------------------------------------
		// Type Definitions:
		//-----------------------------------------------------------------------------------------

		[Flags]
		public enum ProjectionSurfaces {
			Nothing = 0,
			MeshCollider = 1,
			Ugui = 2,
			Everything = 3
		}

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Projection Settings")]
		[SerializeField]
		protected ProjectionSurfaces projectionSurface = ProjectionSurfaces.Everything;

		[SerializeField]
		protected bool passThrough;

		[SerializeField]
		protected float maxDistance = 1;

		//-----------------------------------------------------------------------------------------
		// Protected Fields:
		//-----------------------------------------------------------------------------------------

		protected RaycastHit[] projectionHits = new RaycastHit[MAX_RAYCAST_HITS];
		protected List<RaycastResult> raycastResults = new List<RaycastResult>();

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected override PaintBlob[] GetPaintBlobs() {

			// Store the projection ray which we will fire our paint from.
			Ray projectionRay = GetProjectionRay();
			projectionRay.direction = projectionRay.direction.normalized * maxDistance;

			// Perform a Physics Raycast along the ray.
			projectionHits = new RaycastHit[MAX_RAYCAST_HITS];
			if (projectionSurface.HasFlag(ProjectionSurfaces.MeshCollider)) Physics.RaycastNonAlloc(projectionRay, projectionHits, maxDistance);

			// Perform a UI Raycast using the events system.
			if (projectionSurface.HasFlag(ProjectionSurfaces.Ugui)) RaycastUi(GetScreenProjectionRayPosition(), raycastResults);

			// Append both raycast results together.
			IEnumerable<PaintBlob> blobs = GetPaintBlobs(projectionHits).Append(GetPaintBlobs(raycastResults));

			// Set all the paint blob distances to the emulated distance.
			PaintBlob[] blobArray = blobs.Select(ModifyBlobs).ToArray();

			// Return all valid paint blob results.
			return blobArray;
		}

		protected abstract Ray GetProjectionRay();
		protected abstract Vector2 GetScreenProjectionRayPosition();
		protected abstract PaintBlob ModifyBlobs(PaintBlob blob);
	}
}