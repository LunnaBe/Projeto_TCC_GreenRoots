namespace AppGreenRoots.Models;

public class PassaporteMaterial
{
    public int Id_Materia { get; set; }
    public string NomeMateria { get; set; } = string.Empty;

    public double PesoUsado { get; set; }
    public double FatorCarbono { get; set; }

    public double EmissaoMaterial => PesoUsado * FatorCarbono;
}