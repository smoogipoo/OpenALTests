using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Audio.Decoders
{
    public class FLACDecoder : AudioDecoder
    {
        public ushort MinSampleSize;
        public ushort MaxSampleSize;

        public FLACDecoder(string file)
            : base(file)
        { }

        protected override void Read()
        {
            string flac = new string(Reader.ReadChars(4));
            if (flac != "fLaC")
                throw new Exception("Invalid FLAC type.", new Exception(flac));

            //StreamInfo

        }
    }
}
