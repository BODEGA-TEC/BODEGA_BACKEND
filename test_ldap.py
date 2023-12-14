from ldap3 import Server, Connection, ALL, NTLM

# Configuración del servidor LDAP
ldap_server = 'estudiantes.ie.tec.ac.cr'
ldap_port = 389  # Puerto LDAP típico

# Información de autenticación
ldap_username = 'sibe'
ldap_password = 'Cg7X4k57QWSc'

# Construir el servidor
server = Server(ldap_server, port=ldap_port, get_info=ALL)

# Intentar conectar al servidor LDAP
try:
    # Puedes usar NTLM para la autenticación si es necesario
    connection = Connection(server, user=f'{ldap_username}@estudiantes.ie.tec.ac.cr', password=ldap_password, authentication=NTLM)

    # Establecer la conexión
    if connection.bind():
        print("Conexión exitosa")
    else:
        print(f"Fallo en la conexión: {connection.result}")
except Exception as e:
    print(f"Error durante la conexión: {e}")
finally:
    # Cerrar la conexión
    connection.unbind()
