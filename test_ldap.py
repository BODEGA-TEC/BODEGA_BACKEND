import sys
from ldap3 import Server, Connection, ALL, NTLM, ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES, AUTO_BIND_NO_TLS, SUBTREE

server_name = 'ie-estudiantes'
domain_name = 'estudiantes.ie.tec.ac.cr'
user_name = 'sibe'
password = 'Cg7X4k57QWSc'

format_string = '{:25} {:>6} {:19} {:19}'
# print(format_string.format('User', 'DisplayName', 'userPrincipalName', 'Expires'))

server = Server(server_name, get_info=ALL)
conn = Connection(server, user='{}\\{}'.format(domain_name, user_name), password=password, authentication=NTLM, auto_bind=True)

def print_entities_in_ou(ou_name):
    
    ou_filter = f'(&(objectClass=*)(ou={ou_name}))'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', ou_filter, attributes=[ALL_ATTRIBUTES])

    print(f"Listado de entidades en la OU - '{ou_name}':")
    for index, e in enumerate(conn.entries):
        # if index >= 3:
        #     break  # Sale del bucle después de imprimir las primeras 3 entradas

        print(f"- {e}")

# Llamada a la función para imprimir las entidades en la OU "Docentes"
print_entities_in_ou('Docentes')









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