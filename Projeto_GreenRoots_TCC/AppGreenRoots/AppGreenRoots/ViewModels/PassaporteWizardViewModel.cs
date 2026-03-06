using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppGreenRoots.Helpers;
using AppGreenRoots.Models;
using AppGreenRoots.Services;

namespace AppGreenRoots.ViewModels;

public class PassaporteWizardViewModel : INotifyPropertyChanged
{
    private readonly PassaporteService _service = new();
    private readonly Usuario? _usuario;
    private readonly Action _onVoltar;

    public ObservableCollection<PassaporteMaterial> Materiais { get; } = new();

    private PassaporteMaterial? _selectedMaterial;
    public PassaporteMaterial? SelectedMaterial
    {
        get => _selectedMaterial;
        set { _selectedMaterial = value; OnPropertyChanged(); }
    }

    private string _energiaKwh = "0";
    public string EnergiaKwh
    {
        get => _energiaKwh;
        set { _energiaKwh = value; OnPropertyChanged(); }
    }

    private string _materialNome = "";
    public string MaterialNome
    {
        get => _materialNome;
        set { _materialNome = value; OnPropertyChanged(); }
    }

    private string _materialPeso = "";
    public string MaterialPeso
    {
        get => _materialPeso;
        set { _materialPeso = value; OnPropertyChanged(); }
    }

    private string _materialFator = "";
    public string MaterialFator
    {
        get => _materialFator;
        set { _materialFator = value; OnPropertyChanged(); }
    }

    private string _mensagem = "";
    public string Mensagem
    {
        get => _mensagem;
        set { _mensagem = value; OnPropertyChanged(); }
    }

    public ICommand AddMaterialCommand { get; }
    public ICommand RemoverMaterialCommand { get; }
    public ICommand GerarCommand { get; }
    public ICommand VoltarCommand { get; }

    public PassaporteWizardViewModel(Usuario? usuario, Action onVoltar)
    {
        _usuario = usuario;
        _onVoltar = onVoltar;

        AddMaterialCommand = new RelayCommand(_ => AddMaterial());
        RemoverMaterialCommand = new RelayCommand(_ => RemoverSelecionado(), _ => SelectedMaterial != null);
        GerarCommand = new RelayCommand(_ => Gerar());
        VoltarCommand = new RelayCommand(_ => _onVoltar());
    }

    // construtor safe
    public PassaporteWizardViewModel() : this(null, () => { }) { }

    private void AddMaterial()
    {
        Mensagem = "";

        if (string.IsNullOrWhiteSpace(MaterialNome))
        {
            Mensagem = "Informe o nome do material.";
            return;
        }

        if (!double.TryParse(MaterialPeso.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var peso) || peso <= 0)
        {
            Mensagem = "Peso inválido.";
            return;
        }

        if (!double.TryParse(MaterialFator.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var fator) || fator < 0)
        {
            Mensagem = "Fator CO₂ inválido.";
            return;
        }

        Materiais.Add(new PassaporteMaterial
        {
            Id_Materia = 0, // quando você tiver cadastro real de MateriaPrima, você liga isso
            NomeMateria = MaterialNome.Trim(),
            PesoUsado = peso,
            FatorCarbono = fator
        });

        MaterialNome = "";
        MaterialPeso = "";
        MaterialFator = "";
        Mensagem = "Material adicionado.";
    }

    private void RemoverSelecionado()
    {
        if (SelectedMaterial != null)
        {
            Materiais.Remove(SelectedMaterial);
            SelectedMaterial = null;
            Mensagem = "Material removido.";
        }
    }

    private void Gerar()
    {
        Mensagem = "";

        if (Materiais.Count == 0)
        {
            Mensagem = "Adicione pelo menos 1 material.";
            return;
        }

        if (!double.TryParse(EnergiaKwh.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var kwh) || kwh < 0)
        {
            Mensagem = "Energia inválida.";
            return;
        }

        try
        {
            var (id, pdf) = _service.GerarPassaporte(
                idUsuario: _usuario?.Id_Usuario,
                idComponente: null,
                energiaKwh: kwh,
                materiais: Materiais.ToList()
            );

            Mensagem = $"✅ Passaporte gerado (ID {id}). PDF: {pdf}";
            Process.Start(new ProcessStartInfo(pdf) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Mensagem = ex.Message;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}