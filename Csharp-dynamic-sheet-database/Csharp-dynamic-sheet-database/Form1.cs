using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace FinalProjectSamuelKinaitisCSCI234
{
    public partial class Form1 : Form
    {
            //varibles 
        //Keeps track of the current row the user is on
        int index;
        //delete log
        int lastDeleted;

        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database3DataSet.Table' table. You can move, or remove it, as needed.
            this.tableTableAdapter.Fill(this.database3DataSet.Table);
        }


        //Menu - File -- Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //Form -- Exit
        private void cmdExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Menu - File -- New (goes to fresh row)
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0];
        }

        //Menu - File -- Delete (hides selected row)
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Purge last deleted
            dataGridView1.Rows.RemoveAt(lastDeleted);
            //set as next deleted
            lastDeleted = index;
            
            //Move to fresh row
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0];
            //Hide requested row
            dataGridView1.Rows[index].Visible = false;
        }

        //Menu - File -- Undelete (unhides last hidden row)
        private void undeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows[lastDeleted].Visible = true;
        }

        //Keeps track of the current row the user is on
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
        }

        //Menu - File -- Purge (Deletes row)
        private void purgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(index);
        }

        //Menu - File -- Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        //Menu - Edit -- Save 
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            save();
        }

        //Save method (Saves DataGrid to database) 
        private void save()
        {
            SqlConnection conection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database3.mdf;Integrated Security=True");
            conection.Open();
            SqlCommand cmd = new SqlCommand("insert into Table values (@ADD_UID,@ADD_DTM,@CHG_UID,@CHG_DTM,@STAT_CD)", conection);
            cmd.Parameters.AddWithValue("@ADD_UID", dataGridView1.Rows[index].Cells[0].Value);
            cmd.Parameters.AddWithValue("@ADD_DTM", dataGridView1.Rows[index].Cells[1].Value);
            cmd.Parameters.AddWithValue("@CHG_UID", dataGridView1.Rows[index].Cells[2].Value);
            cmd.Parameters.AddWithValue("@CHG_DTM", dataGridView1.Rows[index].Cells[3].Value);
            cmd.Parameters.AddWithValue("@STAT_CD", dataGridView1.Rows[index].Cells[4].Value);
            conection.Close();
            MessageBox.Show("Saved");
        }

        //Menu - Help -- About (instructions)
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("File" +
                "\n - New: Goes to a fresh row" +
                "\n - Open: Does nothing" +
                "\n - Save: Saves selected row" +
                "\n - Delete: Removes row" +
                "\n - Undelete: Brings last removed row" +
                "\n - Purge: Deletes row" +
                "\n - Exit: Closes program" +
                "\n" +
                "\nEdit" +
                "\n - Save: Saves selected row" +
                "\n" +
                "\nHelp" +
                "\n - About: Shows this message" +
                "\n" +
                "\nSearch Bar" +
                "\nSearch from STAT_CD");
        }

        //Search Bar - Searches and bring up only the results in the data grid
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = database3DataSet.Table;
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.Format("STAT_CD like '%" + txtSearch.Text + "%'");
            dataGridView1.DataSource = dv.ToTable();
        }


    }
}
