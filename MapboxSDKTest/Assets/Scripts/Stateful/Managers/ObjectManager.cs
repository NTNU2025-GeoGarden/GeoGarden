using System;
using System.Collections.Generic;
using System.Linq;
using Garden;
using Structs;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stateful.Managers
{
    public class ObjectManager : MonoBehaviour, IUsingGameState
    {
        public GardenCamera gardenCamera;
        public EditableObject objPrefab;
        
        private List<SerializableObject> _serializedObjects;
        private List<EditableObject> _dynamicObjects;
        
        private void GenerateInGameObjects()
        {
            Debug.Log("<color=lime>[ObjectManager] Generating editable objects</color>");
            
            if (_dynamicObjects == null)
                _dynamicObjects = new List<EditableObject>();
            else
            {
                foreach (EditableObject obj in _dynamicObjects)
                {
                    Destroy(obj);
                }
                
                _dynamicObjects.Clear();
            }
            
            int count = 0;
            foreach (SerializableObject obj in _serializedObjects)
            {
                EditableObject instantiatedObj = Instantiate(objPrefab.gameObject, transform).GetComponent<EditableObject>();
                instantiatedObj.type = obj.Type;
                instantiatedObj.transform.localPosition = new Vector3(obj.X, obj.Y, obj.Z);
                instantiatedObj.transform.rotation = new Quaternion(obj.RotX, obj.RotY, obj.RotZ, obj.RotW);
                instantiatedObj.editControls.transform.rotation = Quaternion.identity;
                instantiatedObj.ObjectID = count;
                instantiatedObj.gardenCamera = gardenCamera;
                
                _dynamicObjects.Add(instantiatedObj);

                count++;
            }
        }
        
        public void LoadData(GameState state)
        {
            Debug.Log("<color=cyan>[ObjectManager] Loading data</color>");
            _serializedObjects = state.Objects;
            
            GenerateInGameObjects();
        }

        public void SaveData(ref GameState state)
        {
            Debug.Log("<color=cyan>[ObjectManager] Saving data</color>");
            state.Objects = _serializedObjects;
        }

        public void AddObject(EditableObject editableObject)
        {
            Debug.Log($"<color=lime>[ObjectManager] Adding object (X {editableObject.transform.position.x} Y {editableObject.transform.position.y} Z {editableObject.transform.position.z}</color>");
            _serializedObjects.Add(new SerializableObject
            {
                Type = editableObject.type,
                   X = editableObject.transform.position.x,
                   Y = editableObject.transform.position.y,
                   Z = editableObject.transform.position.z,
                RotX = 0.0f,
                RotY = 0.0f,
                RotZ = 0.0f,
                RotW = 1.0f
            });
            
            _dynamicObjects.Add(editableObject);
            
            PlaceObjectUI.OnUpdatePlaceObjectUICount();
        }

        public void ObjectChanged(EditableObject obj)
        {
            SerializableObject updatedSerializedObj = _serializedObjects[obj.ObjectID];
            updatedSerializedObj.Type = obj.type;
            updatedSerializedObj.X = obj.transform.localPosition.x;
            updatedSerializedObj.Y = obj.transform.localPosition.y;
            updatedSerializedObj.Z = obj.transform.localPosition.z;

            updatedSerializedObj.RotX = obj.transform.rotation.x;
            updatedSerializedObj.RotY = obj.transform.rotation.y;
            updatedSerializedObj.RotZ = obj.transform.rotation.z;
            updatedSerializedObj.RotW = obj.transform.rotation.w;

            _serializedObjects[obj.ObjectID] = updatedSerializedObj;
        }

        public void DeleteObject(EditableObject obj)
        {
            Debug.Log("<color=lime>[ObjectManager] Deleting object</color>");
            for (int i = obj.ObjectID + 1; i < _dynamicObjects.Count; i++)
            {
                _dynamicObjects[i].ObjectID--;
            }
            
            _serializedObjects.RemoveAt(obj.ObjectID);
            _dynamicObjects.RemoveAt(obj.ObjectID);

            PlaceObjectUI.OnUpdatePlaceObjectUICount();
        }

        public int GetObjectCount()
        {
            return _serializedObjects.Count;
        }

        public int GetAmountUsed(EditableObjectType type)
        {
            return _dynamicObjects.Count(x => x.type == type);
        }
    }
}
