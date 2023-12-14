from ldap3 import Server, Connection, exceptions

def verify_ldap_domain(domain):
    try:
        # Intenta conectar al servidor LDAP sin proporcionar credenciales
        server = Server(domain)
        connection = Connection(server, auto_bind=True)
        connection.unbind()
        return True  # Si no hay excepción, el dominio existe
    except exceptions.LDAPException:
        return False  # Si hay excepción, el dominio no existe o no es accesible

# Uso del método
domain_to_check = "estudiantes.ie.tec.ac.cr"
if verify_ldap_domain(domain_to_check):
    print(f"El dominio LDAP '{domain_to_check}' es válido.")
else:
    print(f"El dominio LDAP '{domain_to_check}' no es válido o no es accesible.")
