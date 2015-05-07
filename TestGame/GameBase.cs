using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestGame.Audio.Decoders;

namespace TestGame
{
    public class GameBase
    {
        public static long Time;

        public GameWindow Game;

        private Stopwatch stopWatch;
        private AudioDecoder decoder;

        public GameBase(GameWindow game)
        {
            Game = game;

            Game.Closed += delegate { Environment.Exit(0); };
            Game.UpdateFrame += Update;
            Game.RenderFrame += Draw;

            initialize();
        }

        private void initialize()
        {
            if (decoder == null)
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        decoder = new WAVEDecoder(ofd.FileName);
                }
            }
        }

        public void Draw(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0, 0, 0, 0);

            Game.SwapBuffers();
        }

        public void Update(object sender, FrameEventArgs e)
        {
            if (stopWatch == null)
                stopWatch = Stopwatch.StartNew();

            Time = stopWatch.ElapsedMilliseconds;
        }
    }
}
