using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PoemTown.Repository.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Utils
{
    public static class FileAudioHelper
    {
        private static readonly string[] permittedExtensions = { ".mp3", ".wav", ".m4a", ".flac", ".ogg" };

        public static bool ValidateAudios(IList<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "File is required");
            }

            foreach (var file in files)
            {
                ValidateAudio(file);
            }

            return true;
        }

        public static bool ValidateAudio(IFormFile file)
        {
            if (!file.ContentType.StartsWith("audio/"))
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Only audio files are allowed.");
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "File extension is not allowed. Only .mp3, .wav, .m4a, .flac, .ogg are allowed.");
            }

            // Kiểm tra chữ ký file (nếu cần)
            if (!IsAudio(file))
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Content does not match the expected audio type.");
            }

            return true;
        }

        private static bool IsAudio(IFormFile file)
        {
            byte[] buffer = new byte[64]; // Đọc nhiều byte hơn
            using (var stream = file.OpenReadStream())
            {
                stream.Read(buffer, 0, buffer.Length);
            }

            // Signature phổ biến cho các định dạng âm thanh
            var id3Signature = new byte[] { 0x49, 0x44, 0x33 }; // "ID3" cho MP3 có thẻ ID3
            var mp3Signature = new byte[] { 0xFF, 0xFB }; // MP3 (MPEG Audio Frame)
            var wavSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 }; // WAV (RIFF)
            var flacSignature = new byte[] { 0x66, 0x4C, 0x61, 0x43 }; // FLAC
            var oggSignature = new byte[] { 0x4F, 0x67, 0x67, 0x53 }; // OGG

            var m4aSignatures = new List<byte[]>
    {
        new byte[] { 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41 }, // ftypM4A
        new byte[] { 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D }, // ftypisom
        new byte[] { 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 }, // ftypmp42
        new byte[] { 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x56 } // ftypM4V
    };

            // Kiểm tra signature cho các định dạng MP3, WAV, FLAC, OGG
            if (buffer.Take(3).SequenceEqual(id3Signature) ||
                buffer.Take(2).SequenceEqual(mp3Signature) ||
                buffer.Take(4).SequenceEqual(wavSignature) ||
                buffer.Take(4).SequenceEqual(flacSignature) ||
                buffer.Take(4).SequenceEqual(oggSignature))
            {
                return true;
            }

            // Kiểm tra M4A trong 64 byte đầu (do signature có thể không nằm ngay đầu)
            for (int i = 0; i < buffer.Length - 8; i++)
            {
                foreach (var sig in m4aSignatures)
                {
                    if (buffer.Skip(i).Take(sig.Length).SequenceEqual(sig))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


    }
}
