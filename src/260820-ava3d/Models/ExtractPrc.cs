using System;
using System.IO;
using iText.Kernel.Pdf;

namespace _260820_ava3d.Models
{
  public class ExtractPrc
  {
    /// <summary>
    /// Extracts the first embedded PRC model from a PDF file.
    /// Returns true when a PRC model is found and written successfully.
    /// </summary>
    public static bool ExtractFile(string inputPdf, string outputPrc)
    {
      if (string.IsNullOrWhiteSpace(inputPdf))
        throw new ArgumentException(
            "Input PDF path is required.", nameof(inputPdf));

      if (string.IsNullOrWhiteSpace(outputPrc))
        throw new ArgumentException(
            "Output PRC path is required.", nameof(outputPrc));

      if (!File.Exists(inputPdf))
        throw new FileNotFoundException(
            "The input PDF file was not found.", inputPdf);

      using (var reader = new PdfReader(inputPdf))
      using (var pdf = new PdfDocument(reader))
      {
        for (int pageNumber = 1;
             pageNumber <= pdf.GetNumberOfPages();
             pageNumber++)
        {
          PdfDictionary pageDictionary =
              pdf.GetPage(pageNumber).GetPdfObject();

          PdfArray annotations =
              pageDictionary.GetAsArray(PdfName.Annots);

          if (annotations == null)
            continue;

          for (int i = 0; i < annotations.Size(); i++)
          {
            PdfDictionary annotation =
                annotations.GetAsDictionary(i);

            if (annotation == null)
              continue;

            PdfName annotationSubtype =
                annotation.GetAsName(PdfName.Subtype);

            if (!PdfName._3D.Equals(annotationSubtype))
              continue;

            Console.WriteLine(
                $"Found 3D annotation on page {pageNumber}");

            PdfStream model =
                annotation.GetAsStream(PdfName._3DD);

            if (model == null)
            {
              Console.WriteLine(
                  "The annotation has no /3DD object.");
              continue;
            }

            PdfName modelSubtype =
                model.GetAsName(PdfName.Subtype);

            Console.WriteLine(
                $"Embedded model type: {modelSubtype}");

            if (!new PdfName("PRC").Equals(modelSubtype))
            {
              Console.WriteLine(
                  "The embedded 3D model is not PRC.");
              continue;
            }

            // GetBytes() returns the decoded PDF stream data.
            byte[] data = model.GetBytes();

            File.WriteAllBytes(outputPrc, data);

            Console.WriteLine(
                $"Extracted {data.Length} bytes.");

            Console.WriteLine(
                $"Saved to: {outputPrc}");

            return true;
          }
        }
      }

      Console.WriteLine("No PRC 3D stream was found.");
      return false;
    }
  }
}
