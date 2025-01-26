namespace AppBase.Auth.Register;

public class Register_Query
{
    public const string Register_query1 = "SELECT COUNT(*) FROM user_login WHERE identifiant = @Identifiant;";
    public const string Register_query2 = "SELECT COUNT(*) FROM user_login WHERE adresse_mail = @AdresseMail;";
    public const string Register_query3 = "INSERT INTO user_login (identifiant, adresse_mail, mot_de_passe) VALUES (@Identifiant, @AdresseMail, @MotDePasse);";
}