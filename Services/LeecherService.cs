using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace FindLeecher.Services
{
    public static class LeecherService
    {
        private static async Task<HtmlDocument> DownloadHtmlSource(string torrentSiteUrl)
        {
            var web = new HtmlWeb();

            var htmlDocument = await web.LoadFromWebAsync(torrentSiteUrl);

            return htmlDocument;
        }

        public static async Task<List<string>> GetTorrents(string torrentSiteUrl, int minimumLeecher)
        {
            var htmlDocument = await DownloadHtmlSource(torrentSiteUrl);

            var torrents = htmlDocument.DocumentNode.SelectNodes("//table[contains(@class, 'table-list')]/tbody/tr");

            var torrentLinks = new List<string>();
            foreach (var torrent in torrents)
            {
                var canParse = int.TryParse(torrent.ChildNodes[5].InnerText, out var leecherAmount);

                if (canParse && leecherAmount >= minimumLeecher)
                    torrentLinks.Add($"https://1337x.to{torrent.ChildNodes[1].ChildNodes[1].Attributes["href"].Value}");
            }

            return torrentLinks;
        }
    }
}