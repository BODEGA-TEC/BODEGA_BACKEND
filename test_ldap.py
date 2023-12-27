import sys
from ldap3 import Server, Connection, ALL, NTLM, ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES, AUTO_BIND_NO_TLS, SUBTREE

server_name = 'ie-estudiantes'
domain_name = 'estudiantes.ie.tec.ac.cr'
user_name = 'sibe'
password = 'Cg7X4k57QWSc'

format_string = '{:25} {:>6} {:19} {:19} {}'
print(format_string.format('User', 'Logins', 'Last Login', 'Expires', 'Description'))

server = Server(server_name, get_info=ALL)
conn = Connection(server, user='{}\\{}'.format(domain_name, user_name), password=password, authentication=NTLM, auto_bind=True)
conn.search('dc={},dc=local'.format(domain_name), '(objectclass=*)', attributes=[ALL_ATTRIBUTES, ALL_OPERATIONAL_ATTRIBUTES])

for e in conn.entries:
    if 'description' in e:
        desc = e.description.value
    else:
        desc = ""

    print(format_string.format(str(e.name), str(e.logonCount.value), str(e.lastLogon.value)[:19], str(e.accountExpires.value)[:19], desc))
