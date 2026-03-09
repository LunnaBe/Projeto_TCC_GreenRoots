using System;
using System.Collections.Generic;
using System.IO;
using AppGreenRoots.Models;
using AppGreenRoots.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AppGreenRoots.Services;

public class PassaporteService
{
    private readonly PassaporteRepository _repo = new();

    // Define o fator de conversão de energia para carbono
    public double FatorCO2PorKwh { get; set; } = 0.0;

    // Método que coordena o fluxo de criação do Passaporte Digital
    public (int idPassaporte, string caminhoPdf) GerarPassaporte(
        int? idUsuario,
        int? idComponente,
        double energiaKwh,
        List<PassaporteMaterial> materiais)
    {
        // Garante que um passaporte tenha pelo menos um material
        if (materiais.Count == 0)
            throw new ArgumentException("Adicione pelo menos 1 matéria-prima.");

        var co2Materiais = 0.0;

        // Cálculo da soma das emissões de materiais + emissão da energia consumida
        foreach (var m in materiais)
            co2Materiais += m.EmissaoMaterial;

        var co2Energia = energiaKwh * FatorCO2PorKwh;
        var co2Total = co2Materiais + co2Energia;

        // Gera um código único para o passaporte
        var codigo = $"GR-{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

        // Diretório para armazenar os PDFs gerados
        var pasta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "AppGreenRoots_Passaportes");

        Directory.CreateDirectory(pasta);

        var pdfPath = Path.Combine(pasta, $"{codigo}.pdf");

        // Gera um arquivo PDF usando iTextSharp ou similares
        GerarPdf(pdfPath, codigo, energiaKwh, co2Total, materiais);

        // Prepara o objeto para ser salvo no banco de dados
        var passaporte = new Passaporte
        {
            Codigo = codigo,
            Data_Geracao = DateTime.Now,
            Status = "Pendente",
            Emissao_CO2 = co2Total,
            Energia_Kwh = energiaKwh,
            Caminho_Pdf = pdfPath,
            Fk_Id_Usuario = idUsuario,
            Fk_Id_Componente = idComponente
        };

        // Salva o Passaporte e vincula os materiais
        var id = _repo.InserirPassaporte(passaporte);
        _repo.InserirMateriais(id, materiais);

        return (id, pdfPath);
    }

    // Método de construção do layout do PDF
    private static void GerarPdf(
        string path,
        string codigo,
        double energiaKwh,
        double co2Total,
        List<PassaporteMaterial> materiais)
    {
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);

        var doc = new Document(PageSize.A4, 40, 40, 40, 40);

        var writer = PdfWriter.GetInstance(doc, fs);

        doc.Open();

        var title = new Paragraph(
            "GREEN ROOTS - PASSAPORTE DIGITAL",
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));

        doc.Add(title);

        doc.Add(new Paragraph($"Código: {codigo}"));
        doc.Add(new Paragraph($"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}"));
        doc.Add(new Paragraph(" "));

        doc.Add(new Paragraph($"Energia (kWh): {energiaKwh:0.###}"));
        doc.Add(new Paragraph($"Emissão Total (kgCO2): {co2Total:0.###}"));

        doc.Add(new Paragraph(" "));

        doc.Add(new Paragraph(
            "Materiais Utilizados:",
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));

        doc.Add(new Paragraph(" "));

        var table = new PdfPTable(4)
        {
            WidthPercentage = 100
        };

        table.SetWidths(new float[] { 45, 18, 18, 19 });

        AddHeader(table, "Matéria-prima");
        AddHeader(table, "Peso");
        AddHeader(table, "Fator CO2");
        AddHeader(table, "CO2 Material");

        foreach (var m in materiais)
        {
            table.AddCell(m.NomeMateria);
            table.AddCell($"{m.PesoUsado:0.###}");
            table.AddCell($"{m.FatorCarbono:0.###}");
            table.AddCell($"{m.EmissaoMaterial:0.###}");
        }

        doc.Add(table);

        doc.Add(new Paragraph(" "));
        doc.Add(new Paragraph("Assinatura/Auditoria: ____________________________"));

        doc.Close();
        writer.Close();
    }

    private static void AddHeader(PdfPTable t, string text)
    {
        var cell = new PdfPCell(
            new Phrase(text, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)))
        {
            BackgroundColor = new BaseColor(230, 230, 230)
        };

        t.AddCell(cell);
    }
}