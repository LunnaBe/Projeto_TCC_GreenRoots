using System;

namespace AppGreenRoots.Models;

public class Passaporte
{
    public int Id_Passaporte { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public DateTime Data_Geracao { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pendente";
    public double Emissao_CO2 { get; set; }
    public double Energia_Kwh { get; set; }
    public string? Caminho_Pdf { get; set; }

    public int? Fk_Id_Usuario { get; set; }
    public int? Fk_Id_Componente { get; set; }
}