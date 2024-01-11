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
using System.Diagnostics;



namespace TRW1
{
    public partial class Login : Form
    {
        //Conexion SQL

      
        SqlConnection con;
        DirConexion dirCon = new DirConexion();

        public Login()
        {
            Procesador();
            if (getPrevInstance())
            {
                this.Close();
                Application.Exit();
                Application.ExitThread();
            }
            else
            {
                InitializeComponent();
                con = dirCon.crearConexion();
            }
        }

        private static bool getPrevInstance()
        {
            //get the name of current process, i,e the process 
            //name of this current application

            string currPrsName = Process.GetCurrentProcess().ProcessName;

            //Get the name of all processes having the 
            //same name as this process name 
            Process[] allProcessWithThisName
                         = Process.GetProcessesByName(currPrsName);

            //if more than one process is running return true.
            //which means already previous instance of the application 
            //is running
            if (allProcessWithThisName.Length > 1)
            {
                MessageBox.Show("YA SE ESTA EJECUTANDO");

                return true; // Yes Previous Instance Exist
            }
            else
            {
                return false; //No Prev Instance Running
            }
        }


        private void Login_Load(object sender, EventArgs e)
        {
            
        }








        private void Verifica(string Nombre, string Fecha)
        {




            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            //query.InsertCommand = new SqlCommand("INSERT INTO TENTRADAS VALUES (@Empleado,@Fecha)", con);


            //query.InsertCommand.Parameters.Add("@Empleado", SqlDbType.NVarChar).Value = Empleado;
            //query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;




            /////////CODIGO BUENO/////

            //query.InsertCommand = new SqlCommand("INSERT INTO TAUDITORIA (Nombre,Fecha,Accion) VALUES (@Nombre, @Fecha,'1')", con);



            //query.InsertCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = Nombre;
            //query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Fecha;
            //query.InsertCommand.ExecuteNonQuery();








            SqlDataAdapter consulta = new SqlDataAdapter();
            DataSet datos = new DataSet();
            consulta.SelectCommand = new SqlCommand("select	* from TUsuarios where Nombre = '" + TextContraseña.Text + "' and Contraseña = '" + TextUsuario.Text + "'", con);
            consulta.Fill(datos);


            try
            {

                string acesso, status, Usuario;
                acesso = datos.Tables[0].Rows[0][3].ToString();
                status = datos.Tables[0].Rows[0][4].ToString();
                Usuario = datos.Tables[0].Rows[0][1].ToString();//recupero el nombre de mi tabla usuario
                // codigo para asignar foto en variable...

                //Foto = datos.Tables[0].Rows[0][4].ToString();

                con.Close();
                if (status == "1")
                {
                    if (acesso == "1")
                    {






                        MessageBox.Show("\nHas Iniciando Sesion como Supervisor", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Administrador frm = new Administrador();
                        frm._Mensaje = TextContraseña.Text;
                        frm.Usuario = acesso;

                        frm.Show(this);
                        this.Hide();
                        Limpiar();
                    }
                    else
                    {
                        if (acesso == "2")
                        {
                            MessageBox.Show("\nHas Iniciando Sesion como Sistemas", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Administrador frm = new Administrador();
                            frm._Mensaje = TextContraseña.Text;
                            frm.Usuario = acesso;

                            frm.Show(this);
                            this.Hide();
                            Limpiar();
                        }
                        if (acesso == "3")
                        {
                            MessageBox.Show("\nHas Iniciando Sesion como CAJERO", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Administrador frm = new Administrador();
                            frm._Mensaje = TextContraseña.Text;
                            frm.Usuario = acesso;

                            frm.Show(this);
                            this.Hide();
                            Limpiar();
                        }


                    }
                }
                else
                {
                    MessageBox.Show("Estas Dado de baja contacte a su  Administador", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Limpiar();
                }

            }
            catch
            {
                con.Close();
                MessageBox.Show("Verifica el Usuario o la Contraseña", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                Limpiar();
            }
        }

        //Al momento de limpiar y si es un combobox dinamico se deve voler a cargar este si no ya n lo carga de nuevo despues de limpiar
        private void Limpiar()
        {

            TextUsuario.Clear();
            TextContraseña.Clear();

        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            Verifica(TextContraseña.Text, TextUsuario.Text);
            Login_Load(sender, e);



        }

        //Codigo Para que precionando enter me mande un evento, recordar poner en el textbox el evento  TxtPruebaENTER_KeyPress 

        private void TxtPruebaENTER_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                //este es el evento que quieres que haga al dar enter
                Verifica(TextContraseña.Text, TextUsuario.Text);
                //este codigo es para que precionando enter me haga tabulador y me pocicione en el siguiente text o boton o lo uqe siga, puedes quitar  el evento anterior Verifica(); para probar..y descomentar el de abajo...
                //SendKeys.Send("{TAB}");   

               
            }

        }



        //Codigo para que solo asepte Numeros Recordar agregar el evento keypress al textbox que queramos
        private void txtPruebaNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
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



        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();

        }

        private void comboBoxNombre_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {


            
            //Application.Restart();
          
            this.Close();
            //this.menuStrip.Enable = false;
        }



        private void buttonAceptar_MouseEnter(object sender, EventArgs e)
        {
            buttonAceptar.BackgroundImage = null;
            buttonAceptar.BackgroundImage = Properties.Resources.MB_0008_touch_copia;
        }

        private void buttonAceptar_MouseLeave(object sender, EventArgs e)
        {
            buttonAceptar.BackgroundImage = null;
            buttonAceptar.BackgroundImage = Properties.Resources.MB_0008_touch;
        }

        //private void buttonLimpiar_MouseEnter(object sender, EventArgs e)
        //{
        //    buttonLimpiar.BackgroundImage = null;
        //    buttonLimpiar.BackgroundImage = Properties.Resources.MB_0015_reload2;
        //}

        //private void buttonLimpiar_MouseLeave(object sender, EventArgs e)
        //{
        //    buttonLimpiar.BackgroundImage = null;
        //    buttonLimpiar.BackgroundImage = Properties.Resources.MB_0015_reload2blanco;
        //}

       

        private void button1_Click(object sender, EventArgs e)
        {
            if (TextContraseña.Text == "SISTEMAS" && TextUsuario.Text == "172106")
            {
                ServidorConex frm = new ServidorConex();


                frm.Show(this);
                this.Hide();
                Limpiar();
            }
            else
            {
                if (ConfigurationManager.AppSettings["status"] == "False")
                {

                    MessageBox.Show("Configura el servidor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                else
                {


                    Verifica(TextContraseña.Text, TextUsuario.Text);

                    Login_Load(sender, e);
                }
            }
            

            


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

     
     

        private void button1_Click_1(object sender, EventArgs e)
        {
            Limpiar();
        }

      

       



          private void TextUsuario_KeyPress(object sender, KeyPressEventArgs e)
          {
              if (e.KeyChar == (char)(Keys.Enter))
              {
                  e.Handled = true;
                  //este es el evento que quieres que haga al dar enter

                  if (TextContraseña.Text == "SISTEMAS" && TextUsuario.Text == "172106")
                  {
                      ServidorConex frm = new ServidorConex();


                      frm.Show(this);
                      this.Hide();
                      Limpiar();
                  }
                  else
                  {
                      if (ConfigurationManager.AppSettings["status"] == "False")
                      {

                          MessageBox.Show("Configura el servidor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                      }
                      else
                      {


                          Verifica(TextContraseña.Text, TextUsuario.Text);

                          Login_Load(sender, e);
                      }
                  }
            

            


                  //este codigo es para que precionando enter me haga tabulador y me pocicione en el siguiente text o boton o lo uqe siga, puedes quitar  el evento anterior Verifica(); para probar..y descomentar el de abajo...
                  //SendKeys.Send("{TAB}");   


              }
          }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnReiniciaFront_Click(object sender, EventArgs e)
        {
            ejecuta();
        }

        public string arquitectura;
        private void ejecuta()
        {



            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            if (arquitectura == "64 bits")
            {
                psi.FileName = "C:\\Windows\\REBELTOOL\\FrontRest64.bat";

            }
            else
            {
                psi.FileName = "C:\\Windows\\REBELTOOL\\FrontRest32.bat";

            }
            Process.Start(psi);

        }
        private void Procesador()
        {
            // La información sobre la arquitectura de 32 o 64 bits se obtiene mejor desde la clave
            // [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment]
            // En la clave PROCESSOR_ARCHITECTURE que puede ser AMD64 o x86
            string Clave2 = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
            //
            Microsoft.Win32.RegistryKey principal = Microsoft.Win32.Registry.LocalMachine; //rama LocalMachine
            Microsoft.Win32.RegistryKey subclave2 = principal.OpenSubKey(Clave2); //clave SYSTEM ...  Environment

            // valor de la cadena PROCESSOR_ARCHITECTURE de Environment
            arquitectura = subclave2.GetValue("PROCESSOR_ARCHITECTURE").ToString();
            if (arquitectura.Equals("AMD64"))
                arquitectura = "64 bits";
            else
                arquitectura = "32 bits";



        }

    }
    }




