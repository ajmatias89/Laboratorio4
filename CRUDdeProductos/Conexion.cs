using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CRUDdeProductos
{
    public class Conexion
    {
        private static string cadenaConexion = "Server=localhost;Database=productos;Uid=root;Pwd=prima1008";
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
                MessageBox.Show("Error al conectar: " + ex.Message);
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
            return listaProductos;
        }
        public static bool InsertSeguro(string tbName, Dictionary<string, object> data)
        {
            var columns = string.Join(", ", data.Keys);
            var placeholders = "@" + string.Join(", @", data.Keys);
            string sql = $"INSERT INTO {tbName} ({columns}) VALUES ({placeholders})";
            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    if (conexion == null) return false;
                    using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
                    {
                        foreach (var kvp in data)
                        {
                            stmt.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                        }
                        stmt.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error en INSERT: " + ex.Message);
                return false;
            }
        }

        public static bool UpdateSeguro(string tbName, Dictionary<string, object> data, string idColumn, int idValue)
        {
            var setParts = new List<string>();
            foreach(var key in data.Keys)
            {
                setParts.Add($"{key} = @{key}");
            }
            string setClause = string.Join(", ", setParts);

            string sql = $"UPDATE {tbName} SET {setClause} WHERE {idColumn} = @idCondicion";

            try
            {
                using (MySqlConnection conexion = ObtenerConexion()) 
                {
                    if(conexion == null) return false;

                    using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
                    {
                        foreach(var kvp in data)
                        {
                            stmt.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                        }
                        stmt.Parameters.AddWithValue("@idCondicion", idValue);
                        stmt.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error en UPDATE: " + ex.Message);
                return false;
            }
        }
        public static bool DeleteSeguro(string tbName, string idColumn, int idValue)
        {
            string sql = $"DELETE FROM {tbName} WHERE {idColumn} = @id";

            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    if (conexion == null)
                        return false;

                    using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
                    {
                        stmt.Parameters.AddWithValue("@id", idValue);

                        stmt.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error en DELETE: " + ex.Message);
                return false;
            }
        }
    }
}