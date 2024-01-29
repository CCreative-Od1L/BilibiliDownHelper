using Xabe.FFmpeg;

namespace Core.FFmpegFunc;
public class FFmpegAPI {
    public static async Task<IConversionResult> MixAudio(
        string output,
        string videoFilePath,
        string audioFilePath
    ) {
        var conversion = await FFmpeg.Conversions.FromSnippet.AddAudio(
            videoPath: videoFilePath,
            audioPath: audioFilePath,
            outputPath: output
        );
        IConversionResult result = await conversion.Start();
        return result;
    }
    public static async Task<(IConversionResult, string)> ChangeVideoFormat(string input, string format, VideoCodec codec) {
        string outputPath = Path.ChangeExtension(input, format);

        IMediaInfo info = await FFmpeg.GetMediaInfo(input);
        IStream? videoStream = info.VideoStreams.FirstOrDefault()?.SetCodec(codec);

        return (await FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .SetOutput(outputPath)
                .Start(),
                outputPath);
    }
    public static async Task<(IConversionResult, string)> ChangeAudioFormat(string input, string format, AudioCodec codec) {
        string outputPath = Path.ChangeExtension(input, format);

        IMediaInfo info = await FFmpeg.GetMediaInfo(input);
        IStream? audioStream = info.AudioStreams.FirstOrDefault()?.SetCodec(codec);

        return (await FFmpeg.Conversions.New()
            .AddStream(audioStream)
            .SetOutput(outputPath)
            .Start(),
            outputPath);
    }
}