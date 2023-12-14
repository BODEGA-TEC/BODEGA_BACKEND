using System;
using System.DirectoryServices.Protocols;

public class LdapService
{
    private readonly string _domain;
    private readonly string _username;
    private readonly string _password;

    public LdapService(string domain, string username, string password)
    {
        _domain = domain;
        _username = username;
        _password = password;
    }

    public bool VerifyLdapDomain()
    {
        try
        {
            // Intenta conectar al servidor LDAP sin proporcionar credenciales
            using (LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(_domain)))
            {
                connection.Bind();
                return true; // Si no hay excepción, el dominio existe
            }
        }
        catch (LdapException)
        {
            return false; // Si hay excepción, el dominio no existe o no es accesible
        }
    }

    public bool VerifyLdapUser()
    {
        try
        {
            // Intenta conectar al servidor LDAP con credenciales
            using (LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(_domain)))
            {
                connection.Credential = new System.Net.NetworkCredential(_username, _password);
                connection.AuthType = AuthType.Basic;

                connection.Bind();
                return true; // Si no hay excepción, el usuario existe y las credenciales son válidas
            }
        }
        catch (LdapException)
        {
            return false; // Si hay excepción, el usuario no existe o las credenciales no son válidas
        }
    }
}
