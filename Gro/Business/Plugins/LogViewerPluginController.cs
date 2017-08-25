using System;
using EPiServer.PlugIn;
using log4net;
using log4net.Appender;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Gro.Business.Plugins
{
    public class LogViewerPluginViewModel
    {
        public IEnumerable<FileInfo> LogFiles { get; set; }
        public string TypeFilter { get; set; }
        public string FromFilter { get; set; }
        public string ToFilter { get; set; }
    }

    [GuiPlugIn(
        Area = PlugInArea.AdminMenu,
        Url = "/custom-plugins/LogViewerPlugin",
        DisplayName = "Log Viewer")]
    [Authorize(Roles = "CmsAdmins")]
    public class LogViewerPluginController : Controller
    {
        public ActionResult Index(string typeFilter, DateTime? fromFilter, DateTime? toFilter)
        {
            if (string.IsNullOrWhiteSpace(typeFilter) && fromFilter == null && toFilter == null)
            {
                return View("~/Views/Plugins/LogViewer/Index.cshtml", new LogViewerPluginViewModel());
            }

            var rollingFilesAppenders = LogManager
                .GetRepository()
                .GetAppenders()
                .OfType<RollingFileAppender>();

            var hs = new HashSet<string>();
            var logFiles = rollingFilesAppenders.SelectMany(a =>
            {
                var dirName = new DirectoryInfo(Path.GetDirectoryName(a.File));
                if (hs.Contains(dirName.FullName)) return new FileInfo[0];
                hs.Add(dirName.FullName);

                var files = dirName.GetFiles().Where(f => f.Extension == ".log");
                if (!string.IsNullOrWhiteSpace(typeFilter))
                {
                    files = files.Where(f => f.Name.StartsWith($"episerver{typeFilter}", StringComparison.OrdinalIgnoreCase));
                }
                if (fromFilter != null)
                {
                    files = files.Where(f => f.CreationTime > fromFilter.Value);
                }
                if (toFilter != null)
                {
                    files = files.Where(f => f.CreationTime < toFilter.Value);
                }

                return files;
            }).OrderByDescending(l => l.CreationTimeUtc);

            return View("~/Views/Plugins/LogViewer/Index.cshtml", new LogViewerPluginViewModel
            {
                LogFiles = logFiles,
                TypeFilter = typeFilter,
                FromFilter = fromFilter?.ToString("yyyy-MM-dd") ?? string.Empty,
                ToFilter = toFilter?.ToString("yyyy-MM-dd") ?? string.Empty
            });
        }

        public ActionResult LogFile(string filePath)
        {
            try
            {
                var stream = new FileStream(filePath, FileMode.Open);
                return new FileStreamResult(stream, "text/plain");
            }
            catch (FileNotFoundException ex)
            {
                return new HttpStatusCodeResult(404, $"Could not find {ex.FileName}");
            }
        }
    }
}
