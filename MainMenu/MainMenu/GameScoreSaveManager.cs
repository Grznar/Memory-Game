using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class GameScoreSaveManager
    {
        private static readonly string ScoreFilePath = "scoreData.bin";

       
        public static void SaveScoreData(List<ScoreData> scoreData)
        {
            try
            {
                using (FileStream fs = new FileStream(ScoreFilePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, scoreData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Chyba při ukládání skóre: " + ex.Message);
            }
        }

       
        public static List<ScoreData> LoadScoreData()
        {
            try
            {
                if (!File.Exists(ScoreFilePath))
                    return new List<ScoreData>();

                using (FileStream fs = new FileStream(ScoreFilePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (List<ScoreData>)formatter.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Chyba při načítání skóre: " + ex.Message);
            }
        }

        
        public static void ClearScoreData()
        {
            try
            {
                if (File.Exists(ScoreFilePath))
                {
                    File.Delete(ScoreFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Chyba při mazání skóre: " + ex.Message);
            }
        }
    }
}
