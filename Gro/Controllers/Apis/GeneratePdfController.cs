using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EPiServer.Logging.Compatibility;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Gro.Constants;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.AppPages.DeliveryAssurance;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.Interfaces;
using Gro.Helpers;
using HtmlAgilityPack;
using WebSupergoo.ABCpdf9;
using Zen.Barcode;

namespace Gro.Controllers.Apis
{
    [RoutePrefix("api/pdfhandler")]
    public class GeneratePdfController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GeneratePdfController));
        private static SettingsPage SettingPage => ContentExtensions.GetSettingsPage();

        private static DeliveryAssuranceListPage DeliveryAssurancePage
            => SettingPage.DeliveryAssurancePage.Get<DeliveryAssuranceListPage>();

        private static string GenerateDeliveryPdfUrl
        {
            get
            {
                var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
                return urlHelper.ContentUrl(SettingPage.DeliveryAssurancePage);
            }
        }

        private readonly IDeliveryAssuranceRepository _deliveryNoteRepository;

        public GeneratePdfController(IDeliveryAssuranceRepository deliveryNoteRepository)
        {
            _deliveryNoteRepository = deliveryNoteRepository;
        }

        [Route("generatemultipdf")]
        public ActionResult GenerateMultiPdf(string a, string l, string resync)
        {
            var sep = new[] { '|' };
            var ios = a.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            var lineNos = l.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            if (ios.Any() && lineNos.Any() && ios.Count().Equals(lineNos.Count()))
            {
                var allDocs = new Doc();

                byte[] pdfs = null;

                for (var i = 0; i < ios.Count(); i++)
                {
                    CreateAndAppendDeliveryAssurancesPdf(Request, ios[i], Convert.ToInt32(lineNos[i]), resync == "1", ref allDocs, ref pdfs);
                }

                allDocs.ClearCachedDecompressedStreams();
                allDocs.Clear();
                allDocs.Dispose();

                if (pdfs != null)
                {
                    Response.AddHeader("Content-Disposition", "inline; filename=DeliveryAssurances.pdf");
                    return File(pdfs, "application/pdf");
                }
            }

            return HttpNotFound();
        }

        [Route("generatepdf")]
        public ActionResult GeneratePdf(string a, string l, string resync)
        {
            var ioNumber = a;
            int lineNumber;

            if (!string.IsNullOrWhiteSpace(ioNumber) && !string.IsNullOrWhiteSpace(l) && int.TryParse(l, out lineNumber))
            {
                var theDoc = new Doc();

                var pdf = CreateDeliveryAssurancePdf(Request, ioNumber, lineNumber, resync == "true", ref theDoc);

                theDoc.ClearCachedDecompressedStreams();
                theDoc.Clear();
                theDoc.Dispose();

                if (pdf != null)
                {
                    Response.AddHeader("Content-Disposition", "inline; filename=DeliveryAssurance.pdf");
                    return File(pdf, "application/pdf");
                }
            }

            return HttpNotFound();
        }

        [Route("generate-drying")]
        public ActionResult GenerateDryingAgrementPdf(string resync = "true")
        {
            var theDoc = new Doc();

            var pdf = CreateDryingAgreementPdf(Request, resync == "true", ref theDoc);

            theDoc.ClearCachedDecompressedStreams();
            theDoc.Clear();
            theDoc.Dispose();

            if (pdf == null) return HttpNotFound();

            Response.AddHeader("Content-Disposition", "inline; filename=DryingAgreement.pdf");
            return File(pdf, "application/pdf");
        }

        private byte[] CreateDryingAgreementPdf(HttpRequestBase request, bool resync, ref Doc theDoc)
        {
            var retID = 0;

            theDoc = new Doc();

            if (resync)
            {
                theDoc.HtmlOptions.Engine = EngineType.Gecko;
                theDoc.HtmlOptions.PageCacheEnabled = false;
                theDoc.HtmlOptions.UseNoCache = true;
                theDoc.HtmlOptions.PageCacheClear();
                theDoc.HtmlOptions.PageCachePurge();
                theDoc.HtmlOptions.UseResync = true;
            }

            theDoc.Rect.Inset(20, 20);
            theDoc.ClearCachedDecompressedStreams();

            const string generatePdfUrl = "/api/drying-agreement/generatePdf";

            var responseStream = GetWebResponseStream(request, generatePdfUrl);
            if (responseStream != null)
            {
                var outerHtml = FixRelativeLinksToAbsolute(responseStream, request.Url);
                Log.DebugFormat("HtmlResponse with relative links {0}", outerHtml);

                retID = theDoc.AddImageHtml(outerHtml);

            }
            else
            {
                Log.DebugFormat("PDFResponseStream was null");
            }

            var theData = theDoc.GetData();

            return theData;
        }

        private void CreateAndAppendDeliveryAssurancesPdf(HttpRequestBase request, string id, int lineNumber, bool resync, ref Doc allDocs, ref byte[] pdfOut)
        {
            var theDoc = new Doc();

            CreateDeliveryAssurancePdf(request, id, lineNumber, resync, ref theDoc);

            if (allDocs == null)
                allDocs = new Doc();

            allDocs.Append(theDoc);

            pdfOut = allDocs.GetData();

            theDoc.ClearCachedDecompressedStreams();
            theDoc.Clear();
            theDoc.Dispose();
        }

        private byte[] CreateDeliveryAssurancePdf(HttpRequestBase request, string id, int lineNumber, bool resync, ref Doc theDoc)
        {
            int retID = 0;

            theDoc = new Doc();

            if (resync)
            {
                theDoc.HtmlOptions.Engine = EngineType.Gecko;
                theDoc.HtmlOptions.PageCacheEnabled = false;
                theDoc.HtmlOptions.UseNoCache = true;
                theDoc.HtmlOptions.PageCacheClear();
                theDoc.HtmlOptions.PageCachePurge();
                theDoc.HtmlOptions.UseResync = true;
            }

            theDoc.Rect.Inset(20, 20);
            theDoc.ClearCachedDecompressedStreams();

            var generatePdfUrl = DeliveryAssuranceHelper.BuildQueryUrl($"{GenerateDeliveryPdfUrl}GeneratePdf",
                new Dictionary<string, string>()
                {
                    {"a", id},
                    {"l", lineNumber.ToString()}
                });

            var responseStream = GetWebResponseStream(request, generatePdfUrl);
            if (responseStream != null)
            {
                var outerHtml = FixRelativeLinksToAbsolute(responseStream, request.Url);
                Log.DebugFormat("HtmlResponse with relative links {0}", outerHtml);

                retID = theDoc.AddImageHtml(outerHtml);

            }
            else
            {
                Log.DebugFormat("PDFResponseStream was null");
            }

            //Add Barcode
            theDoc.Rect.String = "360 680 590 725";
            //theDoc.Rect.String = "360 695 590 740";
            var bdf = BarcodeDrawFactory.Code39WithoutChecksum;
            var image = bdf.Draw(id, 45);

            theDoc.AddImageBitmap(new Bitmap(image), true);

            theDoc.Rect.String = "460 650 530 670";
            theDoc.AddText(id);

            //Add Footer
            theDoc = AddFooter(theDoc, id, lineNumber);

            var theData = theDoc.GetData();
            return theData;
        }

        private static Stream GetWebResponseStream(HttpRequestBase request, string generatePdfUrl)
        {
            try
            {
                if (request?.Url != null)
                {
                    var url = request.Url;
                    var webRequest = (HttpWebRequest)WebRequest.Create($"{url.Scheme}://{url.Host}{generatePdfUrl}");

                    webRequest.CookieContainer = new CookieContainer();

                    foreach (var key in request.Cookies.AllKeys.
                        Where(key => key.Equals(Cookies.SiteUser) || key.Equals(Cookies.ActiveCustomer) || key.Equals(Cookies.EPiServerLogin)))
                    {
                        webRequest.CookieContainer.Add(new Cookie(key, request.Cookies[key]?.Value,
                            "/", request.Url.Host));
                    }

                    var webResponse = webRequest.GetResponse();
                    var responseStream = webResponse.GetResponseStream();
                    return responseStream;
                }
                return null;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("{0} Error when getting webresponse with url {1}:", e.TargetSite, e.Message);
                return null;
            }

        }

        private string GetCustomerName(string customerNo)
        {
            var customer = _deliveryNoteRepository.GetSupplier(customerNo);

            return customer?.SupplierName;
        }

        private Doc AddFooter(Doc theDoc, string id, int lineNumber)
        {
            var theCount = theDoc.PageCount;

            var deliveryAssurance = _deliveryNoteRepository.GetDeliveryAssurance(id, lineNumber);

            for (var i = 1; i <= theCount; i++)
            {
                theDoc.FontSize = 8;

                theDoc.Rect.String = "45 30 575 110";
                theDoc.Color.String = "0 0 0";

                theDoc.Font = theDoc.AddFont("Arial");

                theDoc.AddText($"Leveransförsäkran skapad av {GetCustomerName(deliveryAssurance.SupplierNumber)}");

                theDoc.AddLine(45, 100, 575, 100);

                if (DeliveryAssurancePage != null)
                {
                    AddTextInRect("45 30 151 85", FixNewLine(DeliveryAssurancePage.Footer1), ref theDoc);
                    AddTextInRect("151 30 257 85", FixNewLine(DeliveryAssurancePage.Footer2), ref theDoc);
                    AddTextInRect("257 30 363 85", FixNewLine(DeliveryAssurancePage.Footer3), ref theDoc);
                    AddTextInRect("363 30 469 85", FixNewLine(DeliveryAssurancePage.Footer4), ref theDoc);
                    AddTextInRect("469 30 575 85", FixNewLine(DeliveryAssurancePage.Footer5), ref theDoc);
                }

            }
            return theDoc;
        }

        private string FixNewLine(string input)
        {
            return !string.IsNullOrEmpty(input) ? input.ToLineBreakString().ToHtmlString() : string.Empty;
        }

        private void AddTextInRect(string positions, string text, ref Doc theDoc)
        {
            theDoc.Rect.String = positions;
            theDoc.FontSize = 8;
            theDoc.Font = theDoc.AddFont("Arial");
            theDoc.AddHtml(text);
        }

        private static string FixRelativeLinksToAbsolute(Stream responseStream, Uri builder)
        {
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                var html = reader.ReadToEnd();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var headNode = htmlDocument.DocumentNode.SelectSingleNode("//head");
                headNode.AppendChild(HtmlNode.CreateNode("<meta http-equiv=\"content-type\" content=\"text/xhtml; charset=utf-8\" />"));

                var htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//img");
                if (htmlNodeCollection != null)
                {
                    foreach (var node in htmlNodeCollection)
                    {
                        if (!node.Attributes["src"].Value.StartsWith("http"))
                        {
                            node.Attributes["src"].Value = builder.Scheme + "://" + builder.Host + node.Attributes["src"].Value;
                        }
                    }
                }

                var linkNodeCollection = htmlDocument.DocumentNode.SelectNodes("//link");
                if (linkNodeCollection != null)
                {
                    foreach (var node in linkNodeCollection)
                    {
                        if (!node.Attributes["href"].Value.StartsWith("http"))
                        {
                            node.Attributes["href"].Value = builder.Scheme + "://" + builder.Host + node.Attributes["href"].Value;
                        }
                    }
                }

                return htmlDocument.DocumentNode.OuterHtml;
            }
        }
    }
}