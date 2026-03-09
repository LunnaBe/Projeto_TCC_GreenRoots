using Microsoft.Data.Sqlite;
using AppGreenRoots.Models;
using AppGreenRoots.Data;

namespace AppGreenRoots.Services;

public class UsuarioService
{
    // Registra um novo usuário no banco
    public bool Cadastrar(string nome, string email, string senha)
    {
        using var conn = Database.GetConnection();

        // Verifica se o email já existe para evitar usuários com o mesmo email
        var check = conn.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM Usuario WHERE email=@e";
        check.Parameters.AddWithValue("@e", email);

        var existe = Convert.ToInt64(check.ExecuteScalar());

        if (existe > 0)
            return false;

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Usuario (nome,email,senha)
            VALUES (@n,@e,@s)
        ";

        cmd.Parameters.AddWithValue("@n", nome);
        cmd.Parameters.AddWithValue("@e", email);
        cmd.Parameters.AddWithValue("@s", senha);

        cmd.ExecuteNonQuery();
        return true;
    }

    // Valida se login for realizado com sucesso
    public Usuario? Login(string email, string senha)
    {
        using var conn = Database.GetConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT id_usuario,nome,email,senha
            FROM Usuario
            WHERE email=@e AND senha=@s
        ";

        cmd.Parameters.AddWithValue("@e", email);
        cmd.Parameters.AddWithValue("@s", senha);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
            return null;

        return new Usuario
        {
            Id_Usuario = reader.GetInt32(0),
            Nome = reader.GetString(1),
            Email = reader.GetString(2),
            Senha = reader.GetString(3)
        };
    }
}