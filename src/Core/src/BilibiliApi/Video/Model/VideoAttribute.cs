namespace Core.BilibiliApi.Video.Model;
/// <summary>
/// qn
/// * 在DASH格式下无效
/// </summary>
public enum VIDEO_QUALITY {
    _240P = 6,          // * platform=html5 才有效
    _360P = 16,
    _480P = 32,
    _720P = 64,         // * WEB 默认（需登录）
    _720P_HighHz = 74,
    _1080P = 80,
    _1080P_PLUS = 112,  // * 大会员
    _1080P_HighHz = 116,
    _4K = 120,          // * fnval&128=128, fourk=1
    _HDR_TC = 125,      // * fnval&64=64 仅支持DASH格式
    _DOLBY = 126,       // * fnval&512=512 仅支持DASH格式
    _8K = 127,          // * fnval&1024=1024 仅支持DASH格式
}
public enum VIDEO_FNVAL {
    FLV = 0,            // * 已弃用
    MP4 = 1,
    DASH = 16,
    HDR = 64,           // * 是否需要HDR视频
    _4K = 128,          // * 是否需求   4K
    DOLBY_AUDIO = 256,  // * 是否需要杜比音频
    DOLBY_SCENE = 512,  // * 是否需要杜比视界
    _8K = 1024,         // * 是否需求8K
    AV1_ENCODE = 2048   // * 是否需求AV1编码
}
public enum VIDEO_CODE_CID  {
    AVC = 7,
    HEVC = 12,
    AV1 = 13,
}
public enum AUDIO_QUALITY {
    _64K = 30216,
    _132K = 30232,
    _192K = 30280,
    DOLBY = 30250,      // * 杜比全景声
    HI_RES = 30251      // * Hi-Res无损
}