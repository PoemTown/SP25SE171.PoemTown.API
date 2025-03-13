using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class ConvertPoemTextToImageRequest
{
    [FromQuery(Name = "imageSize")]
    public ImageSize ImageSize { get; set; }
    [FromQuery(Name = "imageStyle")]
    public ImageStyle ImageStyle { get; set; }
    /*[FromQuery(Name = "imageQuality")]
    public ImageQuality ImageQuality { get; set; }*/
    [FromQuery(Name = "poemText")]
    public string PoemText { get; set; }
    [FromQuery(Name = "prompt")]
    public string Prompt { get; set; }
}

public enum ImageSize
{
    /*[Description("256x256")]
    Size256X256 = 1,
    [Description("512x512")]
    Size512X512 = 2,*/
    [Description("1024x1024")]
    Size1024X1024 = 1,
    [Description("1024x1792")]
    Size1024X1792 = 2,
}

public enum ImageStyle
{
    [Description("natural")]
    Natural = 1,
    [Description("vivid")]
    Vivid = 2,
}

public enum ImageQuality
{
    [Description("standard")]
    Standard = 1,
    [Description("hd")]
    Hd = 2
}