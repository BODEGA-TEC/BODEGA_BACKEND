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




def consult_all_ous():
    print("\n\nconsult_all_ous")

    filter = '(objectClass=organizationalUnit)'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    print("\n" * 2)
    for index, e in enumerate(conn.entries):
        print(e.ou.value)
     
def consult_specific_ou(ou_name):
    print("\n\nconsult_specific_ou")

    filter = f'(&(objectClass=organizationalUnit)(ou={ou_name}))'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    print("\n" * 2)
    for e in conn.entries:
        print(e)

def consult_all_domain():
    base_dn = 'ou=Computadores,ou=Estudiantes,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'  # Dominio LDAP
    filter = '(objectClass=*)'  # Filtro para buscar todos los objetos

    conn.search(search_base=base_dn, search_filter=filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES], search_scope=SUBTREE)

    for e in conn.entries:
        print(e)

    print("\n" * 2)


def estudiantes_por_carrera(carrera,names):
    print("\n\nestudiantes_por_carrera")
    
    base_dn = f'ou={carrera.capitalize()},ou=Estudiantes,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'  # Ajusta según tu estructura LDAP
    filter = '(objectClass=person)'

    conn.search(search_base=base_dn, search_filter=filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES], search_scope=SUBTREE)

    print("\n" * 2)

    for e in conn.entries:
        if any(name.lower() in e['cn'].value.lower() for name in names):
            print(e)

    print("\n" * 2)


def consult_all_by_name(name):
    print("\n\nconsult_all_by_name")
    
    base_dn = 'ou=Estudiantes,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'  # Ajusta según tu estructura LDAP
    filter = '(objectClass=*)'

    conn.search(base_dn, filter, SUBTREE)

    print("\n" * 2)

    for e in conn.entries:
        # if name.lower() in e['cn'].value.lower():
        #   print(e['cn'].value.lower())
        print(e)

    print("\n" * 2)
    
    
    
# consult_all_ous()
# consult_specific_ou("Computadores")
# consult_all_domain()
# estudiantes_por_carrera("electronica", ["Richards","Axel"])
consult_all_by_name("Richards")