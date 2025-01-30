using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Stateful
{
    public class FileDataHandler
    {
        private string saveDataDirPath = "";
        private string saveDataFileName = "";

        public FileDataHandler(string saveDataDirPath, string saveDataFileName)
        {
            this.saveDataDirPath = saveDataDirPath;
            this.saveDataFileName = saveDataFileName;
        }

        public GameState Load()
        {
            string fullPath = Path.Combine(saveDataDirPath, saveDataFileName);

            GameState loadedState = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad;

                    using (FileStream file = new(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new(file))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    loadedState = JsonConvert.DeserializeObject<GameState>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.Log("<color=red>[FileDataHandler] Error when loading from file.</color>");
                    Debug.LogError(e);
                    throw;
                }
            }

            return loadedState;
        }

        public void Save(GameState state)
        {
            string fullPath = Path.Combine(saveDataDirPath, saveDataFileName);
            try
            {
                // Create dir if it doesnt exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            
                // Serialize data
                string dataToStore = JsonConvert.SerializeObject(state, Formatting.Indented);
            
                // Write it!

                using (FileStream file = new(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new(file))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("<color=red>[FileDataHandler] Error when saving data to file.</color>");
                Debug.LogError(e);
                throw;
            }
        }
    }
}
