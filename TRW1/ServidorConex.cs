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
            //try
            //{

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
                        
                        }
                    
                    }
                    
                }
                XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                ConfigurationManager.RefreshSection("appSettings");
            //}
            //catch
            //{
            //    MessageBox.Show("Conexion no Exitosa " , "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //}
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
    }
}
