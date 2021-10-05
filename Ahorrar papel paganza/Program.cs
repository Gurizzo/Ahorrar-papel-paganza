using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MergePDF
{
    class Program
    {
        public static DirectoryInfo _directorioEntrada = new DirectoryInfo(@"G:\Mi unidad\Facturas");

        static void Main(string[] args)
        {

            List<string> rutas = new List<string>();
            foreach (FileInfo file in _directorioEntrada.GetFiles())
            {
                rutas.Add(file.FullName);
            }
            MergePDF(rutas);


        }

        private static void MergePDF(List<string> files)
        {

            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = @"G:\Mi unidad\Facturas\" + DateTime.Now.AddMonths(-1).ToString("yyyy_MM")+ "\\ComprobantesMes_" + DateTime.Now.AddMonths(-1).Month + ".pdf";

            if (File.Exists(outputPdfPath))
            {
                string pathSalida = @"G:\Mi unidad\Facturas\Respaldo.pdf";
                //@"C:\Users\guriz\Desktop\Entrada"
                File.Copy(outputPdfPath, pathSalida);

                List<String> aux = new List<string>();
                aux.Add(pathSalida);
                aux.AddRange(files);
                files = aux;
            }
            string carpeta = @"G:\Mi unidad\Facturas\" + DateTime.Now.AddMonths(-1).ToString("yyyy_MM");

            if (!Directory.Exists(carpeta)){
                Directory.CreateDirectory(carpeta);
            }
            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new FileStream(outputPdfPath, FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            foreach (var file in files)
            {
                int pages = TotalPageCount(file);

                reader = new PdfReader(file);

                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();

            }



            //save the output file  
            sourceDocument.Close();

            foreach (FileInfo file in _directorioEntrada.GetFiles())
            {
                file.Delete();
            }

        }

        private static void MergePDF(string File1, string File2)
        {
            string[] fileArray = new string[3];
            fileArray[0] = File1;
            fileArray[1] = File2;

            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = @"D:/Prueba.pdf";

            
            

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new FileStream(outputPdfPath, FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length - 1; f++)
            {
                int pages = TotalPageCount(fileArray[f]);

                reader = new PdfReader(fileArray[f]);




                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();
        }

        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
    }
}