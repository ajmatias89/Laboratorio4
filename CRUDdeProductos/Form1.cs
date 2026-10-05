using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace CRUDdeProductos
{
    public partial class Form1 : Form
    {
        int idProducto;
        bool todoOk = true;
        List<(TextBox txt, Ivalidador validador)> camposValidar = new List<(TextBox txt, Ivalidador validador)>();
        private List<Productos> listaProductos;
        private Dictionary<string, object> myProducto = new Dictionary<string, object>();
        public Form1()
        {
            InitializeComponent();
            listaProductos = new List<Productos>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargarProductos();
        }

        private void cargarProductos(string filtro = "")
        {
            dgvCrud.Rows.Clear();
            dgvCrud.Refresh();
            listaProductos = Conexion.GetProductos(filtro);

            foreach (var prod in listaProductos)
            {
                Image img = null;
                if (prod.Imagen != null && prod.Imagen.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(prod.Imagen))
                    {
                        using (Bitmap bmp = new Bitmap(ms))
                        {
                            img = new Bitmap(bmp);
                        }
                    }
                }
                dgvCrud.Rows.Add(prod.ID, prod.Nombre, prod.Precio, prod.Cantidad, img);
            }
        }

        private void dgvCrud_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow fila = dgvCrud.Rows[e.RowIndex];

            txtFolio.Text = Convert.ToInt32(fila.Cells[0].Value).ToString();
            txtNombre.Text = Convert.ToString(fila.Cells[1].Value);
            txtPrecio.Text = Convert.ToDecimal(fila.Cells[2].Value).ToString();
            txtCantidad.Text = Convert.ToInt32(fila.Cells[3].Value).ToString();

            btnAgregar.Enabled = false;
            btnModificar.Enabled = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            ModificarDatosBD();
        }

        private void ModificarDatosBD()
        {
            CargarDatosProductos();
            int Folio = int.Parse(txtFolio.Text);

            MessageBox.Show("El id del producto es: " + Folio);

            bool resultado = Conexion.UpdateSeguro("Productos", myProducto, "id", Folio);

            if(resultado)
            {
                Console.WriteLine("Actualización existosa");
            }
        }

        private void CargarDatosProductos()
        {
            myProducto["cantidad"] = int.Parse(txtCantidad.Text.Trim());
            myProducto["precio"] = decimal.Parse(txtPrecio.Text.Trim());
            myProducto["nombre"] = txtNombre.Text.Trim();
            myProducto["Imagen"] = ImageToByteArray(pictureBox1.Image);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
            btnModificar.Enabled = false;
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            cargarProductos(txtBusqueda.Text.Trim());
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!datosCorrectos())
            {
                return;
            }

            CargarDatosProductos();

            if(Conexion.InsertSeguro("Productos", myProducto))
            {
                MessageBox.Show("Se ha agregado satisfactoriamente el producto");
                cargarProductos();
                limpiarCampos();
            }
            else
            {
                MessageBox.Show("ERROR: No se pudo insertar el producto");
            }
        }

        private void pbImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar imagen";
                openFileDialog.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void limpiarCampos()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtCantidad.Clear();
            pictureBox1.Image = null;
            btnAgregar.Enabled = true;
        }

        private byte[] ImageToByteArray(Image Img)
        {
            if(Img == null)
            {
                return null;
            }
            using (MemoryStream mMemoryStream = new MemoryStream())
            {
                Img.Save(mMemoryStream, Img.RawFormat);
                return mMemoryStream.ToArray();
            }
        }
        private bool datosCorrectos()
        {
            camposValidar.Add((txtNombre, new ValidatorTexto()));
            camposValidar.Add((txtPrecio, new ValidadorDecimal()));
            camposValidar.Add((txtCantidad, new ValidadorEntero()));

            foreach(var item in camposValidar)
            {
                if (!item.validador.EsValido(item.txt.Text))
                {
                    errorProvider1.SetError(item.txt, item.validador.MensajeError);
                    todoOk = false;
                    break;
                }
                else
                {
                    errorProvider1.SetError(item.txt, string.Empty);
                    todoOk = true;
                }
            }
            return true;

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolio.Text))
            {
                MessageBox.Show("Seleccione un producto para eliminar");
                return;
            }

            int folio = int.Parse(txtFolio.Text);

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este producto?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                if (Conexion.DeleteSeguro("Productos", "id", folio))
                {
                    MessageBox.Show("Producto eliminado correctamente");

                    cargarProductos();
                    limpiarCampos();
                    txtFolio.Clear();

                    btnModificar.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el producto");
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    } 

}