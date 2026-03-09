using System;
using System.Collections.Generic;
using AppGreenRoots.Data;
using AppGreenRoots.Models;
using Microsoft.Data.Sqlite;

namespace AppGreenRoots.Repositories;

public class PassaporteRepository
{
    // Insere um registro no banco e retorna o ID gerado automaticamente
    public int InserirPassaporte(Passaporte p)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
        """
        INSERT INTO Passaporte (codigo, data_geracao, status, emissao_co2, energia_kwh, caminho_pdf, fk_id_usuario, fk_id_componente)
        VALUES (@codigo, @data, @status, @co2, @kwh, @pdf, @user, @comp);
        SELECT last_insert_rowid();
        """;

        // Parâmetros protegem contra SQL Injection
        cmd.Parameters.AddWithValue("@codigo", p.Codigo);
        cmd.Parameters.AddWithValue("@data", p.Data_Geracao.ToString("s"));
        cmd.Parameters.AddWithValue("@status", p.Status);
        cmd.Parameters.AddWithValue("@co2", p.Emissao_CO2);
        cmd.Parameters.AddWithValue("@kwh", p.Energia_Kwh);
        cmd.Parameters.AddWithValue("@pdf", (object?)p.Caminho_Pdf ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@user", (object?)p.Fk_Id_Usuario ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@comp", (object?)p.Fk_Id_Componente ?? DBNull.Value);

        return Convert.ToInt32((long)cmd.ExecuteScalar()!);
    }

    // Insere múltiplos materiais associados a um passaporte dentro de uma transação
    public void InserirMateriais(int idPassaporte, IEnumerable<PassaporteMaterial> materiais)
    {
        using var conn = Database.GetConnection();
        // O uso da Transação garante que ou tudo é salvo ou nada é salvo (Atomicidade)
        using var tx = conn.BeginTransaction();

        foreach (var m in materiais)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx; // Vincula o comando à transação

            cmd.CommandText =
            """
        INSERT INTO Passaporte_Materiais
            (id_passaporte, id_materia, nome_materia, peso_usado, fator_carbono)
        VALUES
            (@p, @idMateria, @nome, @peso, @fator);
        """;

            cmd.Parameters.AddWithValue("@p", idPassaporte);

            // Se ainda não tem cadastro real, pode ficar NULL
            cmd.Parameters.AddWithValue("@idMateria", m.Id_Materia == 0 ? DBNull.Value : m.Id_Materia);

            cmd.Parameters.AddWithValue("@nome", m.NomeMateria);
            cmd.Parameters.AddWithValue("@peso", m.PesoUsado);
            cmd.Parameters.AddWithValue("@fator", m.FatorCarbono);

            cmd.ExecuteNonQuery();
        }

        tx.Commit();
    }
    
    // Lista todos os passaportes, convertendo as colunas do SQLite para objetos 
    public List<Passaporte> ListarPassaportes()
    {
        var lista = new List<Passaporte>();

        using var conn = Database.GetConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
        """
        SELECT id_passaporte, codigo, data_geracao, status, emissao_co2, energia_kwh, caminho_pdf, fk_id_usuario, fk_id_componente
        FROM Passaporte
        ORDER BY id_passaporte DESC;
        """;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Passaporte
            {
                Id_Passaporte = r.GetInt32(0),
                Codigo = r.GetString(1),
                // Tratamento de segurança ao converter string do banco para DateTime
                Data_Geracao = DateTime.TryParse(r.GetString(2), out var dt) ? dt : DateTime.Now,
                Status = r.GetString(3),
                Emissao_CO2 = r.GetDouble(4),
                Energia_Kwh = r.GetDouble(5),
                Caminho_Pdf = r.IsDBNull(6) ? null : r.GetString(6),
                Fk_Id_Usuario = r.IsDBNull(7) ? null : r.GetInt32(7),
                Fk_Id_Componente = r.IsDBNull(8) ? null : r.GetInt32(8)
            });
        }

        return lista;
    }
}