﻿// Source: https://github.com/Vocaluxe/Vocaluxe

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenJinglePlayer.Lib.Sound.Decoder
{
    struct FormatInfo
    {
        public int ChannelCount;
        public int SamplesPerSecond;
        public int BitDepth;
    }

    interface IAudioDecoder
    {
        void Init();
        void Close();

        void Open(string FileName);
        void Open(string FileName, bool Loop);
        FormatInfo GetFormatInfo();

        float GetLength();

        void SetPosition(float Time);
        float GetPosition();

        void Decode(out byte[] Buffer, out float TimeStamp);
    }
}
