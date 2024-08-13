using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace TRW1
{
    public partial class CreaUsuario : Form
    {
        //Conexion SQL
        SqlConnection conexion;
        DirConexion dirCon = new DirConexion();

        public CreaUsuario()
        {
            InitializeComponent();
            conexion = dirCon.crearConexion();
            usuarios();
        }
        private void usuarios()
        {
            comboTipo.DisplayMember = "Text";
            comboTipo.ValueMember = "Value";

            List<ComboboxItem> list = new List<ComboboxItem>();


            ComboboxItem item = new ComboboxItem();
            item.Text = "SUPERVISOR";
            item.Value = "1";
            list.Add(item);

            item = new ComboboxItem();
            item.Text = "CAJERO";
            item.Value = "3";
            list.Add(item);

            comboTipo.DataSource = list;
        }
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void CreaUsuario_Load(object sender, EventArgs e)
        {

        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (textNombre.Text != "" && textBoxApellido.Text != "" && textContraseña.Text != "" && comboTipo.Text != "")
            {
                guardarDatos(textNombre.Text, textBoxApellido.Text, textContraseña.Text, comboTipo.Text);
                CreaUsuario_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Tienes que llenar todos los campos requeridos para dar de alta un Usuario", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void guardarDatos(string numEmpleado, string nombre, string contraseña, string tipo)
        {

            conexion.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.InsertCommand = new SqlCommand("INSERT INTO tusuarios VALUES (@numEmpleado, @nombre, @contraseña, @tipo, '1')", conexion);


            query.InsertCommand.Parameters.Add("@numEmpleado", SqlDbType.NVarChar).Value = numEmpleado;
            query.InsertCommand.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = nombre;
            query.InsertCommand.Parameters.Add("@contraseña", SqlDbType.NVarChar).Value = contraseña;
            query.InsertCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = tipo;


            try
            {

                query.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("El Nuevo Usuario fue dado de Alta con Exito...", "Nuevo Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conexion.Close();

                limpiar();

            }
            catch
            {
                conexion.Close();
                MessageBox.Show("error");
            }

        }
        private void limpiar()
        {

            textNombre.Clear();
            textContraseña.Clear();
            textBoxApellido.Clear();
            comboTipo.SelectedIndex = -1;



        }

        //Codigo para que solo asepte letras  Recordar agregar el evento keypress al textbox que queramos
        private void txtPruebaTexto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        //Codigo para que solo asepte Numeros Recordar agregar el evento keypress al textbox que queramos
        private void txtPruebaNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {

        }



        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }


        private void buttonLimpiar_MouseEnter(object sender, EventArgs e)
        {
            buttonGuardar.BackgroundImage = null;
            buttonGuardar.BackgroundImage = Properties.Resources.MB_0015_reload2;
        }

        private void buttonLimpiar_MouseLeave(object sender, EventArgs e)
        {
            buttonGuardar.BackgroundImage = null;
            buttonGuardar.BackgroundImage = Properties.Resources.MB_0015_reload2blanco;
        }



        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void dataGridViewUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBoxNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void textContraseña_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void button6_MouseEnter(object sender, EventArgs e)
        //{
        //    button6.BackgroundImage = null;
        //    button6.BackgroundImage = Properties.Resources.MB_0019_shut_downrojo;


        //}

        //private void button6_MouseLeave(object sender, EventArgs e)
        //{
        //    button6.BackgroundImage = null;
        //    button6.BackgroundImage = Properties.Resources.MB_0019_shut_down;

        //}

        //private void buttonGuardar_MouseEnter(object sender, EventArgs e)
        //{
        //    button1.BackgroundImage = null;
        //    button1.BackgroundImage = Properties.Resources.MB_0008_save_copia1;
        //}

        //private void buttonGuardar_MouseLeave(object sender, EventArgs e)
        //{
        //    button1.BackgroundImage = null;
        //    button1.BackgroundImage = Properties.Resources.MB_0008_saveblanco;
        //}

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            if (textNombre.Text != "" && textBoxApellido.Text != "" && textContraseña.Text != "" && comboTipo.Text != "")
            {
                //MessageBox.Show("texto: " + comboTipo.Text + "  valor:" + comboTipo.SelectedValue);
                guardarDatos(textNombre.Text, textBoxApellido.Text, textContraseña.Text, comboTipo.SelectedValue.ToString());
                CreaUsuario_Load(sender, e);
                Login frm = new Login();


                frm.Show(this);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tienes que llenar todos los campos requeridos para dar de alta un Usuario", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }



        private void textContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Para obligar a que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan
                    e.Handled = true;
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login frm = new Login();


            frm.Show(this);
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
