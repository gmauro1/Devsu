1- Correr el script BaseDatos en MS Sql Server.
2- Cambiar la cadena de conexión en appsettings.json : ConnectionStrings->DefaultConnection.
3- Para probar los endpoints utilizar Postman. Importar el archivo Devsu.postman_collection.json.
4- Dentro del Postman en la carpeta Cliente en el método Post (Crear Cliente) hay un test integrar que válida el StatusCode y el Schema de la respuesta.
5- Dentro del projecto en la carpeta Test hay dos test unitarios que validan la creación y obtención de clientes.
