from ldap3 import Server, Connection, ALL, SUBTREE

domain = 'estudiantes.ie.tec.ac.cr'
base_dn = 'dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'
username = 'cn=sibe,'+base_dn

# Define el servidor y la conexión
s = Server(domain, get_info=ALL)
c = Connection(s, user=username, password='admin')

# Intenta realizar la conexión manualmente
try:
    c.bind()
    print("Conexión exitosa.")

    # Realiza la búsqueda para recuperar la entrada del usuario
    if c.search(base_dn, f'(cn={username})', SUBTREE):
        # Imprime los atributos de la entrada del usuario
        entry = c.response[0]
        print("Permisos del usuario:")
        for attribute, values in entry['attributes'].items():
            print(f"  {attribute}: {values}")
    else:
        print("No se encontró la entrada del usuario.")
except Exception as e:
    print(f"Error de autenticación: {e}")
finally:
    # Cerrar la conexión
    c.unbind()



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
