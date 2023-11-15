# pip install mysql-connector-python
# pip install pytz
import csv
import mysql.connector
from datetime import datetime, timezone, timedelta
import pytz

# Diccionario de condiciones
condicion_dict = {'BUENO': 1, 'REGULAR': 0, 'DAÑADO': -1}
estado_dict = {
    'DISPONIBLE': 1,
    'PRESTADO': 2,
    'AGOTADO': 3,
    'DAÑADO': 4,
    'EN REPARACION': 5,
    'RETIRADO': 6,
    'APARTADO': 7
}

def getCategoria(tipo, categoria):
    cursor = connection.cursor()
    
    # Mapea el tipo a un entero
    tipo = 1 if tipo.lower() == 'componente' else 2

    # Verifica si la categoría ya existe en la base de datos
    query = "SELECT Id FROM Categoria WHERE Tipo = %s AND Nombre = %s"
    cursor.execute(query, (tipo, categoria))
    result = cursor.fetchone()

    if result:
        # Si existe, devuelve el ID de la categoría existente
        return result[0]
    else:
        # Si no existe, crea la categoría y devuelve el ID recién creado
        query = "INSERT INTO Categoria (Tipo, Nombre) VALUES (%s, %s)"
        cursor.execute(query, (tipo, categoria))
        connection.commit()
        return cursor.lastrowid


def getActivoBodega(table_name):
    cursor = connection.cursor()
    
    # Obtener el ultimo Id de la tabla especificada
    query = f"SELECT MAX(Id) FROM {table_name.capitalize()}"
    cursor.execute(query)
    max_id = cursor.fetchone()[0]

    # Calcular codigo bodega
    scope_identity = max_id + 1 if max_id else 1
    formatted_digit = f"{scope_identity:06}"

    # Utiliza el nombre de la tabla para determinar el prefijo del código
    prefix = "BE" if table_name.lower() == "equipo" else "BC"
    
    code = f"{prefix}{formatted_digit}"
    return code


def process_equipo(file_path):
    with open(file_path, 'r', newline='', encoding='latin-1') as csv_file:
        csv_reader = csv.reader(csv_file, delimiter=',')
        headers = next(csv_reader)

        # Encabezados esperados
        expected_headers = ['CATEGORIA', 'ESTADO', 'DESCRIPCION', 'MARCA', 'MODELO', 'ACTIVOTEC',  'SERIE', 'OBSERVACIONES', 'CONDICION', 'ESTANTE', 'CANTIDAD']

        # Asegurarse de que todos los encabezados esperados estén presentes
        if all(header in headers for header in expected_headers):
            for row in csv_reader:
                # Asigna los valores a variables según los encabezados dinámicamente, debe ser el mismo orden que el de expected headers
                categoria, estado, descripcion, marca, modelo, activoTec, serie, observaciones, condicion, estante, cantidad = (
                    row[headers.index(header)] for header in expected_headers
                )

                # Insertar a dB
                cursor = connection.cursor()
                for _ in range(int(cantidad)):
                    
                    # Id, CategoriaId, EstadoId, FechaRegistro, Descripcion, Marca, Modelo, ActivoBodega, ActivoTec, Serie, Observaciones, Condicion, Estante
                    values = (
                        getCategoria("equipo", categoria.upper()),              # CategoriaId
                        estado_dict.get(estado.upper(), 1),                     # EstadoId: DISPONIBLE por defecto
                        datetime.now(pytz.timezone('America/Costa_Rica')),      # FechaRegistro
                        descripcion.upper(),                                    # Descripcion
                        None if marca.strip() == '' else marca,                 # Marca
                        None if modelo.strip() == '' else modelo,               # Modelo
                        getActivoBodega("equipo"),                              # ActivoBodega
                        activoTec.upper().replace(" ", "").replace("'", "-"),   # ActivoTec: Muchos ponen "'" en vez de "-"
                        None if serie.strip() == '' else serie,                 # Serie
                        None if observaciones.strip() == '' else observaciones, # Observaciones
                        condicion_dict.get(condicion.upper(), 1),               # Condicion: Convierte la condición a mayúsculas y asigna por defecto "BUENO" si no es válida
                        estante.upper(),                                        # Estante
                    )

                    query = """
                        INSERT INTO Equipo (CategoriaId, EstadoId, FechaRegistro, Descripcion, Marca, Modelo, ActivoBodega, ActivoTec, Serie, Observaciones, Condicion, Estante)
                        VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
                    """
                    cursor.execute(query, values)
                    connection.commit()
    
    print("[DONE] Equipo")

def process_componentes(file_path):
    with open(file_path, 'r', newline='', encoding='latin-1') as csv_file:
        csv_reader = csv.reader(csv_file, delimiter=',')
        headers = next(csv_reader)

        # Encabezados esperados
        expected_headers = ['CATEGORIA', 'ESTADO', 'DESCRIPCION', 'CANTIDAD', 'OBSERVACIONES', 'CONDICION', 'ESTANTE', 'MODELO']

        # Asegurarse de que todos los encabezados esperados estén presentes
        if all(header in headers for header in expected_headers):
            for row in csv_reader:
                # Asigna los valores a variables según los encabezados dinámicamente, debe ser el mismo orden que el de expected headers
                categoria, estado, descripcion, cantidad, observaciones, condicion, estante, modelo= (
                    row[headers.index(header)] for header in expected_headers
                )

                # Insertar a dB
                cursor = connection.cursor()
                    
                # Id, CategoriaId, EstadoId, FechaRegistro, Descripcion, Cantidad, ActivoBodega, Observaciones, Estante, Condicion, Modelo
                values = (
                    getCategoria("componente", categoria.upper()),          # CategoriaId
                    estado_dict.get(estado.upper(), 1),                     # EstadoId: DISPONIBLE por defecto
                    datetime.now(pytz.timezone('America/Costa_Rica')),      # FechaRegistro
                    '?' if descripcion.strip() == '' else descripcion,      # Descripcion
                    cantidad,                                               # Cantidad
                    getActivoBodega("componente"),                          # ActivoBodega
                    None if observaciones.strip() == '' else observaciones, # Observaciones
                    estante.upper(),                                        # Estante
                    condicion_dict.get(condicion.upper(), 1),               # Condicion: Convierte la condición a mayúsculas y asigna por defecto "BUENO" si no es válida
                    None if modelo.strip() == '' else modelo,               # Modelo
                )

                query = """
                    INSERT INTO Componente (CategoriaId, EstadoId, FechaRegistro, Descripcion, Cantidad, ActivoBodega, Observaciones, Estante, Condicion, Modelo)
                    VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
                """
                cursor.execute(query, values)
                connection.commit()
    
    print("[DONE] Componentes")



# Ejecuta la función main al abrir el archivo
if __name__ == "__main__":
    # Configura la conexión a la base de datos
    connection = mysql.connector.connect(
        host="localhost",
        user="root",
        password="admin",
        database="sibe_db"
    )

    # Procesa el archivo 'equipo.csv'
    process_equipo('equipo.csv')
    process_componentes('componentes.csv')

    # Cierra la conexión a la base de datos
    connection.close()