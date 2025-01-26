namespace AppBase.Auth.Login;

public class Login_Query
{
    public const string Login_query = "SELECT mot_de_passe FROM user_login WHERE identifiant = @Identifiant;";
}