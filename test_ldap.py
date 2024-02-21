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
    print("\n\nconsult_all_ous\n")

    filter = '(objectClass=organizationalUnit)'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    for index, e in enumerate(conn.entries):
        print(e.ou.value)
     
def consult_specific_ou(ou_name):
    print("\n\nconsult_specific_ou\n")

    filter = f'(&(objectClass=organizationalUnit)(ou={ou_name}))'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    for e in conn.entries:
        print(e)

def consult_all_domain():
    base_dn = 'ou=Computadores,ou=Estudiantes,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'  # Dominio LDAP
    filter = '(objectClass=*)'  # Filtro para buscar todos los objetos

    conn.search(search_base=base_dn, search_filter=filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES], search_scope=SUBTREE)

    for e in conn.entries:
        print(e)


def estudiantes_por_carrera(carrera,names):
    print("\n\nestudiantes_por_carrera\n")
    
    base_dn = f'ou={carrera.capitalize()},ou=Estudiantes,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'  # Ajusta según tu estructura LDAP
    filter = '(objectClass=person)'

    conn.search(search_base=base_dn, search_filter=filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES], search_scope=SUBTREE)

    for e in conn.entries:
        if any(name.lower() in e['cn'].value.lower() for name in names):
            print(e)


def consult_all_by_name(name):
    print("\n\nconsult_all_by_name\n")
    
    base_dn = 'dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'
    filter = f'(&(objectClass=user)(cn=*{name}*))'
    
    conn.search(base_dn, filter, SUBTREE, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    for e in conn.entries:
        print(e)
    
    
def consult_all_by_carne(carne):
    print("\n\n-- CONSULTAR LDAP COMPLETA POR CARNE: ", carne)
    print("------------------------------------")
     
    base_dn = 'dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'
    filter = f'(&(objectClass=user)(telephoneNumber={carne}))'
    
    conn.search(base_dn, filter, SUBTREE, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])
    
    for e in conn.entries:
        career = ""
        if "ou=Docentes" in e.entry_dn:
            print("/ DOCENTE")
        elif "ou=Estudiantes" in e.entry_dn:
            
            dnsplit = e.entry_dn.split(",")
            career = dnsplit[2]
            print("/ ESTUDIANTE")
            
        print("Nombre:", e['cn'])
        print("Carne:",  e['telephoneNumber'])
        print("Correo:", e['userPrincipalName'], ' [No es el correo verdadero]')
        print("Carrera:", career)

print("\n" * 2)
    
# consult_all_ous()
# consult_specific_ou("Computadores")
# consult_all_domain()
# estudiantes_por_carrera("electronica", ["Richards","Axel"])
#consult_all_by_name("Leonardo Sandoval")
consult_all_by_carne('110760813')



print("\n" * 2)