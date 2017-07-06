namespace PHPMySQLWebSiteCreator
{
  using System;
  using System.Globalization;
  using System.Windows.Forms;

  public partial class FormMain : Form
  {
    // Global variables Definition:
    private bool treeExist = false;
    private bool treeChanged = false;
    private bool allowNodeRenaming = false;
    private Database db1;


    public FormMain()
    {
      this.InitializeComponent();
    }

    private void QuitterToolStripMenuItemClick(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void ToolStripMenuItemGenererClick(object sender, EventArgs e)
    {
      // Creation of pages: PHP, CSS et HTML for the site.

      // will be redirected to index.php
      var index = new Page(FileExtension.HTML, "index");

      // TODO: check if no table or field has same name
      if (this.NoDoubleTableOrFieldName())
      {
        // TODO
      }
    }

    private bool NoDoubleTableOrFieldName()
    {
      // we color double name with yellow color
      // treeView1.SelectedNode.BackColor = Color.Yellow;
      return true; // TODO to implement
    }

    private void NewToolStripMenuItemClick(object sender, EventArgs e)
    {
      // check if project has been saved.
      // TODO
      if (this.treeExist && this.treeChanged)
      {
        // ask user if he wants to save existing treeView
        DialogResult dr;
        dr = MessageBox.Show(
          "Do you want to save the existing project ?", "save existing project", MessageBoxButtons.YesNoCancel);
        if (dr == DialogResult.Cancel)
        {
          return;
        }

        if (dr == DialogResult.Yes)
        {
          // save current project
          this.SaveAsToolStripMenuItemClick(sender, e);
        }
      }

      // all tabPageDatase fields have to be emptied
      this.EmptyAllTabPageDatase();

      // create a new instance of WebSite
      var site1 = new WebSite { Name = "New web site" };
      this.db1 = new Database { Name = "new database" };
      site1.ListDb.Add(this.db1);
      var table1 = new Table("Table" + this.db1.NumberOfTable.ToString(CultureInfo.InvariantCulture));
      this.db1.Add(table1);
      table1.Name = "Table" + this.db1.NumberOfTable.ToString(CultureInfo.InvariantCulture);
      this.textBoxDatabaseName.Text = this.db1.Name;
      this.InitializeTreeView();
      this.treeExist = true;
      this.treeChanged = false;
    }

    private void EmptyAllTabPageDatase()
    {
      // tab database
      this.textBoxDatabaseName.Text = string.Empty;
      this.checkBoxSortTableAlphabetically.Checked = false;
      this.checkBoxHideTableNavigationMenu.Checked = false;
      this.checkBoxHideLoginSystem.Checked = false;
      this.comboBoxDateFormat.Text = string.Format("Choose a Date Format");
      this.comboBoxDateSeparator.Text = string.Format("Choose a Date Separator");
      this.comboBoxCharacterEncoding.Text = string.Format("Choose a Character Encoding");

      // tab Table
      this.textBoxTableName.Text = string.Empty;
      this.textBoxRecordPerPage.Text = string.Format("15");
      this.comboBoxSortingByDefault.Text = string.Format("No Sorting");
      this.checkBoxdescendingly.Checked = false;
      this.checkBoxAllowFilters.Checked = false;
      this.checkBoxAllowFiltersToSaveFilters.Checked = false;
      this.checkBoxAllowPrintPreView.Checked = false;
      this.checkBoxAllowSavingToCSVFiles.Checked = false;
      this.checkBoxAllowSorting.Checked = false;
      this.checkBoxSelectivePrinting.Checked = false;
      this.checkBoxFilterBeforeShowingTableView.Checked = false;

      // tab Field
      this.textBoxFieldName.Text = string.Empty;
      this.textBoxFieldDescription.Text = string.Empty;
      this.comboBoxFieldDataType.Items.Clear();

      // dataType dt = dataType.bigInteger;
      this.LoadFieldDataType();
      this.comboBoxCopyPropertiesFrom.Items.Clear();

      // TODO add copy properties from items
      this.textBoxFieldLength.Text = string.Format("50");
      this.textBoxFieldPrecision.Text = string.Empty;
      this.textBoxFieldMaxCharactersInTableView.Text = string.Empty;
      this.textBoxFieldDefault.Text = string.Empty;
      this.listBoxFieldAutomaticValue.Visible = false;
    }

    private void LoadFieldDataType()
    {
      this.comboBoxFieldDataType.Items.Clear();
      foreach (var item in Enum.GetNames(typeof(DataType)))
      {
        this.comboBoxFieldDataType.Items.Add(item);
      }
    }

    private void EnableAllFieldCheckBoxes()
    {
      this.checkBoxFieldReadOnly.Enabled = true;
      this.checkBoxFieldRichHTMLArea.Enabled = true;
      this.checkBoxFieldTextArea.Enabled = true;
      this.checkBoxFieldCheckBox.Enabled = true;
      this.checkBoxFieldPrimaryKey.Enabled = true;
      this.checkBoxFieldAutoIncrement.Enabled = true;
      this.checkBoxFieldUnsigned.Enabled = true;
      this.checkBoxFieldHiddenInTable.Enabled = true;
      this.checkBoxFieldZeroFill.Enabled = true;
      this.checkBoxFieldUnique.Enabled = true;
      this.checkBoxFieldDoNotFilter.Enabled = true;
      this.checkBoxFieldHiddenInDetailView.Enabled = true;
      this.checkBoxFieldCantBeEmpty.Enabled = true;
      this.checkBoxFieldShowColumnSum.Enabled = true;
      this.checkBoxFieldBinary.Enabled = true;
      this.checkBoxFieldImage.Enabled = true;
    }

    private void InitializeTreeView()
    {
      // Populates a TreeView control with starting nodes. 
      this.treeView1.BeginUpdate();
      this.treeView1.Nodes.Clear();
      this.treeView1.Nodes.Add("new database");
      this.treeView1.Nodes[0].Nodes.Add("Table" + this.db1.NumberOfTable.ToString(CultureInfo.InvariantCulture));
      this.treeView1.ExpandAll();
      this.treeView1.EndUpdate();
    }

    private void NewTableToolStripMenuItemClick(object sender, EventArgs e)
    {
      if (this.treeExist)
      {
        // add table to database
        this.treeChanged = true;
        this.db1.Add(new Table("table" + (this.db1.NumberOfTable + 1).ToString(CultureInfo.InvariantCulture)));
        this.treeView1.Nodes[0].Nodes.Add("Table" + this.db1.NumberOfTable.ToString(CultureInfo.InvariantCulture));
      }
      else
      {
        MessageBox.Show("You must create a new web site with a database first by going to File - New or File - Open");
      }
    }

    private void TreeView1AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      if (e.Label != null)
      {
        if (e.Label.Length > 0)
        {
          if (e.Label.IndexOfAny(new[] { '@', '.', ',', '!' }) == -1)
          {
            // Stop editing without canceling the label change.
            e.Node.EndEdit(false);
          }
          else
          {
            /* Cancel the label edit action, inform the user, and 
               place the node in edit mode again. */
            e.CancelEdit = true;
            MessageBox.Show(
              "Invalid tree node label.\n" + "The invalid characters are: '@','.', ',', '!'", "Node Label Edit");
            e.Node.BeginEdit();
          }
        }
        else
        {
          /* Cancel the label edit action, inform the user, and 
             place the node in edit mode again. */
          e.CancelEdit = true;
          MessageBox.Show("Invalid tree node label.\nThe label cannot be blank", "Node Label Edit");
          e.Node.BeginEdit();
        }
      }

      this.treeView1.ExpandAll();
    }

    private void FormMainLoad(object sender, EventArgs e)
    {
      this.treeView1.LabelEdit = true;
      this.treeExist = false;
      this.treeChanged = false;
      this.listBoxFieldAutomaticValue.Visible = false;
    }

    private void NewFieldToolStripMenuItemClick(object sender, EventArgs e)
    {
      // if a Field is selected we don't create a new field under the parent
      // if a Table is selected then a son is created
      TreeNode node = this.treeView1.SelectedNode;
      if (node != null)
      {
        if (node.Level == 2)
        {
          MessageBox.Show("You must select a Table, not a field");
        }
        else
        {
          // detect which table is selected and create a new field
          // db1.listTable[node.Index].numberOfField++;
          this.treeView1.Nodes[0].Nodes[node.Index].Nodes.Add("Field" + this.db1.ListTable[node.Index].NumberOfField.ToString(CultureInfo.InvariantCulture));
          this.db1.ListTable[node.Index].Add(new Field("Field" + this.db1.ListTable[node.Index].NumberOfField.ToString(CultureInfo.InvariantCulture)));
          this.treeChanged = true;
        }

        this.treeView1.ExpandAll();
      }
      else
      {
        MessageBox.Show("You must select a table first");
      }
    }

    private void TreeView1NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      // NodeMouseDoubleClick on TreeView in order to change its name (editing mode)
      TreeNode node = this.treeView1.SelectedNode;
      if (node != null)
      {
        node.BeginEdit();
      }
    }

    private void AboutToolStripMenuItemClick(object sender, EventArgs e)
    {
      var aboutBoxApp = new AboutBoxApplication();
      aboutBoxApp.ShowDialog();
    }

    private void TreeView1NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      TreeNode node = this.treeView1.SelectedNode;
      if (node != null)
      {
        // test if the selected node is a database
        // Node.Level == 0 is a database
        if (e.Node.Level == 0)
        {
          // The Database has been clicked
          this.tabControl1.SelectTab(0); // ("Database");
          this.textBoxDatabaseName.Text = this.db1.Name;

          // toolStripLabelTEST.Text = e.Node.Level.ToString(); //to be removed
        }
        else if (e.Node.Level == 1)
        {
          // Node.Level == 1 is a table
          // A table has been clicked
          this.tabControl1.SelectTab(1); // ("Table");
          this.textBoxTableName.Text = this.db1.ListTable[node.Index].Name;
        }
        else
        {
          // Node.Level == 2 is a field
          // A field has been clicked
          this.tabControl1.SelectTab(2); // ("Field");
          this.listBoxFieldAutomaticValue.Visible = false;
          this.LoadFieldDataType();
          this.toolStripLabelTEST.Text = node.Index.ToString(CultureInfo.InvariantCulture);

          // textBoxFieldName.Text = db1.listTable[node.Index].listfields[node.Index].name;
        }
      }
    }

    private void OpenToolStripMenuItemClick(object sender, EventArgs e)
    {
      var openFile = new OpenFileDialog
        {
          Filter = "XML files (*.xml)|*.xml",
          FileName = Application.StartupPath + "\\MyWebsite1.xml"
        };
      if (openFile.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      this.treeView1.Nodes.Clear();
      var serializer = new TreeViewSerializer();
      serializer.DeserializeTreeView(this.treeView1, openFile.FileName);
      this.treeView1.ExpandAll();
    }

    private void SaveToolStripMenuItemClick(object sender, EventArgs e)
    {
      // TODO: implement save name and path
      // if the project has been openned then we save 
      // otherwise we saved as

      // for now we save as
      var saveFile = new SaveFileDialog
        { Filter = "XML files (*.xml)|*.xml", FileName = Application.StartupPath + "\\MyWebsite1.xml" };
      if (saveFile.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      var serializer = new TreeViewSerializer();
      serializer.SerializeTreeView(this.treeView1, saveFile.FileName);
    }

    private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
    {
      var saveFile = new SaveFileDialog
        { Filter = "XML files (*.xml)|*.xml", FileName = Application.StartupPath + "\\MyWebsite1.xml" };
      if (saveFile.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      var serializer = new TreeViewSerializer();
      serializer.SerializeTreeView(this.treeView1, saveFile.FileName);
    }

    private void TreeView1KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F2)
      {
        this.allowNodeRenaming = true;
        this.treeView1.SelectedNode.BeginEdit();
      }
    }

    private void TreeView1BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      if (!this.allowNodeRenaming)
      {
        e.CancelEdit = true;
      }

      this.allowNodeRenaming = false;
    }

    private void ButtonFieldAutomaticValueClick(object sender, EventArgs e)
    {
      this.listBoxFieldAutomaticValue.Items.Clear();
      this.listBoxFieldAutomaticValue.Visible = true;
      this.listBoxFieldAutomaticValue.Items.Add("Created by Username");
      this.listBoxFieldAutomaticValue.Items.Add("Created by IP address");
      this.listBoxFieldAutomaticValue.Items.Add("Created by Group");
      this.listBoxFieldAutomaticValue.Items.Add("Created by Group ID");
      this.listBoxFieldAutomaticValue.Items.Add("Creation Date");
      this.listBoxFieldAutomaticValue.Items.Add("Creation Time");
      this.listBoxFieldAutomaticValue.Items.Add("Creation Date and Time");
      this.listBoxFieldAutomaticValue.Items.Add("Creation Timestamp");
      this.listBoxFieldAutomaticValue.Items.Add("Edited by Username");
      this.listBoxFieldAutomaticValue.Items.Add("Edited by IP address");
      this.listBoxFieldAutomaticValue.Items.Add("Edited by Group");
      this.listBoxFieldAutomaticValue.Items.Add("Edited by Group ID");
      this.listBoxFieldAutomaticValue.Items.Add("Editing Date");
      this.listBoxFieldAutomaticValue.Items.Add("Editing Time");
      this.listBoxFieldAutomaticValue.Items.Add("Editing Date and Time");
      this.listBoxFieldAutomaticValue.Items.Add("Editing Timestamp");
    }

    private void ListBoxFieldAutomaticValueSelectedIndexChanged(object sender, EventArgs e)
    {
      this.textBoxFieldDefault.Text += "<%%" + this.listBoxFieldAutomaticValue.SelectedItem + "%%>";
      this.listBoxFieldAutomaticValue.Visible = false;
    }

    private void ButtonFieldClearAutomaticValuesClick(object sender, EventArgs e)
    {
      this.textBoxFieldDefault.Text = string.Empty;
    }

    private void CheckBoxFieldImageCheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxFieldImage.Checked)
      {
        this.buttonFieldImageOptions.Enabled = true;

        // TODO add to class instance
        this.textBoxFieldDescription.Text += "Maximum file size allowed: 100 KB.\n";
        this.textBoxFieldDescription.Text += "Allowed file types: jpg, jpeg, gif, png";
      }
      else
      {
        this.buttonFieldImageOptions.Enabled = false;
        this.textBoxFieldDescription.Text += string.Empty; // TODO remove only images with class instance
      }
    }

    private void ComboBoxFieldDataTypeSelectedIndexChanged(object sender, EventArgs e)
    {
      // according to choice, enable or disable checkboxes
      const string GuideLinesStart = "Guidelines regarding the ";
      const string GuideLinesEnd = " data type:";
      switch (this.comboBoxFieldDataType.SelectedIndex)
      {
        case 0: // varchar
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.VarChar.ToString() + GuideLinesEnd;
          break;

        case 1: // tiny integer
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.TinyInteger.ToString() + GuideLinesEnd;
          break;

        case 2: // small integer
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.SmallInteger.ToString() + GuideLinesEnd;
          break;

        case 3: // medium integer
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.MediumInteger.ToString() + GuideLinesEnd;
          break;

        case 4: // integer
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Integer.ToString() + GuideLinesEnd;
          break;

        case 5: // big integer
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.BigInteger.ToString() + GuideLinesEnd;
          break;

        case 6: // float
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Float.ToString() + GuideLinesEnd;
          break;

        case 7: // double
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Double.ToString() + GuideLinesEnd;
          break;

        case 8: // decimal
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Decimal.ToString() + GuideLinesEnd;
          break;

        case 9: // date
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Date.ToString() + GuideLinesEnd;
          break;

        case 10: // DateTime
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.DateTime.ToString() + GuideLinesEnd;
          break;

        case 11: // Timestamp
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.TimeStamp.ToString() + GuideLinesEnd;
          break;

        case 12: // time
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Time.ToString() + GuideLinesEnd;
          break;

        case 13: // Year
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary, this.checkBoxFieldImage);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Year.ToString() + GuideLinesEnd;
          break;

        case 14: // Char
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary);
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Char.ToString() + GuideLinesEnd;
          break;

        case 15: // Tiny Blob
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Tinyblob.ToString() + GuideLinesEnd;
          break;

        case 16: // tiny Text
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.TinyText.ToString() + GuideLinesEnd;
          break;

        case 17: // text
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Text.ToString() + GuideLinesEnd;
          break;

        case 18: // blob
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.Blob.ToString() + GuideLinesEnd;
          break;

        case 19: // medium blob
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.MediumBlob.ToString() + GuideLinesEnd;
          break;

        case 20: // medium text
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.MediumText.ToString() + GuideLinesEnd;
          break;

        case 21: // long blob
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum, this.checkBoxFieldBinary);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.LongBlob.ToString() + GuideLinesEnd;
          break;

        case 22: // long text
          this.EnableAllFieldCheckBoxesBut(this.checkBoxFieldAutoIncrement, this.checkBoxFieldUnsigned, this.checkBoxFieldZeroFill, this.checkBoxFieldShowColumnSum);
          this.checkBoxFieldTextArea.Checked = true;
          this.labelFieldGuidelines.Text = GuideLinesStart + DataType.LongText.ToString() + GuideLinesEnd;
          break;
      }
    }

    private void EnableAllFieldCheckBoxesBut(params CheckBox[] listeOfCheckBoxes)
    {
      this.EnableAllFieldCheckBoxes(); // Enable all checkboxes
      foreach (CheckBox cb in listeOfCheckBoxes)
      {
        cb.Enabled = false; // but these
      }
    }

    private void NewTableFromCsvFileToolStripMenuItemClick(object sender, EventArgs e)
    {
      this.treeChanged = true;
    }

    private void DeleteTableToolStripMenuItemClick(object sender, EventArgs e)
    {
      this.treeChanged = true;
    }

    private void DeleteFieldToolStripMenuItem1Click(object sender, EventArgs e)
    {
      this.treeChanged = true;
    }

    private void MoveUpToolStripMenuItem1Click(object sender, EventArgs e)
    {
      // move field one field up
      // check if field is not the first one
    }

    private void MoveDownToolStripMenuItem1Click(object sender, EventArgs e)
    {
      // move field one field down
      // check if field is not the last one
    }
  }
}