using System;
using System.Windows.Forms;
using System.IO;

namespace BendSheets
{
    public partial class frmCreateTemplate : Form
    {
        private RecipeTemplates m_RecipeTemplates;
        private BindingSource m_BindingSource;

        public frmCreateTemplate()
        {
            InitializeComponent();
        }

        private void frmCreateTemplate_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.Columns["colVariableName"].DataPropertyName = "Name";
            this.dataGridView1.Columns["colVariableType"].DataPropertyName = "ItemType";
            this.dataGridView1.Columns["colCellStart"].DataPropertyName = "CellStart";
            this.dataGridView1.Columns["colCellEnd"].DataPropertyName = "CellEnd";


            cboVariableType.Items.Add(RecipeTemplateItemType.Cell);
            cboVariableType.Items.Add(RecipeTemplateItemType.Range);
            cboVariableType.SelectedIndex = 0;

            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
        }

        private void openTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridView1.SelectionChanged -= new System.EventHandler(this.dataGridView1_SelectionChanged);

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + @"\Recipe Templates";

            bool exists = Directory.Exists(openFileDialog1.InitialDirectory);

            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                m_RecipeTemplates = new RecipeTemplates();
                m_RecipeTemplates.Load(openFileDialog1.FileName);
                this.lblTemplateLocation.Text = openFileDialog1.FileName;

                m_BindingSource = new BindingSource();
                m_BindingSource.DataSource = m_RecipeTemplates.TemplateList;
                this.dataGridView1.DataSource = m_BindingSource;

                SetEditor();

                this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
                this.dataGridView1.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView1_RowsAdded);

                btnNext.Enabled = m_RecipeTemplates.Count > 1;
            }
        }

        private void saveTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_RecipeTemplates == null)
            {
                return;
            }

            if (m_RecipeTemplates.Count == 0)
            {
                return;
            }

            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                m_RecipeTemplates.Save(saveFileDialog1.FileName);
            }

        }

        private void SetEditor()
        {
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            RecipeTemplateItem item = dataGridView1.SelectedRows[0].DataBoundItem as RecipeTemplateItem;
            if (item == null)
            {
                return;
            }

            this.cboVariableType.SelectedItem = item.ItemType;
            this.txtVariableName.Text = item.Name;
            this.txtCellStart.Text = item.CellStart;
            if (item.ItemType == RecipeTemplateItemType.Range)
            {
                this.txtCellEnd.Clear();
            }
            else
            {
                this.txtCellEnd.Text = item.CellEnd;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            SetEditor();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.dataGridView1.Rows[e.RowIndex].Selected = true;
        }

        private void newItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                RecipeTemplateItem item = dataGridView1.CurrentRow.DataBoundItem as RecipeTemplateItem;
                if (item == null)
                {
                    return;
                }
                m_RecipeTemplates.AddTemplate(new RecipeTemplateItem("Variable Name", RecipeTemplateItemType.Cell, String.Empty, String.Empty));
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {               
                RecipeTemplateItem item = dataGridView1.CurrentRow.DataBoundItem as RecipeTemplateItem;
                if (item == null)
                {
                    return;
                }
                m_RecipeTemplates.DeleteTemplate(item);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (m_RecipeTemplates == null)
            {
                return;
            }

            m_BindingSource.MovePrevious();
            btnNext.Enabled = true;
            btnPrevious.Enabled = m_BindingSource.Position != 0;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (m_RecipeTemplates == null)
            {
                return;
            }

            m_BindingSource.MoveNext();
            btnPrevious.Enabled = true;
            btnNext.Enabled = m_BindingSource.Position != m_RecipeTemplates.Count -1;
        }
    }
}
