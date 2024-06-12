using System;
using System.Collections.Generic;

namespace ATA_GUI.Classes
{
    internal class EncoderData
    {
        public string Codec { get; set; }
        public string Encoder { get; set; }

        public EncoderData(string codec, string encoder)
        {
            Codec = codec;
            Encoder = encoder;
        }

        public static List<EncoderData> ExtractEncoders(string input, MEDIA type)
        {
            var encoders = new List<EncoderData>();
            bool isCorrectSection = false;
            string sectionIdentifier = type == MEDIA.VIDEO ? "List of video encoders:" : "List of audio encoders:";
            string codecPrefix = type == MEDIA.VIDEO ? "--video-codec" : "--audio-codec";

            foreach (var line in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains(sectionIdentifier))
                {
                    isCorrectSection = true;
                    continue;
                }

                if (line.Contains("[server] INFO: List of") && !line.Contains(sectionIdentifier))
                {
                    isCorrectSection = false;
                    continue;
                }

                if (isCorrectSection && line.Trim().StartsWith(codecPrefix))
                {
                    var parts = line.Trim().Split(' ');
                    var codec = parts[0].Split('=')[1];
                    var encoder = parts[1].Split('=')[1].Trim('\'');
                    encoders.Add(new EncoderData(codec, encoder));
                }
            }

            return encoders;
        }
    }

    internal enum MEDIA
    {
        AUDIO,
        VIDEO
    }
}
