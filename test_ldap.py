import sys
from ldap3 import Server, Connection, ALL, NTLM, ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES, AUTO_BIND_NO_TLS, SUBTREE
from pprint import pprint  # Importa pprint desde el módulo pprint
server_name = 'ie-estudiantes'
domain_name = 'estudiantes.ie.tec.ac.cr'
user_name = 'sibe'
password = 'Cg7X4k57QWSc'

format_string = '{:25} {:>6} {:19} {:19}'
# print(format_string.format('User', 'DisplayName', 'userPrincipalName', 'Expires'))

server = Server(server_name, get_info=ALL)
conn = Connection(server, user='{}\\{}'.format(domain_name, user_name), password=password, authentication=NTLM, auto_bind=True)

def print_entities_in_ou():
    
    ou_filter = '(&(objectCategory=*)(objectClass=*)(ou=Estudiantes))'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', ou_filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    i = 0
    for e in conn.entries:
        # if index >= 1:
        #     break  # Sale del bucle después de imprimir las primeras 3 entradas
        # attributes = e.entry_raw_attributes
        # print(attributes.items())
        # print("=" * 80)
        
        print(e)
        print("=" * 80)

        i+=1
        if i>=10:
            break
        # t = type(e.entry_raw_attributes())
        # print(t)

# Llamada a la función para imprimir las entidades en la OU "Docentes"
print_entities_in_ou()









# def test():
#     filter = '(objectClass=organizationalUnit)'
#     conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

#     for index, e in enumerate(conn.entries):
#             # if index >= 4:
#             #     break  # Sale del bucle después de imprimir las primeras 3 entradas    
#         ou_name = e.ou.value
#         print(f"- {ou_name}")
        
# test()

#     # print(f"Entry: {e.dn}")
    # for attribute in e:
    #     print(f"  {attribute.key}: {attribute.values}")
    # print("=" * 50)
    # print(format_string.format(str(e.name), str(e.displayName), str(e.userPrincipalName)[:19], str(e.accountExpires.value)[:19]))