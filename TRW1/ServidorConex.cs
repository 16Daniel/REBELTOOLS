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
using System.Configuration;
using System.Xml;

namespace TRW1
{
    public partial class ServidorConex : Form
    {
        SqlConnection con;
        DirConexion dirCon = new DirConexion();
        public ServidorConex()
        {
            InitializeComponent();
        }

        private void ServidorConex_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Conexion = "Data Source =" + textServidor.Text + ";Initial Catalog=" + textBase.Text + ";User Id=" + textUsuario.Text + ";Password=" + textContraseña.Text + "";
            ConfigurationManager.AppSettings["conexion"] = Conexion;
            
            try
            {

                dirCon = new DirConexion();
                con = dirCon.crearConexion();
                SqlDataAdapter query = new SqlDataAdapter();
                SqlDataAdapter consulta = new SqlDataAdapter();
                DataSet datos = new DataSet();
                consulta.SelectCommand = new SqlCommand("select	* from TUsuarios ", con);
                consulta.Fill(datos);
                string Usuario = datos.Tables[0].Rows[0][1].ToString();
                MessageBox.Show("Conexion Exitosa ", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ConfigurationManager.AppSettings["status"] = "True";
                button1.Enabled = true;
                textBase.Enabled = false;
                textServidor.Enabled = false;
                textUsuario.Enabled = false;
                textContraseña.Enabled = false;
                button2.Enabled = false;
               
                
                //

                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                foreach (XmlElement element in XmlDoc.DocumentElement)
                {
                    if(element.Name.Equals("appSettings")){

                        foreach(XmlNode node in element.ChildNodes){

                            if(node.Attributes[0].Value == "status"){
                                node.Attributes[1].Value = "True";
                            }
                            if (node.Attributes[0].Value == "conexion")
                            {
                                node.Attributes[1].Value = Conexion;
                            }
                            if (node.Attributes[0].Value == "minutos")
                            {
                                node.Attributes[1].Value = textMinutos.Text;
                            }

                        }
                    
                    }
                    
                }
                XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                ConfigurationManager.RefreshSection("appSettings");
                creaTabla();
            }
            catch
            {
                MessageBox.Show("Conexion no Exitosa ", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login frm = new Login();

            
            frm.Show(this);
            this.Hide();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login frm = new Login();


            frm.Show(this);
            this.Hide();
        }

        private void creaTabla()
        {

            dirCon = new DirConexion();
            //con = dirCon.crearConexion();
            //SqlDataAdapter query = new SqlDataAdapter();
            //query. = new SqlCommand("USE ["+textBase.Text+"] GO SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO CREATE TABLE [dbo].[TAYC25]([FECHAINI] [dbo].[DDATE] NULL,[SALA] [dbo].[DSMALLINT] NOT NULL,[MESA] [dbo].[DSMALLINT] NOT NULL,[TOTAL_AYC] [int] NULL,[COBROS] [int] NULL,[COBROS_MINIMOS] [int] NULL,[DIFERENCIA] [int] NOT NULL,[JUSTIFICACION] [nvarchar](max) NOT NULL,[USUARIO] [nvarchar](50) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] GO", con);
            try
            {
                using (var conexion = dirCon.crearConexion())
                {
                    conexion.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandText = "CREATE TABLE [TMERMAS]([ID] [int] IDENTITY(1,1) NOT NULL,[FECHA] [dbo].[DDATE] NULL,[SERIE] [nvarchar](4) NOT NULL,[NUMERO] [dbo].[DINTEGER] NOT NULL,[CODARTICULO] [dbo].[DINTEGER] NULL,[REFERENCIA] [nvarchar](15) NULL,[DESCRIPCION] [nvarchar](45) NULL,[UNIDADES] [dbo].[DFLOAT] NULL,[PRECIO] [dbo].[DFLOAT] NULL,[JUSTIFICACION] [nvarchar](max) NOT NULL,[COMENTARIOS] [nvarchar](max) NOT NULL,[USUARIO] [nvarchar](50) NULL,[ENVIADO] [varchar](20) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                        comando.CommandType = CommandType.Text;
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Se Creo Tabla ", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ya existe la Tabla ", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}
