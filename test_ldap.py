from ldap3 import Server, Connection, ALL, SUBTREE

domain = 'estudiantes.ie.tec.ac.cr'
base_dn = 'dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'
username = 'cn=sibe,'+base_dn

# Define el servidor y la conexión
s = Server(domain, get_info=ALL)
c = Connection(s, user=username, password='Cg7X4k57QWSc')

# Intenta realizar la conexión manualmente
try:
    c.bind()
    print("Conexión exitosa.")

    # Define la base de búsqueda y el filtro de búsqueda
    search_filter = '(objectClass=organizationalUnit)'

    # Realiza la búsqueda
    if c.search(base_dn, search_filter, SUBTREE):
        print("Unidades Organizativas encontradas:")
        for entry in c.response:
            print(entry['dn'])
            # for attribute, values in entry['attributes'].items():
            #     print(f"  {attribute}: {values}")
    else:
        print("No se encontraron unidades organizativas.")
                
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