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


base_dn = 'ou=Estudiantes,dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'  # Ajusta según tu estructura LDAP      
filter = '(&(objectClass=person))'

# conn.search(search_base=base_dn, search_filter=filter, attributes=[ALL_ATTRIBUTES], search_scope=SUBTREE)

# i = 0
# print("\n" * 2)
# names = []
# for e in conn.entries:

#     # i+=1
#     # if i>=4:
#     #     break

#     try:
#         # if 'M' in e['name'].value[0]:
#         #     # print("=" * 80)
#         #     names.append(e['name'].value)
#         print(e)

#         #     print()
#         # print(e['sAMAccountName'])
#         # print(e)

#     except:
#         continue


# print("\n" * 4)

# names.sort()
# for n in names:
#     print(n)






def test():
    filter = '(objectClass=organizationalUnit)'
    conn.search('dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr', filter, attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

    for index, e in enumerate(conn.entries):
            # if index >= 4:
            #     break  # Sale del bucle después de imprimir las primeras 3 entradas    
        ou_name = e.ou.value
        print(f"- {ou_name} {e.ou.definition}")
        
test()