import ldap

# Inicializa la conexión LDAP
l = ldap.initialize('ldap://estudiantes.ie.tec.ac.cr')

# Define el nombre de usuario y la contraseña
username = "uid=sibe,ou=People,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr"
password = "Cg7X4k57QWSc"

try:
    # Establece la versión del protocolo LDAP
    l.protocol_version = ldap.VERSION3

    # Intenta iniciar sesión en el servidor LDAP
    l.simple_bind_s(username, password)
    print("Conexión exitosa.")
except ldap.INVALID_CREDENTIALS:
    print("Tu nombre de usuario o contraseña es incorrecto.")
except ldap.LDAPError as e:
    print(e)
