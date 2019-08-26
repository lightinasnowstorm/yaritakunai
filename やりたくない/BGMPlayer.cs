#define mute_start
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace やりたくない
{
    internal static class BGMPlayer
    {
        public static void init()
        {
            Main.switchScreenHandler += onScreenChange;
#if mute_start
            muted = true;
#endif
        }
        private static HashSet<string> allSongNames = new HashSet<string>()
        {
            "Title"
        };
        private static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        public static void LoadSongs(ContentManager Content)
        {
            string songRoot = "Music" + Path.DirectorySeparatorChar;
            foreach (string songName in allSongNames)
            {
                songs.Add(songName, Content.Load<Song>(songRoot + songName));
            }
            //post load init of song
            onScreenChange(Main.currentScreen);
        }

        public static float volume
        {
            get => MediaPlayer.Volume;
            set { MediaPlayer.Volume = value; }
        }

        public static bool muted
        {
            get => MediaPlayer.IsMuted;
            set { MediaPlayer.IsMuted = value; }
        }

        public static void play()
        {
            MediaPlayer.Resume();
        }
        public static void play(string songName, bool repeating = true)
        {
            if (songs.ContainsKey(songName))
            {
                MediaPlayer.Play(songs[songName]);
                MediaPlayer.IsRepeating = repeating;
            }
            //silent failure
        }
        public static void pause()
        {
            MediaPlayer.Pause();
        }
        public static void stop()
        {
            MediaPlayer.Stop();
        }
        public static void onScreenChange(Object data)
        {
            if ((screens)data == screens.title)
            {
                play("Title");
            }
            else
            {
                stop();
            }
        }
    }
}
