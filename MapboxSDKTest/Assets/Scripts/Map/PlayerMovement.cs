using Mapbox.BaseModule.Map;
using Mapbox.BaseModule.Utilities;
using UnityEngine;

namespace Map
{
	public class PlayerMovement : MonoBehaviour
	{
		public delegate void HandleResourceCollected();
		
		public static HandleResourceCollected OnCollectResource;
		
		//public GameObject resourcesUI;
		private SpawnerOnMap _ref = null;
		private Color _resourceColorRef;
		
		public MapBehaviourCore MapBehaviour;
		private IMapInformation _mapInformation;
		
		public Transform Target;
		public Animator CharacterAnimator;
		public float Speed;
		private float _scale;
		private bool _readyForUpdates = false;
		
		public bool SnapToTerrain = false;



	
		private void Start()
		{
			OnCollectResource += CollectResource;
			
			//resourcesUI.SetActive(false);
			
			MapBehaviour.Initialized += map =>
			{
				_mapInformation = map.mapInformation;
				_scale = map.mapInformation.Scale;
				_readyForUpdates = true;
				
				// Snap character to final position when the map scale changes
				_mapInformation.WorldScaleChanged += _ =>
				{
					transform.position = Target.position;
				};
			};
		}

		
		private void DetectObjectClick()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                if (!hit.collider.TryGetComponent<SpawnerOnMap>(out var clickedResource))
                    return; // ✅ Ignore clicks on non-resource objects

                if (_ref == null)
                {
                    Debug.Log("❌ ERROR: Player is not close enough to collect this resource!");
                    return;
                }

                if (clickedResource != _ref)
                {
                    Debug.Log($"❌ ERROR: Clicked object ({clickedResource.gameObject.name}) is not the currently collided resource!");
                    return;
                }

                if (!_ref.collected) // ✅ Only collect if it's not already collected
                {
                    Debug.Log($"🖱️/📱 Player clicked on {clickedResource.gameObject.name} while inside collision area.");
                    _ref.CollectThisResource();
                }
            }
        }



		private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"🚀 Player entered trigger of: {other.gameObject.name}");

			
			if (!other.TryGetComponent<SpawnerOnMap>(out var resource))
			{
				Debug.LogError($"❌ ERROR: SpawnerOnMap component is missing on {other.gameObject.name}!");
				return;
			}

			if (resource.collected)
			{
				Debug.Log("⚠️ Resource is already collected.");
				return;
			}

			_ref = resource; // ✅ Assigning _ref

			Debug.Log($"✅ Successfully assigned _ref: {_ref.gameObject.name}");

			_resourceColorRef = other.GetComponent<Renderer>().material.color;
			other.GetComponent<Renderer>().material.color = Color.green;
			//resourcesUI.SetActive(true);
		}

		private void OnTriggerExit(Collider other)
		{
			if (_ref != null && !_ref.collected)
			{
				other.GetComponent<Renderer>().material.color = _resourceColorRef;
			}

			// ✅ Only clear _ref if it hasn’t been collected yet
			if (_ref != null && !_ref.collected)
			{
				Debug.Log("⚠️ Player exited trigger, clearing _ref!");
				_ref = null;
			}

			//resourcesUI.SetActive(false);
		}



		private void CollectResource()
		{
			  Debug.Log($"📢 CollectResource() was called from: {new System.Diagnostics.StackTrace()}");
			Debug.Log($"📌 BEFORE collecting: _ref = {_ref}");

			if (_ref == null)
			{
				Debug.LogError("❌ ERROR: _ref is NULL in CollectResource()! It may have been reset.");
				return;
			}

			Debug.Log($"✅ Collecting resource: {_ref.gameObject.name}");

			_ref.CollectThisResource();
			//resourcesUI.SetActive(false);
			_ref = null;

			Debug.Log($"📌 AFTER collecting: _ref = {_ref}"); // Should be NULL after collection
		}

		void Update()
		{
			if (!_readyForUpdates)
				return;

			// ✅ Detect Clicks (PC) and Touches (Mobile)
			if (Input.GetMouseButtonDown(0)) // Left Mouse Click (PC)
			{
				DetectObjectClick();
			}
			else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // First Finger Touch (Mobile)
			{
				DetectObjectClick();
			}

			var direction = Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.up);
			var distance = direction.magnitude; // Vector3.Distance(transform.position, Target.position);
			
			if (distance > 1 / _scale)
			{
				transform.LookAt(transform.position + direction);
				transform.Translate(Vector3.forward * (Speed / _scale));

				if (CharacterAnimator) 
					CharacterAnimator.SetBool("IsWalking", true);
			}
			else
			{
				if (CharacterAnimator) 
					CharacterAnimator.SetBool("IsWalking", false);
			}

			if (SnapToTerrain)
			{
				var latlng = _mapInformation.ConvertPositionToLatLng(this.transform.position);
				var tileId = Conversions.LatitudeLongitudeToTileId(latlng, 16).Canonical;

				// Changed this part and haven't tested...
				var tileSpace = Conversions.LatitudeLongitudeToInTile01(latlng, tileId);
				var elevation = _mapInformation.QueryElevation(tileId, tileSpace.x, tileSpace.y);
				transform.position = new Vector3(transform.position.x, elevation, transform.position.z);
			}
		}
	}
}