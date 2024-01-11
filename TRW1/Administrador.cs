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
using System.Diagnostics;
namespace TRW1





{
    public partial class Administrador : Form

    {

         //Conexion SQL
        SqlConnection con;
        DirConexion dirCon = new DirConexion();
        SqlDataAdapter adap = new SqlDataAdapter();
        DataSet ds = new DataSet();
        SqlDataAdapter adap1 = new SqlDataAdapter();
        DataSet ds1 = new DataSet();

        public string arquitectura;

        public int primera = 0;
        public string _Mensaje;
        public string Usuario;
        public Pedido PedidosCompra = new Pedido();


        public Administrador()
        {
            InitializeComponent();
            this.toolTip1.SetToolTip(this.button1, "EFECTIVO");
            this.toolTip1.SetToolTip(this.button2, "TARJETA");
            this.toolTip1.SetToolTip(this.button3, "AMEX");
            con = dirCon.crearConexion();
            CargarTKS();
            CargarIND();
            CargarART();

            CargaSalas();
            //CargaSalas(2);
            Procesador();

        }


   
   


        private void Administrador_Load(object sender, EventArgs e)
        {

            if (Usuario == "1")
            {

                
                button13.Enabled = false;
            }
            if (Usuario == "3")
            {

                button13.Enabled = false;
                this.tabControl1.TabPages.Remove(this.TOTALES);
                this.tabControl1.TabPages.Remove(this.MESAS);
                this.tabControl1.TabPages.Remove(this.SUBTOTALES);
                this.tabControl1.TabPages.Remove(this.tabPage1);
                this.tabControl1.TabPages.Remove(this.tabPage2);
                this.tabControl1.TabPages.Remove(this.tabPage3);
                this.tabControl1.TabPages.Remove(this.tabPage4);
                this.tabControl1.TabPages.Remove(this.tabPage5);
                this.tabControl1.TabPages.Remove(this.tabPage7);

            }

            label23.Text = DateTime.Now.ToString("dd/MM/yyyy/HH:mm:tt");
            label20.Text = _Mensaje;
            timer1.Enabled = true;   
            //con.Open();
            SqlDataAdapter datos = new SqlDataAdapter("select * from TAUDITORIA ORDER BY ID DESC ", con);
            DataSet data = new DataSet();

            datos.Fill(data);
            dataGridViewAuditoria.DataSource = data.Tables[0];
            dataGridViewAuditoria.AutoResizeColumns();
            dataGridViewAuditoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            con.Close();
            textBoxUltimoZ.Text = consultazeta();
            
            
            
            

            
   
        }



        //CONSULTA ULTIMO COSRTE Z
        public string consultazeta()
        {
            
            con.Open();
            string query = "SELECT MAX(NUMERO) AS NUMERO FROM ARQUEOS";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            if (reg.Read())
            {
                String regis = reg["NUMERO"].ToString();
                con.Close();
                return regis;
                
            }

            else
            {
                con.Close();
                return "Null";

            }

        }





        private void Administrador_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Show();
            this.Owner.Hide();
            this.Owner.Show();
            // MessageBox.Show("Al cerrar esta ventana, tu secion habra terminado", "Finalizar Secion", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

            textBox2.Text = "EFECTIVO";
            textBox3.Text = "1"; 
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox2.Clear();

            textBox2.Text = "TARJETA";
            textBox3.Text = "2"; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

            textBox2.Text = "AMEX";
            textBox3.Text = "5"; 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ESTE LOGIN Y SUS MOVIMIENTOS, SE REGISTRARON EN LA BITACORA, PARA UNA FUTURA AUDITORIA", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Application.Exit();
        }

      
        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            AccionMesa(label20.Text, label23.Text, CBMOrigenM.Text);
            Administrador_Load(sender, e);


            
            CambioMesa(CBMOrigenS.Text, CBMOrigenM.Text, CBMDestinoS.Text, CBMDestinoM.Text);
           


            Administrador_Load(sender, e);

           

            


        }
        private void CargaSalas(/*int sal*/) {
            
            DataTable tb1 = new DataTable();
            
            con.Open();
            SqlDataAdapter da1 = new SqlDataAdapter("SELECT SALA FROM SALAS ", con);
            da1.Fill(tb1);
            DataTable tb2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT SALA FROM SALAS ", con);
            da2.Fill(tb2);
            DataTable tb3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter("SELECT SALA FROM SALAS ", con);
            da3.Fill(tb3);

            //if (sal == 1) {
            CBMDestinoS.DisplayMember = "SALA";
                CBMDestinoS.ValueMember = "SALA";
                CBMDestinoS.DataSource = tb1;
                //con.Close();
                
            //}
            //if (sal == 2) {
                CBMOrigenS.DisplayMember = "SALA";
                CBMOrigenS.ValueMember = "SALA";
                CBMOrigenS.DataSource = tb2;

            CBSubtotalS.DisplayMember = "SALA";
            CBSubtotalS.ValueMember = "SALA";
            CBSubtotalS.DataSource = tb3;

            con.Close();
            CargaMesas(1);
            CargaMesas(2);
            CargaMesas(3);
            //}
            primera = 1;
            CargaProveedor();
           
        }

        private void CargaMesas(int Combx)
        {

            DataTable tb = new DataTable();
            con.Open();
            SqlDataAdapter da; 
            

            if (Combx == 1) {
                da = new SqlDataAdapter("SELECT NUMMESA FROM CONFIGSALA WHERE (NUMMESA <> 0) and (SALA = '" + CBMDestinoS.Text + "') ORDER BY NUMMESA", con);
                da.Fill(tb);
                CBMDestinoM.DisplayMember = "NUMMESA";
                CBMDestinoM.ValueMember = "NUMMESA";
                CBMDestinoM.DataSource = tb;
                
            }
            if(Combx == 2){
                da = new SqlDataAdapter("SELECT NUMMESA FROM CONFIGSALA WHERE (NUMMESA <> 0) and (SALA = '" + CBMOrigenS.Text + "') ORDER BY NUMMESA", con);
                da.Fill(tb);
                CBMOrigenM.DisplayMember = "NUMMESA";
                CBMOrigenM.ValueMember = "NUMMESA";
                CBMOrigenM.DataSource = tb;
                
            }
            if (Combx == 3)
            {
                da = new SqlDataAdapter("SELECT NUMMESA FROM CONFIGSALA WHERE (NUMMESA <> 0) and (SALA = '" + CBSubtotalS.Text + "') ORDER BY NUMMESA", con);
                da.Fill(tb);
                CBSubtotalM.DisplayMember = "NUMMESA";
                CBSubtotalM.ValueMember = "NUMMESA";
                CBSubtotalM.DataSource = tb;

            }
            con.Close();

        }

        private void CargaProveedor()
        {

            DataTable tb = new DataTable();
            con.Open();
            SqlDataAdapter da;



                da = new SqlDataAdapter("SELECT   CODPROVEEDOR, NOMPROVEEDOR FROM  PROVEEDORES WHERE (DESCATALOGADO = N'F') AND (APTOCORREOS <> N'T' OR APTOCORREOS IS NULL)", con);
                da.Fill(tb);
                CBProveedor.DisplayMember = "CODPROVEEDOR";
                CBProveedor.ValueMember = "NOMPROVEEDOR";
                CBProveedor.DataSource = tb;


            con.Close();

        }

        private void CargarTKS() {

            con.Open();
            SqlDataAdapter datos = new SqlDataAdapter("SELECT   dbo.TIQUETSPAG.SERIE, dbo.TIQUETSPAG.NUMERO FROM            dbo.TIQUETSPAG INNER JOIN dbo.TIQUETSCAB ON dbo.TIQUETSPAG.SERIE = dbo.TIQUETSCAB.SERIE AND dbo.TIQUETSPAG.NUMERO = dbo.TIQUETSCAB.NUMERO INNER JOIN dbo.CLIENTES ON dbo.TIQUETSCAB.CODCLIENTE = dbo.CLIENTES.CODCLIENTE INNER JOIN dbo.FORMASPAGO ON dbo.TIQUETSPAG.CODFORMAPAGO = dbo.FORMASPAGO.CODFORMAPAGO INNER JOIN dbo.TIPOSERVICIOSDELIVERY ON dbo.TIQUETSCAB.TIPODELIVERY = dbo.TIPOSERVICIOSDELIVERY.TIPO WHERE (TIQUETSCAB.Z = 0) AND (TIQUETSPAG.SERIE <> N'I001')", con);
            DataSet data = new DataSet();

            datos.Fill(data);
            if (data.Tables[0].Rows.Count > 0 ) {
                DGWTicket.Enabled = true;
                DGWTicket.DataSource = data.Tables[0];
                con.Close();
            }
            else { DGWTicket.Enabled = false; con.Close(); }
        }
        private void CargarRKG()
        {

            con.Open();
            //SqlDataAdapter datos = new SqlDataAdapter("SELECT  TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION, SUM(TIQUETSLIN.UNIDADES) AS UDS FROM TIQUETSLIN INNER JOIN ARTICULOSCAMPOSLIBRES ON TIQUETSLIN.CODARTICULO = ARTICULOSCAMPOSLIBRES.CODARTICULO WHERE (TIQUETSLIN.HORA BETWEEN CONVERT(DATETIME, '"+dtpInicio.Value.ToString("yyyy-MM-dd")+ " 00:00:00', 102) AND CONVERT(DATETIME, '"+dtpFinal.Value.ToString("yyyy-MM-dd") + " 23:59:59', 102)) AND (ARTICULOSCAMPOSLIBRES.INDUCIDA = 'T') GROUP BY TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION ORDER BY UDS DESC", con);
            SqlDataAdapter datos = new SqlDataAdapter("SELECT TOP(5) VENDEDORES.NOMBREVENDEDOR AS VENDEDOR, ROUND(SUM(TIQUETSLIN.TOTAL), 2) AS IMPORTE, SUM(TIQUETSLIN.UNIDADES) AS UDS FROM TIQUETSLIN INNER JOIN ARTICULOSCAMPOSLIBRES ON TIQUETSLIN.CODARTICULO = ARTICULOSCAMPOSLIBRES.CODARTICULO INNER JOIN VENDEDORES ON TIQUETSLIN.CODVENDEDOR = VENDEDORES.CODVENDEDOR WHERE  (TIQUETSLIN.HORA BETWEEN CONVERT(DATETIME, '" + dtpInicio.Value.ToString("yyyy-MM-dd") + " 00:00:00', 102) AND CONVERT(DATETIME, '" + dtpFinal.Value.ToString("yyyy-MM-dd") + " 23:59:59', 102)) AND (ARTICULOSCAMPOSLIBRES.INDUCIDA = 'T') GROUP BY TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION, VENDEDORES.NOMBREVENDEDOR ORDER BY IMPORTE DESC", con);
            DataSet data = new DataSet();

            datos.Fill(data);
            if (data.Tables[0].Rows.Count > 0)
            {
                DGWRanking.Enabled = true;
                DGWRanking.DataSource = data.Tables[0];
                con.Close();
            }
            else { DGWRanking.Enabled = false; con.Close(); }
        }

        DataTable dirPedidos = new DataTable();
        private void CargarPedidos()
        {

            con.Open();
            //SqlDataAdapter datos = new SqlDataAdapter("SELECT  TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION, SUM(TIQUETSLIN.UNIDADES) AS UDS FROM TIQUETSLIN INNER JOIN ARTICULOSCAMPOSLIBRES ON TIQUETSLIN.CODARTICULO = ARTICULOSCAMPOSLIBRES.CODARTICULO WHERE (TIQUETSLIN.HORA BETWEEN CONVERT(DATETIME, '"+dtpInicio.Value.ToString("yyyy-MM-dd")+ " 00:00:00', 102) AND CONVERT(DATETIME, '"+dtpFinal.Value.ToString("yyyy-MM-dd") + " 23:59:59', 102)) AND (ARTICULOSCAMPOSLIBRES.INDUCIDA = 'T') GROUP BY TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION ORDER BY UDS DESC", con);
            SqlDataAdapter datos = new SqlDataAdapter("SELECT   PEDCOMPRACAB.NUMSERIE AS SERIE, PEDCOMPRACAB.NUMPEDIDO AS PEDIDO, PROVEEDORES.NOMPROVEEDOR AS PROVEEDOR, PEDCOMPRACAB.FECHAPEDIDO AS FECHA, FORMAT(PEDCOMPRACAB.TOTNETO,'C','En-Us') AS TOTAL_NETO, CIF AS RFC FROM  PEDCOMPRACAB INNER JOIN PROVEEDORES ON PEDCOMPRACAB.CODPROVEEDOR = PROVEEDORES.CODPROVEEDOR WHERE (PEDCOMPRACAB.TODORECIBIDO = N'F') AND (PEDCOMPRACAB.FECHAPEDIDO BETWEEN CONVERT(DATETIME, '" + dateTimePickerPI.Value.ToString("yyyy-MM-dd")+ "', 102) AND CONVERT(DATETIME, '" + dateTimePickerPF.Value.ToString("yyyy-MM-dd") + "', 102)) AND (PROVEEDORES.NOMPROVEEDOR = '"+labelNomProveedor.Text+"') ORDER BY PEDCOMPRACAB.FECHAPEDIDO DESC", con);
            //SqlDataAdapter datos = new SqlDataAdapter("SELECT   PEDCOMPRACAB.NUMSERIE AS SERIE, PEDCOMPRACAB.NUMPEDIDO AS PEDIDO, PROVEEDORES.NOMPROVEEDOR AS PROVEEDOR, PEDCOMPRACAB.FECHAPEDIDO AS FECHA, FORMAT(PEDCOMPRACAB.TOTNETO,'C','En-Us') AS TOTAL_NETO, CIF AS RFC FROM  PEDCOMPRACAB INNER JOIN PROVEEDORES ON PEDCOMPRACAB.CODPROVEEDOR = PROVEEDORES.CODPROVEEDOR WHERE (PEDCOMPRACAB.TODORECIBIDO = N'F') AND (PEDCOMPRACAB.FECHAPEDIDO > CONVERT(DATETIME, '" + dateTimePickerPI.Value.ToString("yyyy-MM-dd") + "', 102)) ORDER BY PEDCOMPRACAB.FECHAPEDIDO DESC", con);
            DataSet data = new DataSet();

            datos.Fill(data);
            if (data.Tables[0].Rows.Count > 0)
            {
                DGWPedidos.Enabled = true;
                dirPedidos = data.Tables[0];
                DGWPedidos.DataSource = dirPedidos;
                con.Close();
            }
            else { DGWPedidos.Enabled = false; con.Close(); }
        }
        public class Pedido {
            public string serie;
            public int numero;
            public string total;
            public DateTime fecha;
            public string proveedor;
            public string RFC;
        }
        private void CargarIND()
        {

            con.Open();
            //SqlDataAdapter datos = new SqlDataAdapter("SELECT  TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION, SUM(TIQUETSLIN.UNIDADES) AS UDS FROM TIQUETSLIN INNER JOIN ARTICULOSCAMPOSLIBRES ON TIQUETSLIN.CODARTICULO = ARTICULOSCAMPOSLIBRES.CODARTICULO WHERE (TIQUETSLIN.HORA BETWEEN CONVERT(DATETIME, '"+dtpInicio.Value.ToString("yyyy-MM-dd")+ " 00:00:00', 102) AND CONVERT(DATETIME, '"+dtpFinal.Value.ToString("yyyy-MM-dd") + " 23:59:59', 102)) AND (ARTICULOSCAMPOSLIBRES.INDUCIDA = 'T') GROUP BY TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION ORDER BY UDS DESC", con);
            SqlDataAdapter datos = new SqlDataAdapter("SELECT  ARTICULOS.DESCRIPCION AS ARTICULOS FROM ARTICULOS INNER JOIN ARTICULOSCAMPOSLIBRES ON ARTICULOS.CODARTICULO = ARTICULOSCAMPOSLIBRES.CODARTICULO WHERE  (ARTICULOSCAMPOSLIBRES.INDUCIDA = N'T')", con);
            DataSet data = new DataSet();

            datos.Fill(data);
            if (data.Tables[0].Rows.Count > 0)
            {
                DGWInducida.Enabled = true;
                DGWInducida.DataSource = data.Tables[0];
                con.Close();
            }
            else { DGWInducida.Enabled = false; con.Close(); }
        }


        private void AccionMesa(string Nombre, string Fecha,string Mesa)
{

    con.Open();
    SqlDataAdapter query = new SqlDataAdapter();
    query.InsertCommand = new SqlCommand("INSERT INTO TAUDITORIA (Nombre,Fecha,Accion,Ticket_Mesa) VALUES (@Nombre, @Fecha,'CAMBIO DE MESA',@Mesa)", con);



    query.InsertCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = Nombre;
    query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;
    query.InsertCommand.Parameters.Add("@Mesa", SqlDbType.NVarChar).Value = Mesa;
    query.InsertCommand.ExecuteNonQuery();
    con.Close();

}



private void Cambio(string SOrigen, string MOrigen, string SDestino, string MDestino)
{
    con.Open();


    SqlDataAdapter query = new SqlDataAdapter();
    query.UpdateCommand = new SqlCommand("UPDATE MINUTASCAB SET Mesa = @MesaDestino,  Sala = @SalaDestino WHERE Mesa = @MesaOrigen and  Sala = @SalaOrigen  ", con);

    query.UpdateCommand.Parameters.Add("@SalaOrigen", SqlDbType.Int).Value = SOrigen;
    query.UpdateCommand.Parameters.Add("@MesaOrigen", SqlDbType.Int).Value = MOrigen;
    query.UpdateCommand.Parameters.Add("@SalaDestino", SqlDbType.Int).Value = SDestino;
    query.UpdateCommand.Parameters.Add("@MesaDestino", SqlDbType.Int).Value = MDestino;


    try
    {

        query.UpdateCommand.ExecuteNonQuery();
        MessageBox.Show("Movimiento de mesa TOTAL exitoso...", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        con.Close();

        

        // Application.Exit();

    }
    catch
    {
        con.Close();
        MessageBox.Show("Error ");
    }


}

        private void CambioMesa(string SalaOrigen, string MesaOrigen, string SalaDestino, string MesaDestino)
        {
            DialogResult ms;

            ms = MessageBox.Show("Confirmas cambio?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ms == DialogResult.Yes)
            {
                if (SalaDestino != "0" && SalaOrigen != "0" && MesaDestino != "0" && MesaOrigen != "0")
                {

                    SqlDataAdapter consulta = new SqlDataAdapter();
                    DataSet datos = new DataSet();
                    consulta.SelectCommand = new SqlCommand("select	CONSUBTOTAL from MINUTASCAB  where MESA = '" + MesaOrigen + "'", con);

                    try
                    {
                        consulta.Fill(datos);


                        string Subtotal = datos.Tables[0].Rows[0][0].ToString();

                        if (Subtotal == "F" || datos.Tables[0].Rows.Count > 0)
                        {


                            string Ocupada;
                            SqlDataAdapter consul = new SqlDataAdapter();
                            DataSet datos1 = new DataSet();
                            consul.SelectCommand = new SqlCommand("SELECT SALA, MESA,CASE WHEN  MESA != NULL THEN 'F' WHEN  SALA != NULL THEN 'F' ELSE 'T' END AS OCUPADA FROM MINUTASCAB WHERE SALA = '" + SalaDestino + "' and MESA = '" + MesaDestino + "' ", con);
                            try
                            {
                                consul.Fill(datos1);
                                Ocupada = datos1.Tables[0].Rows[0][2].ToString();
                                //MessageBox.Show("Error " + Ocupada.ToString());
                                if (Ocupada == "T")
                                {
                                    MessageBox.Show("No se Puede Realizar la operacion, la mesa destino esta ocupada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            catch
                            {
                                Ocupada = "F";
                                //MessageBox.Show("Error " + Ocupada.ToString());
                                Cambio(SalaOrigen, MesaOrigen, SalaDestino, MesaDestino);

                            }








                        }
                        else
                        {
                            MessageBox.Show("No se Puede Realizar la operacion con mesa en Subtotal o Vacia", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    catch {
                        MessageBox.Show("No se Puede Realizar la operacion, la mesa origen esta vacia", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                else
                {


                    if (ms == DialogResult.No)
                    {



                        MessageBox.Show("Verifica las MESAS/SALAS antes del cambio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                    }
                    else
                    {
                        MessageBox.Show("No se admite campos en 0", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {


            //Administrador_Load(sender, e);
            //Subtotal(numericUpDown3.Text);
            //Administrador_Load(sender, e);
            if (int.Parse(CBSubtotalM.Text) != 0)
            {
                DialogResult ms;

                ms = MessageBox.Show("CONFIRMAS EL CAMBIO? SE REINICIARA EL FRONT", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ms == DialogResult.Yes)
                {
                    timerCierraP.Stop();
                    AccionLiberar(label20.Text, label23.Text, CBSubtotalM.Text);
                    MessageBox.Show("TENDRAS 2 MINUTO PARA REALIZAR EL AJUSTE", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Subtotal fmr = new Subtotal();
                    fmr.Show();
                    this.Hide();

                }
                else
                {

                    if (ms == DialogResult.No)
                    {

                        MessageBox.Show("OPERACION CANCELADA.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
            else { MessageBox.Show("INGRESA LA MESA", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            Administrador_Load(sender, e);

        }

        private void AccionLiberar(string Nombre, string Fecha, string Mesa)
        {

            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.InsertCommand = new SqlCommand("INSERT INTO TAUDITORIA (Nombre,Fecha,Accion,Ticket_Mesa) VALUES (@Nombre, @Fecha,'LIBERAR SUBTOTAL',@Mesa)", con);



            query.InsertCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = Nombre;
            query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;
            query.InsertCommand.Parameters.Add("@Mesa", SqlDbType.NVarChar).Value = Mesa;
            query.InsertCommand.ExecuteNonQuery();
            con.Close();
            
        }




        private void Subtotal(string Mesa)
        {
            DialogResult ms;

            ms = MessageBox.Show("Confirmas cambio?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ms == DialogResult.Yes)
            {




                con.Open();
                SqlDataAdapter query = new SqlDataAdapter();
                query.UpdateCommand = new SqlCommand("UPDATE MINUTASCAB SET CONSUBTOTAL = 'F' WHERE Mesa = @Mesa ", con);

                query.UpdateCommand.Parameters.Add("@Mesa", SqlDbType.NVarChar).Value = Mesa;
                


                try
                {

                    query.UpdateCommand.ExecuteNonQuery();
                    MessageBox.Show("Mesa LIBERADA con exito", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();

                    
                  //  Application.Exit();

                }
                catch
                {
                    con.Close();
                    MessageBox.Show("Error");
                }
            }
            else
            {

                if (ms == DialogResult.No)
                {

                    MessageBox.Show("Verifica la MESA a liberar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                




                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {




              if (textBox1.Text != "" && textBox2.Text != "")
            {
                con.Open();
                string query = "SELECT Z FROM TIQUETSCAB WHERE (SERIE = @serie) AND (NUMERO = @ticket)";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@serie", textBox15.Text);
                command.Parameters.AddWithValue("@ticket", textBox1.Text);
                int zeta = Convert.ToInt32(command.ExecuteScalar());
                if(zeta==0){
                        con.Close();
                        con.Open();
                        string query1 = "SELECT TOTALNETO FROM TIQUETSCAB WHERE (SERIE = @serie) AND (NUMERO = @ticket)";
                        SqlCommand command1 = new SqlCommand(query1, con);
                        command1.Parameters.AddWithValue("@serie", textBox15.Text);
                        command1.Parameters.AddWithValue("@ticket", textBox1.Text);
                        int reg = Convert.ToInt32(command1.ExecuteScalar());
                        if (reg > 0)
                         {
                         con.Close();
                         AccionREM(textBoxUltimoZ.Text, textBox15.Text, textBox1.Text);
                         Administrador_Load(sender, e);

                         AccionPago(label20.Text, label23.Text, textBox1.Text, textBox15.Text);
                         Administrador_Load(sender, e);

                         TOTAL(textBox1.Text, textBox3.Text, textBox15.Text);
                         Administrador_Load(sender, e);

                         }

                         else
                         {
                             MessageBox.Show("Verifica Serie/Ticket ya que no se encontro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                             con.Close();
                         }
                  
                  
                  }
  
                  else{
                      MessageBox.Show("Ya se realizo el Z, no puedes modificar despues del Z.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                  }
                con.Close();

            }
            else{
                MessageBox.Show("Verifica que todos los campos esten completos");
           
            }

        }
    


        private void AccionPago(string Nombre, string Fecha, string Ticket, string Serie)
        {


            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.InsertCommand = new SqlCommand("INSERT INTO TAUDITORIA (Nombre,Fecha,Accion,Ticket_Mesa) VALUES (@Nombre, @Fecha,'CAMBIO DE FORMA DE PAGO',@Ticket)", con);



            query.InsertCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = Nombre;
            query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;
            query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.NVarChar).Value = Ticket;
            query.InsertCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = Serie;
            query.InsertCommand.ExecuteNonQuery();
            con.Close();
            
            
        }

        private void AccionREM(string z, string Serie, string Ticket)
        {

              int intzeta = int.Parse(z)+1;
              int intticket = int.Parse(Ticket);
        //    //inserta en rem transacciones
              con.Open();
              SqlDataAdapter query = new SqlDataAdapter();

              query.InsertCommand = new SqlCommand("INSERT INTO REM_TRANSACCIONES (TERMINAL,CAJA,CAJANUM,Z,TIPO,ACCION,SERIE,NUMERO,N,FECHA,HORA,FO,IDCENTRAL,TALLA,COLOR,CODIGOSTR,SUBTIPO,FECHA2,FECHA3,CAMPOBIT,NUMERO2) VALUES (HOST_NAME(),'','1',@z,'0','1',@Serie,@Ticket,'B',CONVERT(datetime, '30-12-1899',105),CONVERT(datetime, '30-12-1899',105),'0','1','.','.','',NULL,NULL,NULL,NULL,NULL)", con);
              query.InsertCommand.Parameters.Add("@z", SqlDbType.Int).Value = intzeta;
              query.InsertCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = Serie;
              query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.Int).Value = intticket;
              query.InsertCommand.ExecuteNonQuery();
            
       
              con.Close();

        }





        private void TOTAL(string Ticket, string Pago, string Serie)
        {
            
                con.Open();
                SqlDataAdapter query = new SqlDataAdapter();
                query.UpdateCommand = new SqlCommand("UPDATE TIQUETSPAG SET CODFORMAPAGO = @PAGO WHERE NUMERO = @Ticket AND SERIE = @Serie", con);

                query.UpdateCommand.Parameters.Add("@Pago", SqlDbType.NVarChar).Value = Pago;
                query.UpdateCommand.Parameters.Add("@Ticket", SqlDbType.NVarChar).Value = Ticket;
                query.UpdateCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = Serie;



                try
                {

                  query.UpdateCommand.ExecuteNonQuery();
                    MessageBox.Show("Cambio de forma de pago con exito", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox15.Clear();


                }
                catch
                {
                    con.Close();
                    MessageBox.Show("Error");
                }
  
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Numero != "0")
            {
                Busqueda(Serie, Numero);
            }
            else { MessageBox.Show("Selecciona el Tiquet"); }
            
        }

        private void AccionBuscar(string Nombre, string Fecha, string Ticket)
        {
            

            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.InsertCommand = new SqlCommand("INSERT INTO TAUDITORIA (Nombre,Fecha,Accion,Ticket_Mesa) VALUES (@Nombre, @Fecha,'CONSULTA DE TICKET',@Ticket)", con);



            query.InsertCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = Nombre;
            query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;
            query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.NVarChar).Value = Ticket;
            query.InsertCommand.ExecuteNonQuery();
            con.Close();

        }



        private void Busqueda(string TQSerie, string TQNumero)
        {

            con.Close();
            con.Open();// abro conexion.
            string StrComando = "SELECT TOP 1  dbo.TIQUETSPAG.SERIE, dbo.TIQUETSPAG.NUMERO, dbo.TIQUETSCAB.TOTALNETO, dbo.FORMASPAGO.DESCRIPCION, dbo.CLIENTES.NOMBRECOMERCIAL, dbo.TIPOSERVICIOSDELIVERY.DESCRIPCION AS SERVICIO FROM            dbo.TIQUETSPAG INNER JOIN dbo.TIQUETSCAB ON dbo.TIQUETSPAG.SERIE = dbo.TIQUETSCAB.SERIE AND dbo.TIQUETSPAG.NUMERO = dbo.TIQUETSCAB.NUMERO INNER JOIN dbo.CLIENTES ON dbo.TIQUETSCAB.CODCLIENTE = dbo.CLIENTES.CODCLIENTE INNER JOIN dbo.FORMASPAGO ON dbo.TIQUETSPAG.CODFORMAPAGO = dbo.FORMASPAGO.CODFORMAPAGO INNER JOIN dbo.TIPOSERVICIOSDELIVERY ON dbo.TIQUETSCAB.TIPODELIVERY = dbo.TIPOSERVICIOSDELIVERY.TIPO WHERE dbo.TIQUETSPAG.NUMERO ='" + TQNumero + "' AND  dbo.TIQUETSPAG.SERIE = '" + TQSerie + "' ";
            SqlCommand COMAND = new SqlCommand(StrComando, con);
            DataTable tabla = new DataTable(); // El resultado lo guardaremos en una tabla
            SqlDataAdapter AdaptadorTabla = new SqlDataAdapter(StrComando, con); // Usaremos un DataAdapter para leer los datos
                                                                                 //DataSet ds = new DataSet();



            AdaptadorTabla.Fill(tabla);// Llenamos la tabla con los datos leídos


            string SERIE = tabla.Rows[0]["SERIE"].ToString();//guardo informacion en variables
            string NUMERO = tabla.Rows[0]["NUMERO"].ToString();
            string TOTALNETO = tabla.Rows[0]["TOTALNETO"].ToString();
            string DESCRIPCION = tabla.Rows[0]["DESCRIPCION"].ToString();
            string NOMBRECOMERCIAL = tabla.Rows[0]["NOMBRECOMERCIAL"].ToString();
            string SERVICIO = tabla.Rows[0]["SERVICIO"].ToString();



            textBox5.Text = NUMERO;//asigno valores a txt
            textBox6.Text = TOTALNETO;
            textBox7.Text = DESCRIPCION;
            textBox8.Text = NOMBRECOMERCIAL;
            textBox9.Text = SERVICIO;


            con.Close();


        }
       

      
        private void timer1_Tick(object sender, EventArgs e)
        {
            label23.Text = DateTime.Now.ToString("dd/MM/yyyy/HH:mm:tt");
             
        
        }

             private void button9_Click(object sender, EventArgs e)
             {
                 textBox12.Clear();

                 textBox12.Text = "1";
                 comboBox1.Text = "DELIVERY"; 
             }

             private void button10_Click(object sender, EventArgs e)
             {
                 textBox12.Clear();

                 textBox12.Text = "2";
                 comboBox1.Text = "SALON"; 
             }

             private void button11_Click(object sender, EventArgs e)
             {
                 textBox12.Clear();

                 textBox12.Text = "3";
                 comboBox1.Text = "PICK UP"; 
             }

             private void button8_Click(object sender, EventArgs e)
             {

                 if (textBox10.Text != "" && comboBox1.Text != "")
                 {
                     con.Open();
                     string query = "SELECT Z FROM TIQUETSCAB WHERE (SERIE = @serie) AND (NUMERO = @ticket)";
                     SqlCommand command = new SqlCommand(query, con);
                     command.Parameters.AddWithValue("@serie", textBox16.Text);
                     command.Parameters.AddWithValue("@ticket", textBox10.Text);
                     int zeta = Convert.ToInt32(command.ExecuteScalar());
                     if(zeta==0){
                         con.Close();
                         con.Open();
                         string query1 = "SELECT TOTALNETO FROM TIQUETSCAB WHERE (SERIE = @serie) AND (NUMERO = @ticket)";
                         SqlCommand command1 = new SqlCommand(query1, con);
                         command1.Parameters.AddWithValue("@serie", textBox16.Text);
                         command1.Parameters.AddWithValue("@ticket", textBox10.Text);
                         int reg = Convert.ToInt32(command1.ExecuteScalar());
                         if (reg > 0)
                         {
                             con.Close();
                             AccionServicio(label20.Text, label23.Text, textBox10.Text);

                             AccionREM(textBoxUltimoZ.Text, textBox16.Text, textBox10.Text);
                             Administrador_Load(sender, e);
                  
                             Servicio(textBox16.Text,textBox10.Text,textBox12.Text);
                             Administrador_Load(sender, e);

                          }

                         else
                         {
                             MessageBox.Show("Verifica Serie/Ticket ya que no se encontro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                             con.Close();
                         }
                  
                  
                  }
  
                  else{
                      MessageBox.Show("Ya se realizo el Z, no puedes modificar despues del Z.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                  }

                 }
                 else
                 {
                     MessageBox.Show("Verifica que todos los campos esten completos");
                     con.Close();
                 }
         
        

             }








             private void AccionServicio(string Nombre, string Fecha, string Ticket)
             {


                 con.Open();
                 SqlDataAdapter query = new SqlDataAdapter();
                 query.InsertCommand = new SqlCommand("INSERT INTO TAUDITORIA (Nombre,Fecha,Accion,Ticket_Mesa) VALUES (@Nombre, @Fecha,'CAMBIO DE TIPO DE SERVICIO',@Ticket)", con);



                 query.InsertCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = Nombre;
                 query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;
                 query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.NVarChar).Value = Ticket;
                 query.InsertCommand.ExecuteNonQuery();
                 con.Close();

             }



             private void Servicio(string Serie, string Ticket, string Servicio)
             {
                 

                     con.Open();
                     SqlDataAdapter query = new SqlDataAdapter();
                     query.UpdateCommand = new SqlCommand("UPDATE TIQUETSCAB SET TIPODELIVERY = @Servicio WHERE SERIE = @Serie AND  NUMERO = @Ticket   ", con);

                     query.UpdateCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = Serie;
                     query.UpdateCommand.Parameters.Add("@Ticket", SqlDbType.NVarChar).Value = Ticket;
                     query.UpdateCommand.Parameters.Add("@Servicio", SqlDbType.Int).Value = Servicio;






                     try
                     {

                         query.UpdateCommand.ExecuteNonQuery();
                         MessageBox.Show("Cambio de Cliente exitoso", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         con.Close();

                         textBox10.Clear();
                   
                         textBox16.Clear();
                         
             
                         
                       



                         //  Application.Exit();

                     }
                     catch
                     {
                         con.Close();
                         MessageBox.Show("Error");
                     }
                 
             }





             private void button12_Click(object sender, EventArgs e)
             {
                 if (textBox11.Text != "")
                 {
                     con.Open();
                     string query = "SELECT Z FROM TIQUETSCAB WHERE (SERIE = @serie) AND (NUMERO = @ticket)";
                     SqlCommand command = new SqlCommand(query, con);
                     command.Parameters.AddWithValue("@serie", textBox13.Text);
                     command.Parameters.AddWithValue("@ticket", textBox11.Text);
                     int zeta = Convert.ToInt32(command.ExecuteScalar());
                     if (zeta == 0)
                     {
                        con.Close();
                        con.Open();
                        string query1 = "SELECT TOTALNETO FROM TIQUETSCAB WHERE (SERIE = @serie) AND (NUMERO = @ticket)";
                        SqlCommand command1 = new SqlCommand(query1, con);
                        command1.Parameters.AddWithValue("@serie", textBox13.Text);
                        command1.Parameters.AddWithValue("@ticket", textBox11.Text);
                        int reg = Convert.ToInt32(command1.ExecuteScalar());
                        if (reg > 0)
                        {
                            con.Close();
                            AccionCliente(label20.Text, label23.Text, textBox11.Text);
                            Administrador_Load(sender, e);

                            AccionREM(textBoxUltimoZ.Text, textBox13.Text, textBox11.Text);
                            Administrador_Load(sender, e);

                            Cliente(textBox11.Text, numericUpDown6.Text, textBox13.Text);
                            Administrador_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Verifica Serie/Ticket ya que no se encontro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            con.Close();
                        }

                     }
                     else
                     {
                        MessageBox.Show("Ya se realizo el Z, no puedes modificar despues del Z.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                     }


                 }
                 else
                 {
                     MessageBox.Show("Verifica que todos los campos esten completos");
                     con.Close();
                 }
        
             }



             private void AccionCliente(string Nombre, string Fecha, string Ticket)
             {


                 con.Open();
                 SqlDataAdapter query = new SqlDataAdapter();
                 query.InsertCommand = new SqlCommand("INSERT INTO TAUDITORIA (Nombre,Fecha,Accion,Ticket_Mesa) VALUES (@Nombre, @Fecha,'CAMBIO DE CLIENTE',@Ticket)", con);



                 query.InsertCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = Nombre;
                 query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;
                 query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.NVarChar).Value = Ticket;
                 query.InsertCommand.ExecuteNonQuery();
                 con.Close();

             }









             private void  Cliente(string Ticket, string Cliente, string Serie)
             {
                 
                     con.Open();
                     SqlDataAdapter query = new SqlDataAdapter();
                     query.UpdateCommand = new SqlCommand("UPDATE TIQUETSCAB SET CODCLIENTE = @Cliente WHERE NUMERO = @Ticket AND SERIE = @Serie ", con);

                     query.UpdateCommand.Parameters.Add("@Cliente", SqlDbType.Int).Value = Cliente;
                     query.UpdateCommand.Parameters.Add("@Ticket", SqlDbType.NVarChar).Value = Ticket;
                     query.UpdateCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = Serie;



                     try
                     {

                         query.UpdateCommand.ExecuteNonQuery();
                         MessageBox.Show("Cambio de Cliente exitoso", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         con.Close();

                         textBox11.Clear();
                         numericUpDown6.Value = 0;
                         textBox13.Clear();
                         

                       //  Application.Exit();

                     }
                     catch
                     {
                         con.Close();
                         MessageBox.Show("Error");
                     }
                 
             }

             private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
             {
                
                
             }

             private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
             {

                 if (comboBox1.SelectedItem.ToString() == "DELIVERY")
                 {
                     textBox12.Text = "1";
                 }

                 if (comboBox1.SelectedItem.ToString() == "SALON")
                 {
                     textBox12.Text = "2";
                 }

                 if (comboBox1.SelectedItem.ToString() == "PICK UP")
                 {
                     textBox12.Text = "3";
                 }
                
             }

             private void button9_Click_1(object sender, EventArgs e)
             {
                 textBox5.Clear();
                 textBox6.Clear();
                 textBox7.Clear();
                 textBox8.Clear();
                 textBox9.Clear();
                 

             }

             private void timer3_Tick(object sender, EventArgs e)
             {
                 MessageBox.Show("LA APLICACION SOLO PUEDE OPERARSE POR 3 MINUTOS. SE CERRARA EN 10 SEGUNDOS POR SEGURIDAD", "TIME-OUT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
             }

             private void timer2_Tick(object sender, EventArgs e)
             {
                 
                 Application.Exit();
             }

            

             private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

         

            

             private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
             {
               if (Usuario == "2") {
                if (tabControl1.SelectedIndex == 0)
                {
                    MessageBox.Show("UN CAMBIO DE MESA, APLICA PARA TODOS LOS ARTICULOS DE LA MESA", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (tabControl1.SelectedIndex == 1)
                {
                    MessageBox.Show("SELECCIONA LA MESA QUE DECEAS LIBERAR", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                if (tabControl1.SelectedIndex == 2)
                {
                    MessageBox.Show("EL CAMBIO DE FORMA DE PAGO SOLO ES EN CUENTAS QUE NO SON MIXTAS", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (tabControl1.SelectedIndex == 3)
                {
                    MessageBox.Show("EL CAMBIO DE CLIENTE SOLO ES EN CUENTAS QUE NO SON MIXTAS", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (tabControl1.SelectedIndex == 4)
                {
                    MessageBox.Show("EL CAMBIO DE TIPO DE SERVICIO SOLO ES EN CUENTAS QUE NO SON MIXTAS", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               }
             }

             private void button10_Click_1(object sender, EventArgs e)
             {
                 Process.Start("https://forms.monday.com/forms/55a62ea2e603963995180a5e107cb37e");

             }

             private void button11_Click_1(object sender, EventArgs e)
             {
                 Process.Start("https://forms.monday.com/forms/61aed37da42926220d8fb954d248eed9");
             }

             private void button13_Click(object sender, EventArgs e)
             {
                 CreaUsuario frm = new CreaUsuario();


                 frm.Show(this);
                 this.Hide();
                 
             }

        private void TabPage3_Click(object sender, EventArgs e)
        {
            
        }

        public class DataGridViewSeleccion
        {

            public static string GetValorCelda(DataGridView dgv, int num)
            {

                string valor = "";
                try
                {
                    valor = dgv.Rows[dgv.CurrentRow.Index].Cells[num].Value.ToString();
                    return valor;
                }
                catch { return valor; }

            }

        }
        public string Serie="0", Numero="0";

        private void CBMOrigenM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void CBMOrigenS_TextChanged(object sender, EventArgs e)
        {
            if (primera == 1) {
                CargaMesas(2);
            }
        }

        private void CBMDestinoS_TextChanged(object sender, EventArgs e)
        {
            if (primera == 1)
            {
                CargaMesas(1);
            }
        }

        private void CBSubtotalS_TextChanged(object sender, EventArgs e)
        {
            if (primera == 1)
            {
                CargaMesas(3);
            }
        }

        private void btnAceptarR_Click(object sender, EventArgs e)
        {
            CargarRKG();
        }

        private void buttonBuscaP_Click(object sender, EventArgs e)
        {
            dirPedidos.Rows.Clear();
            labelSP.Text = "SERIE";
            labelNP.Text = "NUMERO";
            CargarPedidos();
        }

        private void DGWTicket_Click(object sender, EventArgs e)
        {
            Serie = DataGridViewSeleccion.GetValorCelda(DGWTicket, 0);
            Numero = DataGridViewSeleccion.GetValorCelda(DGWTicket, 1);
            Busqueda(Serie, Numero);

            //    con.Close();
            //    con.Open();
            //    string query1 = "SELECT TOTALNETO FROM TIQUETSCAB WHERE (SERIE = @serie) AND (NUMERO = @ticket)";
            //    SqlCommand command1 = new SqlCommand(query1, con);
            //    command1.Parameters.AddWithValue("@serie", Serie);
            //    command1.Parameters.AddWithValue("@ticket", Numero);
            //    int reg = Convert.ToInt32(command1.ExecuteScalar());
            //    if (reg > 0)
            //    {
            //        con.Close();


            //        //AccionBuscar(label20.Text, label23.Text, DataGridViewSeleccion.GetValorCelda(DGWTicket, 1));
            //        Administrador_Load(sender, e);

            //        Busqueda();
            //        Administrador_Load(sender, e);
            //    }


        }

        private void button9_Click_2(object sender, EventArgs e)
        {
            if (labelNP.Text != "NUMERO")
            {
                PEDIDO frm = new PEDIDO(PedidosCompra, int.Parse(textBoxUltimoZ.Text));
                frm.acceso = Usuario;

                frm.Show(this);
                this.Hide();
            }
            else {

                MessageBox.Show("SELECCIONA UN PEDIDO", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CBProveedor_DisplayMemberChanged(object sender, EventArgs e)
        {
            labelNomProveedor.Text = CBProveedor.SelectedValue.ToString();
        }

        private void labelNomProveedor_TextChanged(object sender, EventArgs e)
        {
            //if (dirPedidos.Rows.Count != 0) { 
            //    dirPedidos.DefaultView.RowFilter = $"PROVEEDOR LIKE '{labelNomProveedor.Text}%'";
            //    labelSP.Text = "SERIE";
            //    labelNP.Text = "NUMERO";
            //}
        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            ejecuta();
        }

        private void DGWPedido_Click(object sender, EventArgs e)
        {
            PedidosCompra.serie = DataGridViewSeleccion.GetValorCelda(DGWPedidos, 0);
            PedidosCompra.numero = int.Parse(DataGridViewSeleccion.GetValorCelda(DGWPedidos, 1));
            PedidosCompra.proveedor = DataGridViewSeleccion.GetValorCelda(DGWPedidos, 2);
            PedidosCompra.fecha = DateTime.Parse(DataGridViewSeleccion.GetValorCelda(DGWPedidos, 3));
            PedidosCompra.total = DataGridViewSeleccion.GetValorCelda(DGWPedidos, 4);
            PedidosCompra.RFC = DataGridViewSeleccion.GetValorCelda(DGWPedidos, 5);
            labelSP.Text = DataGridViewSeleccion.GetValorCelda(DGWPedidos, 0);
            labelNP.Text = DataGridViewSeleccion.GetValorCelda(DGWPedidos, 1);
        }
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

        //////////////////////////////////////////MERMAS
        DataTable articulosData = new DataTable();
        private void CargarART()
        {

            con.Open();
            SqlDataAdapter datos = new SqlDataAdapter("SELECT DESCRIPCION ,CODARTICULO, UNIDADMEDIDA FROM ARTICULOS WHERE (UDSTRASPASO IS NOT NULL) AND (USASTOCKS = N'T') AND (DESCATALOGADO = N'F')", con);
            DataSet data = new DataSet();

            datos.Fill(data);
            if (data.Tables[0].Rows.Count > 0)
            {
                DGArt.Enabled = true;
                articulosData = data.Tables[0];
                DGArt.DataSource = articulosData;
                con.Close();
            }
            else { DGArt.Enabled = false; con.Close(); }
        }

        private void tbxArtDesc_TextChanged(object sender, EventArgs e)
        {
            articulosData.DefaultView.RowFilter = $"DESCRIPCION LIKE '{tbxArtDesc.Text}%'";
        }

        private void DGArt_Click(object sender, EventArgs e)
        {
            lbArtMerma.Text = DataGridViewSeleccion.GetValorCelda(DGArt, 0);
            lbCodArt.Text = DataGridViewSeleccion.GetValorCelda(DGArt, 1);
            lbUnidadMed.Text = DataGridViewSeleccion.GetValorCelda(DGArt, 2);

        }

        string FO;
        string SerieTraspaso;
        string AlmacenMermas;

        public void ConsultaFO()
        {

            con.Open();
            string query = "SELECT  TOP (1) CODFO FROM TERMINALES WHERE (CONECTADO = 1)";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {
                    FO = reg["CODFO"].ToString();
                    

                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion ConsultaFO()");

            }
            

        }

        public void ConsultaAlmacen()
        {
            ConsultaFO();
            con.Open();
            string query = "SELECT CODALMACEN, SERIETRASPASOS FROM ALMACEN WHERE (CODALMACEN LIKE N'%"+FO+"')";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {
                    AlmacenMermas = reg["CODALMACEN"].ToString()+"M";
                    SerieTraspaso = reg["SERIETRASPASOS"].ToString();
                    FO = reg["CODALMACEN"].ToString();


                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion ConsultaAlmacen()");

            }


        }

        string Ref;
        string Desc;
        string UltCost;

        public void ObtenArt()
        {

            con.Open();
            string query = "SELECT CODARTICULO, REFERENCIA, DESCRIPCION, ULTIMOCOSTE FROM ARTICULOS WHERE (CODARTICULO = "+lbCodArt.Text+")";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {
                    Ref = reg["REFERENCIA"].ToString();
                    Desc = reg["DESCRIPCION"].ToString();
                    UltCost = reg["ULTIMOCOSTE"].ToString();

                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion ObtenArt()");

            }


        }
        string NumeroTraspaso;
        public void ObtenNumTraspaso()
        {

            con.Open();
            string query = "SELECT NUMTRASP FROM SERIES WHERE (SERIE = N'"+SerieTraspaso+"')";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {
                    
                    NumeroTraspaso = ""+(int.Parse(reg["NUMTRASP"].ToString())+1);


                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion ObtenNumTraspaso()");

            }


        }

        string stock;
        public void ObtenStock()
        {

            con.Open();
            string query = "SELECT STOCK FROM STOCKS WHERE(CODARTICULO = "+lbCodArt.Text+") AND (CODALMACEN = N'"+FO+"')";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {

                    stock = reg["STOCK"].ToString();


                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion ObtenNumTraspaso()");

            }


        }

        private void AccionTraspaso()
        {
            ConsultaAlmacen();
            ObtenArt();
            ObtenNumTraspaso();
            ObtenStock();
            try
            {
                DateTime dia = DateTime.Now;

            con.Open();
                SqlDataAdapter query = new SqlDataAdapter();
                query.InsertCommand = new SqlCommand("INSERT INTO  TRASPALMACEN(SERIE, NUMERO, LINEA, CODALMORIG, CODALMDEST, FECHA, CODARTICULO, REFERENCIA, DESCRIPCION, UNIDADES, PRECIO, USUARIO, CAJA, Z, STOCK, DESCARGADO, FECHACREACION, IMPRESIONES, ESRECUENTO, OBSERVACIONES)"
                                                                       + "VALUES(@Serie,@Traspaso,0,@Origen,@Destino,@Dia,@CodArt,@Referencia,@Descripcion,@Uds,@Precio,'',1,@Z,@Stock,'T',@Dia,1,0,'')", con);



                query.InsertCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = SerieTraspaso;
                query.InsertCommand.Parameters.Add("@Traspaso", SqlDbType.Int).Value = int.Parse(NumeroTraspaso);
                query.InsertCommand.Parameters.Add("@Origen", SqlDbType.NVarChar).Value = FO;
                query.InsertCommand.Parameters.Add("@Destino", SqlDbType.NVarChar).Value = AlmacenMermas;
                query.InsertCommand.Parameters.Add("@Dia", SqlDbType.DateTime).Value = dia; 
                query.InsertCommand.Parameters.Add("@CodArt", SqlDbType.Int).Value = int.Parse(lbCodArt.Text);
                query.InsertCommand.Parameters.Add("@Referencia", SqlDbType.NVarChar).Value = Ref;
                query.InsertCommand.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = Desc;
                query.InsertCommand.Parameters.Add("@Uds", SqlDbType.Float).Value = float.Parse(txtUdsMermas.Text);
                query.InsertCommand.Parameters.Add("@Precio", SqlDbType.Float).Value = float.Parse(UltCost);
                query.InsertCommand.Parameters.Add("@Z", SqlDbType.Int).Value = int.Parse(textBoxUltimoZ.Text);
                query.InsertCommand.Parameters.Add("@Stock", SqlDbType.Float).Value = float.Parse(stock);




                query.InsertCommand.ExecuteNonQuery();
                con.Close();
                AccionREMTrasp();
                MessageBox.Show("SE GENERO EL REGISTRO CORRECTAMENTE");
                lbArtMerma.Text = "-";
                lbCodArt.Text = "-";
                lbUnidadMed.Text = "-";
                txtUdsMermas.Text = "";
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion AccionTraspaso()");

            }

        }

        private void AccionREMTrasp()
        {
            try
            {

                int intzeta = int.Parse(textBoxUltimoZ.Text) + 1;
                int intticket = int.Parse(NumeroTraspaso);
                //    //inserta en rem transacciones
                con.Open();
                SqlDataAdapter query = new SqlDataAdapter();

                query.InsertCommand = new SqlCommand("INSERT INTO REM_TRANSACCIONES (TERMINAL,CAJA,CAJANUM,Z,TIPO,ACCION,SERIE,NUMERO,N,FECHA,HORA,FO,IDCENTRAL,TALLA,COLOR,CODIGOSTR,SUBTIPO,FECHA2,FECHA3,CAMPOBIT,NUMERO2) VALUES (HOST_NAME(),'','1',@z,'6','0',@Serie,@Ticket,'',CONVERT(datetime, '30-12-1899',105),CONVERT(datetime, '30-12-1899',105),'0','1','.','.','',NULL,NULL,NULL,NULL,NULL)", con);
                query.InsertCommand.Parameters.Add("@z", SqlDbType.Int).Value = intzeta;
                query.InsertCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = SerieTraspaso;
                query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.Int).Value = intticket;
                query.InsertCommand.ExecuteNonQuery();


                con.Close();
                ActualizaSerieTrasp();
                ActualizaStock();
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion AccionREMTrasp()");

            }


        }

        private void btnGuardarMerma_Click(object sender, EventArgs e)
        {
            if (lbCodArt.Text != "-")
            {
                if (txtUdsMermas.Text != null && txtUdsMermas.Text != "")
                {
                    DialogResult ms;

                    ms = MessageBox.Show("CONFIRMAS QUE LA CANTIDAD A MERMAR ES LA CORRECTA?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (ms == DialogResult.Yes)
                    {
                        AccionTraspaso();
                    }
                    else
                    {

                        if (ms == DialogResult.No)
                        {

                            MessageBox.Show("OPERACION CANCELADA.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                    }
                }
                else { MessageBox.Show("ingresa las unidades a mermar"); }
            }
            else { MessageBox.Show("Selecciona el Articulo a Mermar"); }
          

        }

        private void ActualizaSerieTrasp()
        {


            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.UpdateCommand = new SqlCommand("UPDATE SERIES SET NUMTRASP = @numero WHERE (SERIE = N'" + SerieTraspaso + "')", con);

            query.UpdateCommand.Parameters.Add("@numero", SqlDbType.NVarChar).Value = NumeroTraspaso;







            try
            {

                query.UpdateCommand.ExecuteNonQuery();
                //MessageBox.Show("Cambio de Cliente exitoso", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();



            }
            catch
            {
                con.Close();
                MessageBox.Show("Error ActualizaSerieTrasp()");
            }

        }

        private void ActualizaStock()
        {
            float stkfinal;

            stkfinal = float.Parse(stock) - float.Parse(txtUdsMermas.Text);



            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.UpdateCommand = new SqlCommand("UPDATE  STOCKS SET  STOCK = @stock WHERE (CODARTICULO = "+lbCodArt.Text+") AND (CODALMACEN = N'"+FO+"')", con);

            query.UpdateCommand.Parameters.Add("@stock", SqlDbType.Float).Value = stkfinal;







            try
            {

                query.UpdateCommand.ExecuteNonQuery();
                //MessageBox.Show("Cambio de Cliente exitoso", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();



            }
            catch
            {
                con.Close();
                MessageBox.Show("Error ActualizaStock()");
            }

        }

        private void textBoxMermas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

    }
}














            

        







