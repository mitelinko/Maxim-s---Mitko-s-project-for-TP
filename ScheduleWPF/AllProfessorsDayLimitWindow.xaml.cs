using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScheduleWPF
{
    /// <summary>
    /// Interaction logic for AllProfessorsDayLimitWindow.xaml
    /// </summary>
    public partial class AllProfessorsDayLimitWindow : Window
    {
        public AllProfessorsDayLimitWindow()
        {
            InitializeComponent();
        }

        public int MaximumClassesPerDay
        {
            get
            {
                try
                {
                    return int.Parse(txtMaxClasses.Text);
                }
                catch
                {
                    throw new Exception("Cannot convert text to integer.");
                }
            }
        }

        public string ConstraintName
        {
            get
            {
                return txtName.Text;
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int result;
            if (ConstraintName.Trim() == string.Empty)
            {
                MessageBox.Show("Ooops. Please, enter a name!");
                return;
            }
            if (int.TryParse(txtMaxClasses.Text, out result))
            {
                if (result > 0)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ooops. The number must not be negative or zero!");
                }
            }
            else
            {
                MessageBox.Show("Ooops. Please enter a number!");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
