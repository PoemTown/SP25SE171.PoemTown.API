using System.ComponentModel;

namespace PoemTown.Repository.Enums.Poems;

public enum PoemType
{
    [Description("Thơ tự do")]
    ThoTuDo = 1,

    [Description("Thơ Lục bát")]
    ThoLucBat = 2,

    [Description("Thơ Song thất lục bát")]
    ThoSongThatLucBat = 3,

    [Description("Thơ Thất ngôn tứ tuyệt")]
    ThoThatNgonTuTuyet = 4,

    [Description("Thơ Ngũ ngôn tứ tuyệt")]
    ThoNguNgonTuTuyet = 5,

    [Description("Thơ Thất ngôn bát cú")]
    ThoThatNgonBatCu = 6,

    [Description("Thơ bốn chữ")]
    ThoBonChu = 7,

    [Description("Thơ năm chữ")]
    ThoNamChu = 8,

    [Description("Thơ sáu chữ")]
    ThoSauChu = 9,

    [Description("Thơ bảy chữ")]
    ThoBayChu = 10,

    [Description("Thơ tám chữ")]
    ThoTamChu = 11,
}