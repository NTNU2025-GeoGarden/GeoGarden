using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.BaseModule.Map;
using Mapbox.BaseModule.Utilities;
using Mapbox.Example.Scripts.Map;
using UnityEngine;

namespace Mapbox.Examples
{
	public class CharacterMovement : MonoBehaviour
	{
		public delegate void HandleResourceCollected();

		public static HandleResourceCollected OnCollectResource;
		
		public GameObject resourcesUI;
		private MapResource _resourceRef = null;
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
			
			resourcesUI.SetActive(false);
			
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

		private void OnTriggerEnter(Collider other)
		{
			_resourceRef = other.GetComponent<MapResource>();
			if (_resourceRef.IsTaken)
			{
				_resourceRef = null;
				return;
			}

			_resourceColorRef = other.GetComponent<Renderer>().material.color;
			other.GetComponent<Renderer>().material.color = Color.green;
			resourcesUI.SetActive(true);
		}

		private void OnTriggerExit(Collider other)
		{
			if(_resourceRef != null && !_resourceRef.IsTaken)
				other.GetComponent<Renderer>().material.color = _resourceColorRef;
			
			resourcesUI.SetActive(false);
			_resourceRef = null;
		}

		private void CollectResource()
		{
			_resourceRef.OnCollectResource();
			resourcesUI.SetActive(false);
			_resourceRef = null;
		}

		void Update()
		{
			if (!_readyForUpdates)
				return;
			
			var direction = Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.up);
			var distance = direction.magnitude; //Vector3.Distance(transform.position, Target.position);
			if (distance > 1/_scale)
			{
				transform.LookAt(transform.position + direction);
				transform.Translate(Vector3.forward * (Speed/_scale));
				if(CharacterAnimator) CharacterAnimator.SetBool("IsWalking", true);
			}
			else
			{
				if(CharacterAnimator) CharacterAnimator.SetBool("IsWalking", false);
			}

			if (SnapToTerrain)
			{
				var latlng = _mapInformation.ConvertPositionToLatLng(this.transform.position);
				var tileId = Conversions.LatitudeLongitudeToTileId(latlng, 16).Canonical;
				
				//changed this part and haven't tested...
				var tileSpace = Conversions.LatitudeLongitudeToInTile01(latlng, tileId);
				var elevation = _mapInformation.QueryElevation(tileId, tileSpace.x, tileSpace.y);
				transform.position = new Vector3(transform.position.x, elevation, transform.position.z);
			}
		}
	}
}