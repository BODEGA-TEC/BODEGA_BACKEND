from ldap3 import Server, Connection, ALL, SUBTREE, ALL_ATTRIBUTES

# Define el servidor y la conexión
ldap_server = 'estudiantes.ie.tec.ac.cr'
ldap_port = 389
base_dn = 'dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'
username = 'cn=sibe,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'
password = 'Cg7X4k57QWSc'

# Construir el servidor
server = Server(ldap_server, port=ldap_port, get_info=ALL)

# Intenta conectar y realizar la búsqueda
with Connection(server, user=username, password=password, auto_bind=True) as conn:
    # Realizar una búsqueda
    conn.search(search_base=base_dn, search_filter='(objectClass=*)', search_scope=SUBTREE, attributes=ALL_ATTRIBUTES)

    # Imprimir los resultados en una tabla
    print("{:<20} {:<20}".format("Nombre", "Valor"))
    print("=" * 40)
    for entry in conn.entries:
        for attribute in entry.entry_attributes:
            print("{:<20} {:<20}".format(attribute, entry[attribute]))
        print("-" * 40)


# from ldap3 import Server, Connection, ALL

# # Define el servidor y la conexión
# s = Server('estudiantes.ie.tec.ac.cr', get_info=ALL)
# c = Connection(s, user='cn=sibe,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', password='Cg7X4k57QWSc')

# # Intenta realizar la conexión manualmente
# try:
#     c.bind()
#     print("Conexión exitosa.")
# except Exception as e:
#     print(f"Error de autenticación: {e}")
# finally:
#     # Cerrar la conexión
#     c.unbind()
