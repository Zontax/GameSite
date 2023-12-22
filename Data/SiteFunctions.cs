using System.Text.RegularExpressions;
using GameSite.Models;
using Humanizer;

namespace GameSite.Data;

public static class SiteFunctions
{
    public static string GetColorCode(PostType typeId)
    {
        return typeId switch
        {
            PostType.Новина => "rgb(201, 142, 5)",
            PostType.Огляд => "orange",
            PostType.Стаття => "rgb(204, 190, 0)",
            PostType.Гайд => "green",
            _ => "rgb(201, 142, 5)",
        };
    }

    public static string GetReviewText(int? rating)
    {
        string responce;
        if (rating < 10) responce = "Дуже погано";
        else if (rating < 25) responce = "Погано";
        else if (rating < 50) responce = "Ні риба ні м'ясо";
        else if (rating < 75) responce = "Варте уваги";
        else if (rating < 100) responce = "Рекомендуємо";
        else responce = "Рекомендуємо";

        return responce.ToUpper();
    }

    public static string ProcessContentForEmbeddedVideo(string content)
    {
        string pattern = "<oembed\\b[^>]*>(.*?)<\\/oembed>";
        var regex = new Regex(pattern);
        Match match = regex.Match(content);

        if (match.Success)
        {
            string oembedContent = match.Groups[1].Value;
            var urlMatch = Regex.Match(oembedContent, "url=['\"]?([^'\" >]+)");
            if (urlMatch.Success)
            {
                string videoUrl = urlMatch.Groups[1].Value;
                string embeddedVideo = $"<iframe width='560' height='315' src='{videoUrl}' frameborder='0' allowfullscreen></iframe>";
                return embeddedVideo;
            }
        }

        return string.Empty;
    }
}

