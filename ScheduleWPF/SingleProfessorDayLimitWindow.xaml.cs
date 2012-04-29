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
using ScheduleCommon;

namespace ScheduleWPF
{
    /// <summary>
    /// Interaction logic for SingleProfessorDayLimitWindow.xaml
    /// </summary>
    public partial class SingleProfessorDayLimitWindow : Window
    {
        public SingleProfessorDayLimitWindow()
        {
            InitializeComponent();
        }

        public string ConstraintName
        {
            get
            {
                return txtName.Text;
            }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboProf.ItemsSource = Configuration.Instance.Professors;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (ConstraintName.Trim() == string.Empty)
            {
                MessageBox.Show("Ooops. Please, enter a name!");
                return;
            }

            if (comboProf.SelectedIndex == -1)
            {
                MessageBox.Show("Ooops. Please select a professor!");
                return;
            }
            int result;
            if (int.TryParse(txtMaxClasses.Text, out result))
            {
                if (result > 0 && result < 10)
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
