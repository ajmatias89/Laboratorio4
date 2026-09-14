using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace CRUDdeProductos
{
    public class Conexion
    {
        private static string cadenaConexion = "Server=localhost;Database=productosdb;Uid=root;Pwd=prima1008";

        public static MySqlConnection ObtenerConexion()
        {
            try
            {
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                return (conexion);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al conectar: " + ex.Message);
                return null;
            }
        }

        public static List<Productos> GetProductos(string filtro)
        {
            List<Productos> listaProductos = new List<Productos>();
            string query = "SELECT id, nombre, precio, cantidad, imagen FROM productos";
 
            if (!string.IsNullOrEmpty(filtro))
            {
                query += " WHERE id LIKE @filtro OR nombre LIKE @filtro " + " OR precio LIKE @filtro OR cantidad LIKE @filtro";
            }
            using (MySqlConnection conn = ObtenerConexion())
            {
                if (conn == null) return listaProductos;
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    using (MySqlDataReader mReader = cmd.ExecuteReader())
                    {

                        while (mReader.Read())
                        {
                            Productos prod = new Productos();

                            prod.ID = Convert.ToInt32(mReader["id"]);
                            prod.Nombre = mReader["nombre"].ToString();
                            prod.Precio = Convert.ToDecimal(mReader["precio"]);
                            prod.Cantidad = Convert.ToInt32(mReader["cantidad"]);
                            prod.Imagen = mReader["imagen"] != DBNull.Value ? (byte[])mReader["imagen"] : null;
                            listaProductos.Add(prod);
                        }
                        mReader.Close();
                    }
                }
            }
        }
    }
}