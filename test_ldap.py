import sys
from ldap3 import Server, Connection, ALL, NTLM, SUBTREE, ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES, AUTO_BIND_NO_TLS, SUBTREE
from pprint import pprint  # Importa pprint desde el módulo pprint
server_name = 'ie-estudiantes'
domain_name = 'estudiantes.ie.tec.ac.cr'
user_name = 'sibe'
password = 'Cg7X4k57QWSc'

format_string = '{:25} {:>6} {:19} {:19}'
# print(format_string.format('User', 'DisplayName', 'userPrincipalName', 'Expires'))

server = Server(server_name, get_info=ALL)
conn = Connection(server, user='{}\\{}'.format(domain_name, user_name), password=password, authentication=NTLM, auto_bind=True)
#(memberOf=CN=allestudiantes,OU=Grupos,DC=estudiantes,DC=ie,DC=tec,DC=ac,DC=cr)




def consult_all_ou():
    print("\n\nconsult_all_ou")

    filter = '(objectClass=organizationalUnit)'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    print("\n" * 2)
    for e in enumerate(conn.entries):
        print(e.ou.value)
     
def consult_specific_ou(ou_name):
    print("\n\nconsult_specific_ou")

    filter = f'(&(objectClass=organizationalUnit)(ou={ou_name}))'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    print("\n" * 2)
    for e in conn.entries:
        print(e)


def estudiantes_por_carrera(carrera,names):
    print("\n\nestudiantes_por_carrera")
    carrera = carrera.capitalize()
    print(carrera)
    base_dn = 'ou={carrera},ou=Estudiantes,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'  # Ajusta según tu estructura LDAP
    filter = '(objectClass=person)'

    conn.search(search_base=base_dn, search_filter=filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES], search_scope=SUBTREE)

    print("\n" * 2)

    for e in conn.entries:
        ##if e['cn'].value.lower() in [name.lower() for name in names]:
        print(e)

    print("\n" * 2)

    
# consult_all_ou()
# consult_specific_ou("Computadores")
estudiantes_por_carrera("computadores", ["MichaeL","Richards","Alexis"])