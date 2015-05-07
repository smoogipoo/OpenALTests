using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Audio.Decoders
{
    public class WAVEDecoder : AudioDecoder
    {
        public WAVEFormat Format;
        public uint BytesPerSecond;
        public ushort BlockAlign;

        public WAVEDecoder(string file)
            : base(file)
        { }

        protected override void Read()
        {
            if (Reader.PeekByte() == -1)
                return;

            string ckID = new string(Reader.ReadChars(4)).ToUpper().Trim();
            uint ckSize = Reader.ReadUInt32();

            switch (ckID)
            {
                case "RIFF":
                    {
                        string waveID = new string(Reader.ReadChars(4));
                        if (waveID != "WAVE")
                            throw new Exception("Wave type is not supported.", new Exception(waveID));
                    }
                    break;
                case "FMT":
                    {
                        Format = (WAVEFormat)Reader.ReadInt16();
                        Channels = Reader.ReadUInt16();                   //N_c
                        SampleRate = Reader.ReadUInt32();                 //F
                        BytesPerSecond = Reader.ReadUInt32();             //F*M*N_c
                        BlockAlign = Reader.ReadUInt16();                 //M*N_c
                        BitsPerSample = Reader.ReadUInt16();              //8*M

                        if (Format != WAVEFormat.PCM)
                        {
                            Reader.ReadUInt16();
                        }

                        if (Format == WAVEFormat.EXTENSIBLE)
                        {
                            Reader.ReadUInt16();
                            Reader.ReadUInt32();
                            Reader.ReadChars(16);
                        }
                    }
                    break;
                case "DATA":
                    {
                        Samples = Reader.ReadBytes((int)ckSize);
                    }
                    break;
                default:
                    {
                        Reader.ReadBytes((int)ckSize);
                    }
                    break;
            }

            Read();
        }
    }

    public enum WAVEFormat : ushort
    {
        PCM = 0x0001,
        IEEE_FLOAT = 0x0003,
        ALAW = 0x0006,
        MULAW = 0x0007,
        EXTENSIBLE = 0xFFFE
    }
}
