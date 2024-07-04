
using System;
using System.IO;

namespace Cave_dweller
{
    public static class FileHelper
    {
        private static string HighScoreFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "highscore.txt");

        static FileHelper()
        {
            string directory = Path.GetDirectoryName(HighScoreFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static int LoadHighScore()
        {
            if (File.Exists(HighScoreFilePath))
            {
                string scoreText = File.ReadAllText(HighScoreFilePath);
                if (int.TryParse(scoreText, out int highScore))
                {
                    return highScore;
                }
            }
            return 0;
        }

        public static void SaveHighScore(int highScore)
        {
            File.WriteAllText(HighScoreFilePath, highScore.ToString());
        }
    }
}
