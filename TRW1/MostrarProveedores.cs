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
    public partial class MostrarProveedores : Form
    {

        //Conexion SQL
        //Data Source=Daniel-PC\sqlexpress;Initial Catalog=TRW;Integrated Security=True;Pooling=False
        SqlConnection conexion = new SqlConnection("Data Source=gilberto-pc\\sqlexpress;AttachDbFilename='C:\\Program Files\\Microsoft SQL Server\\MSSQL10.SQLEXPRESS\\MSSQL\\DATA\\TRW.mdf';Integrated Security=True");

        public MostrarProveedores()
        {
            InitializeComponent();
        }

        private void mostrarProveedor_Load(object sender, EventArgs e)
        {
            mostrarProveedor();
        }

        private void mostrarProveedor()
        {


            conexion.Open();
            SqlDataAdapter datos = new SqlDataAdapter("select * from tproveedores where status = 1", conexion);
            DataSet data = new DataSet();

            datos.Fill(data);
            dataGridViewmostrarProveedor.DataSource = data.Tables[0];
            conexion.Close();

        }



        private void dataGridViewmostrarProveedor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
