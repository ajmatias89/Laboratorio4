# Laboratorio 4 - CRUD de Productos

Aplicación desarrollada en **C# con Windows Forms** para realizar operaciones de gestión de productos mediante una conexión a una base de datos **MySQL**.

## Descripción

El proyecto permite trabajar con información de productos almacenada en una base de datos MySQL.

Cada producto contiene los siguientes datos:

- ID
- Nombre
- Precio
- Cantidad
- Imagen

La aplicación utiliza una interfaz gráfica desarrollada con Windows Forms para facilitar la consulta y administración de los registros.

## Funcionalidades

- Conexión con una base de datos MySQL.
- Visualización de productos.
- Búsqueda y filtrado de productos.
- Registro de información de productos.
- Actualización de productos.
- Eliminación de productos.
- Manejo de imágenes asociadas a los productos.
- Visualización de los registros mediante DataGridView.

## Tecnologías utilizadas

- C#
- .NET
- Windows Forms
- MySQL
- MySql.Data
- Visual Studio

## Estructura principal

### `Productos.cs`

Representa el modelo utilizado para almacenar la información de cada producto.

Sus propiedades principales son:

- `ID`
- `Nombre`
- `Precio`
- `Cantidad`
- `Imagen`

### `Conexion.cs`

Se encarga de establecer la conexión con MySQL y realizar las consultas necesarias para obtener y buscar los productos almacenados en la base de datos.

### Formularios

La interfaz gráfica permite al usuario interactuar con los datos mediante controles de Windows Forms, incluyendo cuadros de texto, botones y DataGridView.

## Base de datos

El proyecto utiliza una base de datos llamada:

`productos`

La tabla principal utilizada es:

`productos`

con los campos correspondientes a la información de cada producto.

## Objetivo del laboratorio

Aplicar los conocimientos de programación en C#, Windows Forms y conexión a bases de datos para desarrollar una aplicación CRUD que permita administrar registros almacenados en MySQL.

## Autor

**Aimee Matias**
4-751-2038
