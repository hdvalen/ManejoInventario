# 📦 Sistema de Gestión de Inventario

Este proyecto implementa un sistema de gestión de inventario en lenguaje C# con conexión a MySQL, permitiendo realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) sobre los productos almacenados en la base de datos.

## ✨ Características

- ✅ **Crear** nuevos productos en el inventario
- 📋 **Listar** todos los productos existentes
- 🔍 **Buscar** productos por ID o nombre
- 🔄 **Actualizar** información de productos existentes
- ❌ **Eliminar** productos del inventario
- 💻 **Interfaz** de línea de comandos amigable

## 📋 Requisitos Previos

Para ejecutar este sistema necesitarás tener instalado:

- 🔧 Extensiones de C#
- 🗄️ MySQL Server 

## 🗄️ Configuración de la Base de Datos

1. Inicia sesión en MySQL:
   ```bash
   mysql -u root -p
   ```

2. Crea la base de datos:
   ```sql
   CREATE DATABASE gestionInventario;
   USE gestionInventario;
   ```
3. Insertar las tablas (db.sql)

4. Para insertar datos (inserts.sql)

5. En el archivo Application/config/AppSettings.cs  se edita el nombre y la contraseña del MySql

## 🔨 Compilación

Ya creada la Base de datos, para compilar el proyecto:

```bash
 dotnet run
```
## Git Flow
![image](https://github.com/user-attachments/assets/756951da-aa40-45d8-930d-044c46902bf6)

## ❓ Solución de Problemas

### Error de conexión a la base de datos
- ✅ Verifica que el servicio MySQL esté ejecutándose
- ✅ Comprueba que las credenciales en el archivo de configuración sean correctas
- ✅ Confirma que la base de datos "gestionInventario" existe


