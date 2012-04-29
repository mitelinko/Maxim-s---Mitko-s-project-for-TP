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
    /// Interaction logic for ProfessorTimeOffWindow.xaml
    /// </summary>
    public partial class ProfessorTimeOffWindow : Window
    {
        public ProfessorTimeOffWindow()
        {
            InitializeComponent();
        }

        public TimeSpan Start;
        public TimeSpan End;

        public string ConstraintName
        {
            get
            {
                return txtName.Text;
            }
        }

        public int StartTimeHour
        {
            get
            {
                try
                {
                    return int.Parse(txtStartHour.Text);
                }
                catch
                {
                    throw new Exception("Cannot convert text to integer.");
                }
            }
        }

        public int StartTimeMin
        {
            get
            {
                try
                {
                    return int.Parse(txtStartMin.Text);
                }
                catch
                {
                    throw new Exception("Cannot convert text to integer.");
                }
            }
        }

        public int EndTimeHour
        {
            get
            {
                try
                {
                    return int.Parse(txtEndHour.Text);
                }
                catch
                {
                    throw new Exception("Cannot convert text to integer.");
                }
            }
        }

        public int EndTimeMin
        {
            get
            {
                try
                {
                    return int.Parse(txtEndMin.Text);
                }
                catch
                {
                    throw new Exception("Cannot convert text to integer.");
                }
            }
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Please select a professor!");
                return;
            }

            int startHours;
            int startMinutes;
            int endHours;
            int endMinutes;
            int seconds = 0;

            if (int.TryParse(txtStartHour.Text, out startHours))
            {
                if (startHours < 8 || startHours > 20)
                {
                    MessageBox.Show("Ooops. The start hour must be between 8 and 20!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ooops. Please enter a start hour[8,20] !");
                return;
            }

            if (int.TryParse(txtStartMin.Text, out startMinutes))
            {
                if (startMinutes < 0 || startMinutes > 59)
                {
                    MessageBox.Show("Ooops. The start minutes must be between 0 and 59!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ooops. Please enter start minutes[0,59] !");
                return;
            }

            if (int.TryParse(txtEndHour.Text, out endHours))
            {
                if (endHours < 8 || endHours > 20)
                {
                    MessageBox.Show("Ooops. The end hour must be between 8 and 20!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ooops. Please enter an end hour[8,20] !");
                return;
            }

            if (int.TryParse(txtEndMin.Text, out endMinutes))
            {
                if (endMinutes < 0 || endMinutes > 59)
                {
                    MessageBox.Show("Ooops. The end minutes must be between 0 and 59!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ooops. Please enter end minutes[0,59] !");
                return;
            }

            Start = new TimeSpan(startHours, startMinutes, seconds);
            End = new TimeSpan(endHours, endMinutes, seconds);

            if (Start < End)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ooops. Start time must be earlier than the end time!");
                return;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
