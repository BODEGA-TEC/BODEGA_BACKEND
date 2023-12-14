import ldap3

class Ldap:
    """Class for LDAP related connections/operations."""

    def __init__(self, server_uri, ldap_user, ldap_pass):
        self.server = ldap3.Server(server_uri, get_info=ldap3.ALL)
        self.conn = ldap3.Connection(self.server, user=ldap_user, password=ldap_pass, auto_bind=True)

    def who_am_i(self):
        return self.conn.extend.standard.who_am_i()

    def get_groups(self):
        self.conn.search('dc=fill_this_1,dc=fill_this_2', '(objectClass=group)')
        return self.conn.entries

    def get_groups_with_members(self):
        fq_groups = [result.entry_dn for result in ldap.get_groups()]

        groups_with_members = {}
        for group in fq_groups:
            self.conn.search(group, '(objectclass=group)', attributes=['sAMAccountName'])

            if 'sAMAccountName' in self.conn.entries[0]:
                groups_with_members[group] = self.conn.entries[0]['sAMAccountName'].values

        return groups_with_members

    def get_members_with_groups(self):
        groups_with_members = self.get_groups_with_members()

        members_with_groups = {}
        for group, members in groups_with_members.items():
            for member in members:
                if not member in members_with_groups:
                    members_with_groups[member] = []

                members_with_groups[member].append(group)

        return members_with_groups


if __name__ == '__main__':
    LDAP_URI = 'estudiantes.ie.tec.ac.cr'
    base_dn = 'dc=estudiantes,dc=ie,dc=tec,dc=ac,dc=cr'
    username = 'sibe'
    password = 'Cg7X4k57QWSc'
    try:
        ldap = Ldap(LDAP_URI, username, password)
        if ldap:
            print('User authenticated. Welcome {0}'.format(ldap.who_am_i()))
        print(ldap.get_groups())
    except ldap3.core.exceptions.LDAPBindError as bind_error:
        print(str(bind_error))
    except ldap3.core.exceptions.LDAPPasswordIsMandatoryError as pwd_mandatory_error:
        print(str(pwd_mandatory_error))