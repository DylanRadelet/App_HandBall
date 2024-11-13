using HtmlAgilityPack;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

class Program
{
    static void Main(string[] args)
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--headless");

        string defaultPathFile = "defaultPath.txt";
        string defaultDirectory = Directory.GetCurrentDirectory();
        string defaultPath = defaultDirectory;

        if (File.Exists(defaultPathFile))
        {
            defaultPath = File.ReadAllText(defaultPathFile);
        }

        Console.WriteLine("Entrez l'url du match : ");
        string urlMatch = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Entrez le nom du fichier PDF (sans extension) : ");
        string fileName = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine($"Entrez le chemin d'enregistrement du fichier (par défaut : {defaultPath}) : ");
        string userPath = Console.ReadLine();
        Console.WriteLine();

        if (!string.IsNullOrWhiteSpace(userPath))
        {
            defaultPath = userPath;
            File.WriteAllText(defaultPathFile, defaultPath);
        }

        string filePath = Path.Combine(defaultPath, $"{fileName}.pdf");

        int heimScore = 0;
        int gastScore = 0;

        int manualRows = 0;

        PdfPTable table = new PdfPTable(5);
        float[] columnWidths = { 1, 1, 1, 3, 3 };
        table.SetWidths(columnWidths);
        table.WidthPercentage = 100f;

        Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
        table.AddCell(new PdfPCell(new Phrase("Score", boldFont)));
        table.AddCell(new PdfPCell(new Phrase("Team", boldFont)));
        table.AddCell(new PdfPCell(new Phrase("Time", boldFont)));
        table.AddCell(new PdfPCell(new Phrase("Event", boldFont)));
        table.AddCell(new PdfPCell(new Phrase("Person", boldFont)));

        using (IWebDriver driver = new ChromeDriver(options))
        {
            driver.Navigate().GoToUrl(urlMatch);
            System.Threading.Thread.Sleep(3000);

            string html = driver.PageSource;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var rows = doc.DocumentNode.SelectNodes("//tr[@eventline]");

            if (rows != null && rows.Count > 0)
            {
                Console.WriteLine("Informations extraites :");
                foreach (var row in rows)
                {
                    AddRowToTable(row, table, ref heimScore, ref gastScore);
                }
            }
            else
            {
                Console.WriteLine("Aucune ligne trouvée avec l'attribut 'eventline'.");
            }
        }

        Console.Clear();
        Console.WriteLine("\nAperçu du contenu du fichier PDF :");
        Console.WriteLine();
        DisplayTablePreview(table);

        Console.WriteLine();
        Console.WriteLine("Souhaitez-vous ajouter des lignes manuellement ? (O/N) : ");
        string userChoice = Console.ReadLine().Trim().ToUpper();

        if (userChoice == "O")
        {
            manualRows = GetManualRowCount();
        }

        for (int i = 0; i < manualRows; i++)
        {
            AddManualRow(table, ref heimScore, ref gastScore);
            Console.WriteLine();
        }

        Document pdfDoc = new Document(PageSize.A4);
        PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
        pdfDoc.Open();
        pdfDoc.Add(new Paragraph("Tableau des scores", boldFont));
        pdfDoc.Add(new Paragraph("\n"));
        pdfDoc.Add(table);
        pdfDoc.Add(new Paragraph($"\nScore final : Heim {heimScore} - Gast {gastScore}", boldFont));
        pdfDoc.Close();

        Console.WriteLine($"Le fichier PDF a été généré avec succès sous le nom '{filePath}'.");
    }

    private static void DisplayTablePreview(PdfPTable table)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            foreach (PdfPCell cell in row.GetCells())
            {
                Console.Write($"{cell.Phrase.Content} \t");
            }
            Console.WriteLine();
        }
    }

    private static int GetManualRowCount()
    {
        Console.WriteLine("Combien de lignes voulez-vous ajouter manuellement ?");
        if (int.TryParse(Console.ReadLine(), out int count))
        {
            return count;
        }
        return 0;
    }

    private static void AddManualRow(PdfPTable table, ref int heimScore, ref int gastScore)
    {
        string team = GetValidInput("Entrez l'équipe (Heim ou Gast) : ", new[] { "Heim", "Gast" });
        string time = GetValidInput("Entrez le temps (format libre) : ");
        string eventDetail = GetValidInput("Entrez l'événement (Goal ou 7m with Goal) : ", new[] { "Goal", "7m with Goal" });
        string person = GetValidInput("Entrez le nom de la personne : ");

        if (eventDetail == "Goal" || eventDetail.Contains("7m with Goal"))
        {
            if (team == "Heim")
            {
                heimScore++;
            }
            else if (team == "Gast")
            {
                gastScore++;
            }
        }

        string score = $"{heimScore}-{gastScore}";

        table.AddCell(score);
        table.AddCell(team);
        table.AddCell(time);
        table.AddCell(eventDetail);
        table.AddCell(person);
    }

    private static void AddRowToTable(HtmlNode row, PdfPTable table, ref int heimScore, ref int gastScore)
    {
        var tdElements = row.SelectNodes(".//td");
        if (tdElements == null || tdElements.Count == 0) return;

        string first = tdElements.ElementAtOrDefault(0)?.InnerText.Trim() ?? "";
        string second = tdElements.ElementAtOrDefault(1)?.InnerText.Trim() ?? "";
        string third = tdElements.ElementAtOrDefault(2)?.InnerText.Trim() ?? "";
        string fourth = tdElements.ElementAtOrDefault(3)?.InnerText.Trim() ?? "";

        if (first == "&nbsp;" && second == "&nbsp;" && third == "&nbsp;" && fourth == "&nbsp;") return;

        string team = first;
        if (third == "Goal" || third.Contains("7m with Goal"))
        {
            if (team == "Heim")
            {
                heimScore++;
            }
            else if (team == "Gast")
            {
                gastScore++;
            }
        }

        string score = $"{heimScore}-{gastScore}";
        string time = second;
        string eventDetail = third;
        string person = fourth;

        table.AddCell(score);
        table.AddCell(team);
        table.AddCell(time);
        table.AddCell(eventDetail);
        table.AddCell(person);
    }

    private static string GetValidInput(string prompt, string[] validOptions = null)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim();

            if (validOptions == null || validOptions.Contains(input))
            {
                return input;
            }

            Console.WriteLine("Valeur invalide. Veuillez réessayer.");
        }
    }
}