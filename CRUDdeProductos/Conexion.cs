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

            string query = @"SELECT id, nombre, precio, cantidad,
                     imagen, anulado, fechaCreacion,
                     fechaModificacion, usuario
                     FROM productos
                     WHERE anulado = 0";

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query += @" AND (CAST(id AS CHAR) LIKE @filtro
                    OR nombre LIKE @filtro
                    OR CAST(precio AS CHAR) LIKE @filtro
                    OR CAST(cantidad AS CHAR) LIKE @filtro)";
            }

            using (MySqlConnection conn = ObtenerConexion())
            {
                if (conn == null)
                    return listaProductos;

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(filtro))
                    {
                        cmd.Parameters.AddWithValue(
                            "@filtro", "%" + filtro + "%");
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Productos prod = new Productos();

                            prod.ID = Convert.ToInt32(reader["id"]);
                            prod.Nombre = reader["nombre"].ToString();
                            prod.Precio = Convert.ToDecimal(reader["precio"]);
                            prod.Cantidad = Convert.ToInt32(reader["cantidad"]);

                            prod.Imagen = reader["imagen"] != DBNull.Value
                                ? (byte[])reader["imagen"] : null;

                            prod.Anulado =
                                Convert.ToBoolean(reader["anulado"]);

                            prod.FechaCreacion =
                                Convert.ToDateTime(reader["fechaCreacion"]);

                            prod.FechaModificacion =
                                Convert.ToDateTime(reader["fechaModificacion"]);

                            prod.Usuario = reader["usuario"] == DBNull.Value
                                ? "" : reader["usuario"].ToString();

                            listaProductos.Add(prod);
                        }
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
            foreach (var key in data.Keys)
            {
                setParts.Add($"{key} = @{key}");
            }
            string setClause = string.Join(", ", setParts);

            string sql = $"UPDATE {tbName} SET {setClause} WHERE {idColumn} = @idCondicion";

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

            if (tbName != "Productos" || idColumn != "id")
                return false;

            string sql = @"UPDATE productos
                   SET anulado = 1,
                       usuario = @usuario
                   WHERE id = @id AND anulado = 0";

            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    if (conexion == null)
                        return false;

                    using (MySqlCommand stmt =
                        new MySqlCommand(sql, conexion))
                    {
                        stmt.Parameters.AddWithValue("@id", idValue);
                        stmt.Parameters.AddWithValue(
                            "@usuario", Environment.UserName);

                        return stmt.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al anular: " + ex.Message);
                return false;
            }
        }
    }
}