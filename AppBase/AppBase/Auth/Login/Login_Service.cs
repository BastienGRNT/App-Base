using BCrypt.Net;
using Npgsql;

namespace AppBase.Auth.Login;

public class CheckLoginClass
{
    public string UserLogin(Data_Login dataLogin)
    {
        using (var connection = Connect.ConnectSql())
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            string query = Login_Query.Login_query;
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Identifiant", dataLogin.user_id);

                string CheckHashedPassword = command.ExecuteScalar() as string;

                if (string.IsNullOrEmpty(CheckHashedPassword))
                {
                    return "Erreur : L'identifiant n'existe pas !";
                }

                bool CorrectPassword = BCrypt.Net.BCrypt.Verify(dataLogin.user_password, CheckHashedPassword);
                if (CorrectPassword)
                {
                    return "Mot de passe correct !";
                }
                else
                {
                    return "Mot de passe incorrect !";
                }
            }
        }
    }
}