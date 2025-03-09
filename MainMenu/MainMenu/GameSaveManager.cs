using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
namespace MainMenu
{
    public static class GameSaveManager
    {
       
            
            public static void SaveGame(GameSave data, string filePath)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, data);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Chyba při ukládání hry: " + ex.Message, ex);
                }
            }


            public static GameSave LoadGame(string filePath)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        var loadedGame = (GameSave)formatter.Deserialize(fs);
                        return loadedGame;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Chyba při načítání hry: " + ex.Message, ex);
                    
                }
            }
        }
    }

