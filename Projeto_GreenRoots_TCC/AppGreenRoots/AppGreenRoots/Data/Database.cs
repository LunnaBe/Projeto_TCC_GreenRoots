using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace AppGreenRoots.Data;

public static class Database
{
    private static readonly string dbPath =
     Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Passaporte_Digital.db");
    public static SqliteConnection GetConnection()
    {
        var conn = new SqliteConnection($"Data Source={dbPath}");
        conn.Open();
        return conn;
    }

    public static void Initialize()
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText =
        """
        PRAGMA foreign_keys = ON;

        CREATE TABLE IF NOT EXISTS Usuario (
            id_usuario INTEGER PRIMARY KEY AUTOINCREMENT,
            nome TEXT NOT NULL,
            email TEXT NOT NULL UNIQUE,
            senha TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS MateriaPrima (
            id_materia INTEGER PRIMARY KEY AUTOINCREMENT,
            nome TEXT NOT NULL,
            tipo TEXT,
            fator_carbono REAL NOT NULL DEFAULT 0,
            fk_id_fornecedor INTEGER
        );

        CREATE TABLE IF NOT EXISTS Passaporte (
            id_passaporte INTEGER PRIMARY KEY AUTOINCREMENT,
            codigo TEXT NOT NULL UNIQUE,
            data_geracao TEXT NOT NULL,
            status TEXT NOT NULL,
            emissao_co2 REAL NOT NULL DEFAULT 0,
            energia_kwh REAL NOT NULL DEFAULT 0,
            caminho_pdf TEXT,
            fk_id_usuario INTEGER,
            fk_id_componente INTEGER,

            FOREIGN KEY (fk_id_usuario) REFERENCES Usuario(id_usuario)
        );

        CREATE TABLE IF NOT EXISTS Passaporte_Materiais (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            id_passaporte INTEGER NOT NULL,
            id_materia INTEGER,
            nome_materia TEXT NOT NULL,
            peso_usado REAL NOT NULL,
            fator_carbono REAL NOT NULL,

            FOREIGN KEY (id_passaporte) REFERENCES Passaporte(id_passaporte) ON DELETE CASCADE,
            FOREIGN KEY (id_materia) REFERENCES MateriaPrima(id_materia)
        );

        CREATE INDEX IF NOT EXISTS IX_Passaporte_Data ON Passaporte(data_geracao);
        CREATE INDEX IF NOT EXISTS IX_PM_Passaporte ON Passaporte_Materiais(id_passaporte);
        """;

        cmd.ExecuteNonQuery();
    }
}