using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace ImageEXIFScanner
{
    public class EXIFScanner : IHostedService
    {
        private static readonly IList<VisualFeatureTypes?> features =
            new List<VisualFeatureTypes?>()
{
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
};
        private readonly string endpoint = "https://analyzemyimages.cognitiveservices.azure.com/";
        private readonly string key = "4dc779469be24958b62c906ca78feafd";
        private string[] GetTags(string imagepath)
        {
            IList<string> ret = new string[0];
            using (var client = AzureFunctions.Authenticate(endpoint,key))
            {
                using (var file = File.OpenRead(imagepath))
                {
                    var results = client.AnalyzeImageInStreamAsync(file,features).Result;
                    ret = results.Description.Captions.Select(a => a.Text.ToString()).ToList() ?? new List<string>();
                    ret = ret.Concat(results.Tags.Select(a => $"{a.Hint}:{a.Name}").Where(a => !string.IsNullOrWhiteSpace(a)) ?? new List<string>()).ToList();
                    ret = ret.Concat(results?.Objects?.Select(a => a?.ObjectProperty ?? "").Where(a=>!string.IsNullOrWhiteSpace(a)) ?? new List<string>()).ToList();
                }
            }
            return ret.ToArray();
        }
        public EXIFScanner(ILogger<EXIFScanner> logger, IHostApplicationLifetime appLifetime,IConfiguration config)
        {
            Logger = logger;
            AppLifetime = appLifetime;
            Config = config;
            ImageExtensions = Config["ImageExtensions"].Split(',').Select(a => a.Trim()).ToList();
        }

        private ILogger<EXIFScanner> Logger { get; }
        private IHostApplicationLifetime AppLifetime { get; }
        private IConfiguration Config { get; }
        private volatile bool Stopping = false;
        private List<string> ImageExtensions { get; set; }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            AppLifetime.ApplicationStarted.Register(AppStarted);
            AppLifetime.ApplicationStopping.Register(AppStopping);
            AppLifetime.ApplicationStopped.Register(AppStopped);
            return Task.CompletedTask;
        }

        private void AppStopped()
        {
            Console.WriteLine("Work Completed");
        }

        private void AppStopping()
        {
            Stopping = true;
        }

        bool extensionisimage(string path) =>
            ImageExtensions.Where(a => path.EndsWith(a, StringComparison.CurrentCultureIgnoreCase)).Select(a => a).Count() > 0;
        private void AppStarted()
        {
            var workingpath = Config["path"];
            List<string> files = new List<string>();
            if ((File.GetAttributes(workingpath) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                files = Directory.EnumerateFiles(workingpath, "*.*", new EnumerationOptions()
                {
                    RecurseSubdirectories = true,
                    IgnoreInaccessible = true
                })
                    .Where(a => extensionisimage(a))
                    .Select(a => a)
                    .ToList();
            }
            else if (File.Exists(workingpath) && extensionisimage(workingpath))
                files.Add(workingpath);


            if (files.Count == 0)
                Console.WriteLine("No Files Found");
            foreach (var file in files)
            {
                var tags = JsonConvert.SerializeObject(GetTags(file));
                Console.WriteLine($"{Path.GetFileName(file)} Subject Matter: {tags}");
                using(var img = Image.FromFile(file))
                {
                    var properties = img.PropertyItems;
                    var commentitem = img.GetPropertyItem(0);
                    commentitem.Id = 0x9286;
                    commentitem.Value = System.Text.Encoding.Default.GetBytes(tags);
                    commentitem.Len = tags.Length;
                    commentitem.Type = 7;
                    img.SetPropertyItem(commentitem);
                }
            }
            AppLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}