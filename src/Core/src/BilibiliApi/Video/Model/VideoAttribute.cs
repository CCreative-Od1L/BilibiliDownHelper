namespace Core.BilibiliApi.Video.Model;

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