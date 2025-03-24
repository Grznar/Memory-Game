using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MainMenu
{
    public class SoundManager
    {
        private readonly bool isSoundEnabled;
        private Dictionary<string, MemoryStream> audioStreams;
        private IWavePlayer waveOut;
        private Mp3FileReader mp3Reader;

        public SoundManager(bool isSound)
        {
            isSoundEnabled = isSound;
            audioStreams = new Dictionary<string, MemoryStream>();
            waveOut = new WaveOutEvent();

            if (isSound)
                LoadAudio();
            else
                return;
        }

        public void LoadAudio()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceFolder = "MainMenu.Root.Audio";
            string[] resourceNames = assembly.GetManifestResourceNames();

            foreach (string resourceName in resourceNames)
            {
                if (resourceName.StartsWith(resourceFolder) &&
                    resourceName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
                    if (resourceStream != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        resourceStream.CopyTo(memoryStream);
                        memoryStream.Position = 0;
                        string fileName = resourceName.Substring(resourceFolder.Length + 1);
                        fileName = Path.GetFileNameWithoutExtension(fileName);
                        audioStreams[fileName] = memoryStream;
                    }
                }
            }

            if (audioStreams.Count == 0)
                MessageBox.Show("Nebyly nalezeny žádné zvukové soubory!");
        }

        public void PlayAudio(string audioName)
        {
            if (!isSoundEnabled)
                return;
            if (audioStreams.ContainsKey(audioName))
            {
                StopAudio();
                MemoryStream memoryStream = audioStreams[audioName];
                memoryStream.Position = 0;
                mp3Reader = new Mp3FileReader(memoryStream);
                waveOut.Init(mp3Reader);
                waveOut.Play();
            }
            else
            {
                if (isSoundEnabled)
                    MessageBox.Show("Neplatný název zvuku!");
            }
        }

        public void StopAudio()
        {
            if (!isSoundEnabled)
                return;
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
                waveOut.Stop();

            mp3Reader?.Dispose();
            mp3Reader = null;
            if (mp3Reader != null)
            {
                mp3Reader.Dispose();
                mp3Reader = null;
            }
        }

        public void Dispose()
        {
            StopAudio();
            waveOut?.Dispose();
            foreach (var stream in audioStreams.Values)
                stream.Dispose();
            audioStreams.Clear();
        }

        public void PlayFlipCardSound()
        {
            PlayAudio("flipCard");
        }

        public void PlayMatchedCorrect()
        {
            PlayAudio("isMatching");
        }

        public void PlayMatchedWrong()
        {
            PlayAudio("notMatching");
        }
    }
}
