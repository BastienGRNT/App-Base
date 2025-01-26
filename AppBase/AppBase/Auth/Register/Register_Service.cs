using System;
using System.IO;
using Npgsql;
using BCrypt.Net;

namespace AppBase.Auth.Register;

public class Services_Register
{
    public static string UserRegister(Data_Register dataRegister)
    {
        try
        {
            using (var connection = Connect.ConnectSql())
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                string queryCheckRegisterID = Register_Query.Register_query3;
                using (var command = new NpgsqlCommand(queryCheckRegisterID, connection))
                {
                    command.Parameters.AddWithValue("@Identifiant", dataRegister.user_id);
                    int identifiantCount = Convert.ToInt32(command.ExecuteScalar());

                    if (identifiantCount > 0)
                    {
                        return "Erreur : L'identifiant existe déjà.";
                    }
                }

                string QueryCheckRegisterEmail = Register_Query.Register_query2;
                using (var command = new NpgsqlCommand(QueryCheckRegisterEmail, connection))
                {
                    command.Parameters.AddWithValue("@AdresseMail", dataRegister.user_mail);
                    int emailCount = Convert.ToInt32(command.ExecuteScalar());

                    if (emailCount > 0)
                    {
                        return "Erreur : L'adresse e-mail est déjà utilisée.";
                    }
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dataRegister.user_password);

                string QueryPassword = Register_Query.Register_query1;
                using (var command = new NpgsqlCommand(QueryPassword, connection))
                {
                    command.Parameters.AddWithValue("@Identifiant", dataRegister.user_id);
                    command.Parameters.AddWithValue("@AdresseMail", dataRegister.user_mail);
                    command.Parameters.AddWithValue("@MotDePasse", hashedPassword);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine($"Utilisateur {dataRegister.user_id} ajouté avec succès !");
                        return "Utilisateur ajouté avec succès !";
                    }
                    else
                    {
                        return "Erreur lors de l'ajout de l'utilisateur.";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            return $"Erreur : {ex.Message}";
        }
    }
}