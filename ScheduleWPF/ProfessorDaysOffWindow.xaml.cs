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
    /// Interaction logic for ProfessorDaysOffWindow.xaml
    /// </summary>
    public partial class ProfessorDaysOffWindow : Window
    {
        public List<int> Days = new List<int>();

        public ProfessorDaysOffWindow()
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

            if ( checkMonday.IsChecked==true )
            {
                Days.Add(0);
            }

            if (checkTuesday.IsChecked == true)
            {
                Days.Add(1);
            }

            if (checkWednesday.IsChecked == true)
            {
                Days.Add(2);
            }

            if (checkThursday.IsChecked == true)
            {
                Days.Add(3);
            }

            if (checkFriday.IsChecked == true)
            {
                Days.Add(4);
            }

            if (Days.Count == 0)
            {
                MessageBox.Show("Ooops. Please, select at least one day!");
                return;
            }

            this.DialogResult = true;
            this.Close();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        
    }
}
