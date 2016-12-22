using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace DH.Helpdesk.EForm.FormLib
{
    public class FormLibPdfContentResult: FileContentResult
    {
        public FormLibPdfContentResult(byte[] data) : base(data, "application/pdf") { }

        public FormLibPdfContentResult(byte[] data, string fileName)
            : this(data)
        {
            if(fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.FileDownloadName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ClearHeaders();

            base.ExecuteResult(context);

            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Private);
        }
    }

    public class FormlibPdfResult : ViewResult
    {
        private readonly ViewResult _header;
        private readonly ViewResult _footer;

        public FormlibPdfResult(
            object model, 
            string name,
            ViewResult header,
            ViewResult footer)
        {
            _header = header;
            _footer = footer;
            ViewData = new ViewDataDictionary(model);
            ViewName = name;
        }

        public FormlibPdfResult(object model, string name) : this(model, name, null, null) { }

        protected override ViewEngineResult FindView(ControllerContext context)
        {
            ViewEngineResult result = base.FindView(context);
            if (result.View == null)
            {
                return result;
            }

            ViewEngineResult headerResult = null;
            if (_header != null)
            {
                headerResult = _header.ViewEngineCollection.FindView(context, _header.ViewName, _header.MasterName);
            }

            ViewEngineResult footerResult = null;
            if (_footer != null)
            {
                footerResult = _footer.ViewEngineCollection.FindView(context, _footer.ViewName, _footer.MasterName);
            }

            var view = new FormlibPdfView(result, headerResult, footerResult);
            return new ViewEngineResult(view, view);
        }
    }

    public class FormlibPdfView : IView, IViewEngine
    {
        private readonly ViewEngineResult _result;
        private readonly ViewEngineResult _header;
        private readonly ViewEngineResult _footer;

        public FormlibPdfView(ViewEngineResult result, ViewEngineResult header, ViewEngineResult footer)
        {
            _result = result;
            _header = header;
            _footer = footer;
        }

        public FormlibPdfView(ViewEngineResult result)
            : this(result, null, null)
        {

        }

        //public void Render(ViewContext viewContext, TextWriter writer)
        //{
        //    XmlParser parser;
        //    string source = RenderView(viewContext, _result.View);
        //    using(var reader = GetXmlReader(source))
        //    {
        //        while(reader.Read() && (reader.NodeType != XmlNodeType.Element))
        //        {
        //        }
        //        if((reader.NodeType == XmlNodeType.Element) && (reader.Name == "itext"))
        //        {
        //            parser = new XmlParser();
        //        }
        //        else
        //        {
        //            parser = new HtmlParser();
        //        }
        //    }

        //    var document = new Document();
        //    document.Open();
        //    PdfWriter instance = PdfWriter.GetInstance(document, viewContext.HttpContext.Response.OutputStream);

        //    string headerString = null;
        //    if(this._header != null)
        //    {
        //        headerString = RenderView(viewContext, _header.View);
        //    }

        //    string footerString = null;
        //    if(this._footer != null)
        //    {
        //        footerString = RenderView(viewContext, _footer.View);
        //    }

        //    instance.PageEvent = new PrintPdfEvents(headerString, footerString);
        //    instance.CloseStream = false;
        //    viewContext.HttpContext.Response.ContentType = "application/pdf";
        //    using(var reader2 = GetXmlReader(source))
        //    {
        //        parser.Go(document, reader2);
        //    }
        //    instance.Close();
        //}

        //public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        //{
        //    throw new NotImplementedException();
        //}

        //public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ReleaseView(ControllerContext controllerContext, IView view)
        //{
        //    this._result.ViewEngine.ReleaseView(controllerContext, _result.View);
        //}

        //private XmlTextReader GetXmlReader(string source)
        //{
        //    return new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(source))) { WhitespaceHandling = WhitespaceHandling.None };
        //}

        //private string RenderView(ViewContext viewContext, IView view)
        //{
        //    if(view == null)
        //    {
        //        return null;
        //    }

        //    var sb = new StringBuilder();
        //    TextWriter writer = new StringWriter(sb);
        //    view.Render(viewContext, writer);
        //    return sb.ToString();
        //}

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            throw new NotImplementedException();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            throw new NotImplementedException();
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }
    }

    public class PrintPdfEvents : PdfPageEventHelper
    {
        private readonly string _header;
        private readonly string _footer;
        private List<IElement> _headerElements;
        private List<IElement> _footerElements;

        public PrintPdfEvents(string header, string footer)
        {
            _header = header;
            _footer = footer;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            if(!string.IsNullOrEmpty(_header))
            {
                using(var sr = new StringReader(_header))
                {
                    _headerElements = HTMLWorker.ParseToList(sr, new StyleSheet());
                }
            }
            if(!string.IsNullOrEmpty(_footer))
            {
                using(var sr = new StringReader(_footer))
                {
                    _footerElements = HTMLWorker.ParseToList(sr, new StyleSheet());
                }
            }
            base.OnOpenDocument(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            if(_headerElements != null)
            {
                foreach(var element in _headerElements)
                {
                    document.Add((IElement)element);
                }
            }
            base.OnStartPage(writer, document);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if(_footerElements != null)
            {
                foreach(var element in _footerElements)
                {
                    document.Add((IElement)element);
                }
            }
            base.OnEndPage(writer, document);
        }
    }
}