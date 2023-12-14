using System;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;

public class LdapService
{
    private readonly string _domain;
    private readonly string _baseDn;
    private readonly string _username;
    private readonly string _password;

    public LdapService(string domain, string baseDn, string username, string password)
    {
        _domain = domain;
        _baseDn = baseDn;
        _username = username;
        _password = password;
    }

    public void GetLdapServerDetails()
    {
        using (LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(_domain)))
        {
            connection.Credential = new System.Net.NetworkCredential(_username, _password);
            connection.AuthType = AuthType.Basic;

            connection.Bind();

            Console.WriteLine("Detalles del servidor LDAP:");
            Console.WriteLine($"  Servidor: {_domain}");
            Console.WriteLine($"  Usuario de conexión: {_username}");

            // Puedes agregar más detalles según tus necesidades

            connection.Dispose();
        }
    }
}
