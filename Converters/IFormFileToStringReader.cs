using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using krzysztofb.CustomExceptions;
using krzysztofb.Models.DTO;
using krzysztofb.Services.Interfaces;
using System.Globalization;

namespace krzysztofb.Converters
{
    public class IFormFileToStringReader :
        IPdfToDatabaseRead<WniosekDTO>
    {
        public static StringReader LoadPdf(IFormFile file)
        {
            StringReader sr;
            WniosekDTO wniosek = new WniosekDTO();
            FileInfo fileInfo;
            using (MemoryStream _memoryStream = new MemoryStream())
            {
                file.CopyToAsync(_memoryStream);
                //convert file to System.IO.FIleInfo
                fileInfo = new FileInfo(file.FileName);
            }
            using (PdfReader reader = new PdfReader(fileInfo))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    string text = PdfTextExtractor.GetTextFromPage(doc.GetPage(1));
                    sr = new StringReader(text);

                }
                return sr;
            }
        }
        public static string[] GetWniosekSender(StringReader document)
        {
            string[] imieNazwisko = new string[2];
            string line = "";
            string previousLine = "";
            while ((line = document.ReadLine()) != null)
            {
                if (line.Contains("(nazwisko i imię)"))
                {
                    imieNazwisko = previousLine.Split(" Nr ewid.")[0].Split(" ");
                }
                else
                {
                    previousLine = line;
                }
            }
            return imieNazwisko;
        }
        public static WniosekDTO GetPdfData(StringReader document)
        {
            WniosekDTO wniosekUrlop = new WniosekDTO();
            string[] imieNazwisko = new string[2];
            string line = "";
            string previousLine = "";
            while ((line = document.ReadLine()) != null)
            {
                if (line.Contains("(nazwisko i imię)"))
                {
                    wniosekUrlop.Nr_ewidencyjny = Int32.Parse(previousLine.Split(" Nr ewid.")[1]);
                    imieNazwisko = previousLine.Split(" Nr ewid.")[0].Split(" ");
                }
                else if (line.Contains("w ilości"))
                {
                    wniosekUrlop.Ilosc_dni = Int32.Parse(line.Split("w ilości ")[1].Split(" dni.")[0]);
                }
                else if (line.Contains("Elbląg, dnia "))
                {
                    wniosekUrlop.Data_wypelnienia = DateOnly.FromDateTime(DateTime.Parse(line.Split("Elbląg, dnia ")[1]));
                }
                else if (line.Contains("Od dnia"))
                {
                    wniosekUrlop.Data_rozpoczecia = DateOnly.FromDateTime(DateTime.ParseExact(line.Split("Od dnia ")[1].Split(" Do dnia ")[0].Replace('.', '/').Trim(), "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal));
                    wniosekUrlop.Data_zakonczenia = DateOnly.FromDateTime(DateTime.ParseExact(line.Split("Do dnia ")[1].Replace('.', '/').Trim(), "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal));
                }
                else
                {
                    previousLine = line;
                }
            }
            if (wniosekUrlop.Data_zakonczenia.Value.DayNumber - wniosekUrlop.Data_rozpoczecia.Value.DayNumber != wniosekUrlop.Ilosc_dni)
            {
                //throw new PdfToDatabaseException("Ilość dni nie zgadza się z podanymi datami");
            }
            if (wniosekUrlop.Data_rozpoczecia.Value.DayNumber - wniosekUrlop.Data_wypelnienia.Value.DayNumber < 1)
            {
                throw new PdfToDatabaseException("Data wypełnienia nie zgadza się z podaną datą rozpoczęcia");
            }
            return wniosekUrlop;
        }
    }
}
