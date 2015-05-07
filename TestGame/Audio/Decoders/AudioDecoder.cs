using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Helpers;

namespace TestGame.Audio.Decoders
{
    public abstract class AudioDecoder : IDisposable
    {
        public byte[] Samples;
        public ushort Channels;
        public uint SampleRate;
        public ushort BitsPerSample;

        protected BinaryBitReader Reader;

        public AudioDecoder(string file)
        {
            Reader = new BinaryBitReader(File.OpenRead(file));

            try
            {
                Read();
            }
            catch { }
        }

        protected abstract void Read();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                Reader.Dispose();
        }
    }
}
