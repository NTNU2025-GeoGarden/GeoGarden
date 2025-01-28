using System.Collections.Generic;
using Garden;
using UnityEditor;
using UnityEngine;

namespace Stateful.Managers
{
    public class ObjectManager : MonoBehaviour, IUsingGameState
    {
        public EditableObject objPrefab;
        private List<SerializableObject> _serializedObjects;
        private List<EditableObject> _dynamicObjects;

        private void GenerateInGameObjects()
        {
            if (_dynamicObjects == null)
                _dynamicObjects = new List<EditableObject>();
            else
            {
                foreach (EditableObject obj in _dynamicObjects)
                {
                    Destroy(obj.gameObject);
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
                
                //TODO fix rotation of edit buttons
                
                instantiatedObj.ObjectID = count;
                
                _dynamicObjects.Add(instantiatedObj);

                count++;
            }
        }
        
        public void LoadData(GameState state)
        {
            _serializedObjects = state.Objects;
            
            GenerateInGameObjects();
        }

        public void SaveData(ref GameState state)
        {
            state.Objects = _serializedObjects;
        }

        public void AddObject(EditableObject editableObject)
        {
            _serializedObjects.Add(new SerializableObject
            {
                X    = editableObject.transform.position.x,
                Y    = editableObject.transform.position.y,
                Z    = editableObject.transform.position.z,
                Type = editableObject.type
            });
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
            _serializedObjects.RemoveAt(obj.ObjectID);
        }

        public int GetObjectCount()
        {
            return _serializedObjects.Count;
        }
    }
}
