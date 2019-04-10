using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fillComboBoxtoDelete();
        }

        private void fillComboBoxtoDelete()
        {
            comboBoxIDtoDelete.Items.Clear();
            List<int> someList = Logic.giveID();
            foreach (var item in someList)
            {
                comboBoxIDtoDelete.Items.Add(item);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnAddRetard_Click(object sender, EventArgs e)
        {
            Logic.CheckStuff(textBoxNameStyle.Text, textBoxTitle.Text, textBoxFirstName.Text, textBoxMiddleName.Text,
                textBoxLastName.Text, textBoxSuffix.Text, textBoxMail.Text);
        }

        private void comboBoxIDtoDelete_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (Logic.DeletePerson(Convert.ToInt32(comboBoxIDtoDelete.GetItemText(comboBoxIDtoDelete.SelectedItem))))
            {
                MessageBox.Show("Faggot deleted");
                fillComboBoxtoDelete();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
          Dictionary<int,string> valueList = new Dictionary<int, string>(Logic.SelectPerson(Convert.ToInt32(comboBoxIDtoDelete.GetItemText(comboBoxIDtoDelete.SelectedItem))));
            try
            {
                textBoxNameStyle.Text = valueList[2];
                textBoxTitle.Text = valueList[3];
                textBoxFirstName.Text = valueList[4];
                textBoxMiddleName.Text = valueList[5];
                textBoxLastName.Text = valueList[6];
                textBoxSuffix.Text = valueList[7];
                textBoxMail.Text = valueList[8];
            }
            catch (Exception asd)
            {
                MessageBox.Show(asd.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Logic.Update(Convert.ToInt32(comboBoxIDtoDelete.GetItemText(comboBoxIDtoDelete.SelectedItem)), textBoxNameStyle.Text, textBoxTitle.Text, textBoxFirstName.Text,
                textBoxMiddleName.Text, textBoxLastName.Text, textBoxSuffix.Text, textBoxMail.Text))
            {
                MessageBox.Show("YEEEEEY");
            }
            else
            {
                MessageBox.Show("BLEEEE");
            }
        }
    }
}
