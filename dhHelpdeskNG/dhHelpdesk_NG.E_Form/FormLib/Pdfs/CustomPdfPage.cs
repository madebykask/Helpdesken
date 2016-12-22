using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.EForm.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using DH.Helpdesk.EForm.FormLib.Models;
using System.Text;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Contrete;

namespace DH.Helpdesk.EForm.FormLib.Pdfs
{
    public class CustomPdfPage : iTextSharp.text.pdf.PdfPageEventHelper
    {
        public Contract Contract { get; set; }
        public string Language { get; set; }
        public int State { get; set; }

        public DocumentTextRepository documentTextRepository = new DocumentTextRepository(System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString);


      
        /// <summary>ErrorHandling, check if Department is empty.
        /// </summary> 
        public static string IsEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                return EmptyLabel(name);
            }

            return value;
        }

        public static string EmptyLabel(string value)
        {
            
            return "[" + value.ToString().ToUpper() + "]";
        }

        /// <summary>Draft text Should be implemented in status 20. Taken away after status 30.
        /// </summary> 
        public static bool ShowDraft(int stateSecondaryId)
        {
            if (stateSecondaryId >= 20 && stateSecondaryId < 40)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Get Date in Words instead of ISO-format, with check for empty data
        /// </summary> 
        public static string GetDateInWords(string name, FormModel model)
        {
            string Date = model.GetDocumentAnswer(name);

            if (!string.IsNullOrEmpty(Date))
            {
                string s = Date.Replace(".", "-");
                try
                {
                DateTime dt = DateTime.ParseExact(s, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                return string.Format(new System.Globalization.CultureInfo("en-GB"), "{0:d MMMMMM yyyy}", dt);
                }
                //Fel på datumet skicka ut label värdet istället
                catch (Exception)
                {
                    return Date; //model.GetDocumentLabelEmpty(name);
                    //return model.LabelEmpty( IsEmpty(name, model);
                }
            }
            else
            {
                return model.GetDocumentLabelEmpty(name);
            }
        }

        /// <summary>Get Time in AM/PM format, with check for empty data
        /// </summary> 
        public static string GetAmPm(string name, FormModel model)
        {
         
            //return IsEmpty(name, model);
            return model.GetDocumentAnswer(name);

            ////try
            ////{
            ////    return string.Format(new System.Globalization.CultureInfo("en-GB"), "{0:h:mm tt}", DateTime.ParseExact(Time, "HH:mm", CultureInfo.InvariantCulture));
            ////}
            ////catch (Exception)
            ////{

            //return time;
            //// }

        }

        /// <summary>Get LengthOfInductionInDaysText, with check for empty data
        /// </summary> 
        public static string GetLengthOfInductionInDaysText(string name, FormModel model)
        {
            string LengthOfInductionInDays = model.GetDocumentAnswer(name);

            if (LengthOfInductionInDays.ToLower() == "1 day".ToLower())
            {
                return  "day";
            }
            else
            {
                return LengthOfInductionInDays;
            }

        }
       
        protected Font Header
        {
            get
            {
                return FontFactory.GetFont("Verdana", 12, Font.NORMAL, BaseColor.GRAY);
            }
        }

        protected Font HeaderSmall
        {
            get
            {
                return FontFactory.GetFont("Verdana", 9, Font.NORMAL, BaseColor.GRAY);
            }
        }

        protected Font Footer
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.NORMAL, BaseColor.GRAY);

            }
        }

        protected Font PL_Footer(float _size, int _style , BaseColor _baseColor)
        {
            return FontFactory.GetFont("Verdana", BaseFont.CP1257, _size, _style, _baseColor);
        }

        protected Font FooterSmall
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.BOLD, BaseColor.GRAY);
            }
        }

        protected Font CustomFooter
        {
            get
            {
                return FontFactory.GetFont("Verdana", BaseFont.CP1257, 9, Font.NORMAL, BaseColor.BLACK);

            }
        }
        protected Font FooterUnderline
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.UNDERLINE, BaseColor.BLUE);
            }
        }
        protected Font FooterLinkUnderline
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.UNDERLINE, BaseColor.GRAY);
            }
        }

        protected Font SignatureFont(int fontSize)
        {
                return FontFactory.GetFont("Verdana", BaseFont.CP1252, fontSize, Font.NORMAL, BaseColor.BLACK);
        }

        protected Font PageNumberFont
        {
            get
            {
                return FontFactory.GetFont("Verdana", BaseFont.CP1252, 9, Font.NORMAL, BaseColor.BLACK);
            }
        }

        protected Font SignatureFontBold(int fontSize)
        {

                return FontFactory.GetFont("Verdana", BaseFont.CP1252, fontSize, Font.BOLD, BaseColor.BLACK);

        }

        protected Font SignatureFontBoldRed
        {
            get
            {
                return FontFactory.GetFont("Verdana", BaseFont.CP1252, 11, Font.BOLD, BaseColor.RED);
            }
        }


        protected Font Verdana(float _size, BaseColor _baseColor)
        {
            // string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Verdana.TTF");
            return FontFactory.GetFont("Verdana", BaseFont.CP1252, _size, Font.NORMAL, _baseColor);
        }


        protected Font Malgun(float _size, BaseColor _baseColor)
        {
            return FontFactory.GetFont("Malgun", BaseFont.IDENTITY_H, _size, Font.NORMAL, _baseColor);
        }


        protected Font BarCodeFont
        {         
            get
            {
                return FontFactory.GetFont("IDAutomationHC39M", 11, BaseColor.BLACK);
            }
        }


        public static string IsEmpty(Department department, string p)
        {
            throw new NotImplementedException();
        }
    }
}