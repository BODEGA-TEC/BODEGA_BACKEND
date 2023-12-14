from ldap3 import Server, Connection, ALL

# Define el servidor y la conexión
s = Server('estudiantes.ie.tec.ac.cr', get_info=ALL)
c = Connection(s, user='cn=sibe,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', password='Cg7X4k57QWSc', auto_bind=True)

# Imprime un mensaje si la conexión es exitosa
if c.bound:
    print("Conexión exitosa.")
