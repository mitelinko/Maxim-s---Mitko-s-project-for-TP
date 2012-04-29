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
    /// Interaction logic for ProfessorDayAndTimeWindow.xaml
    /// </summary>

    public partial class ProfessorDayAndTimeWindow : Window
    {
        public TrulyObservableCollection<TimeDayRequirement> Requirements = new TrulyObservableCollection<TimeDayRequirement>();
       
        public ProfessorDayAndTimeWindow()
        {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboProfessors.ItemsSource = Configuration.Instance.Professors;
            listRequirements.ItemsSource = Requirements;
           // comboDays.ItemsSource = ConversionServices.DayNames;
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

        public string ConstraintName
        {
            get
            {
                return txtName.Text;
            }
        }

        private void btnAddClick(object sender, RoutedEventArgs e)
        {
            if (comboProfessors.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a professor!");
                return;
            }

            Professor prof = comboProfessors.SelectedItem as Professor;

            if (comboDays.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a day!");
                return;
            }

            int day = comboDays.SelectedIndex;
            int startHours;
            int startMinutes;
            int endHours;
            int endMinutes;
            int seconds = 0;

            if (int.TryParse(txtStartHour.Text, out startHours))
            {
                if (startHours < 8 || startHours >20 )
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
                if (startMinutes < 0 || startMinutes >59 )
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
                if (endHours < 8 || endHours >20 )
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

            TimeSpan start = new TimeSpan( startHours, startMinutes, seconds );
            TimeSpan end = new TimeSpan( endHours, endMinutes, seconds);

            if (start < end)
            {
                TimeDayRequirement req = new TimeDayRequirement(prof, day, start, end);
                if (Requirements.Contains(req))
                {
                    MessageBox.Show("Ooops. This requirement already exists!");
                    return;
                }
                else
                {
                    Requirements.Add(req);
                }
            }
            else
            {
                MessageBox.Show("Ooops. Start time must be earlier than the end time!");
                return;
            }
        }

        private void btnDelClick(object sender, RoutedEventArgs e)
        {
            var aReq = listRequirements.SelectedItem as TimeDayRequirement;
            Requirements.Remove(aReq);
        }

        private void btnClearClick(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "You are about to clear the requirements list. Are you sure?";
            string caption = "Clear requirements";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Requirements.Clear();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btnCreateClick(object sender, RoutedEventArgs e)
        {
            if (ConstraintName.Trim() == string.Empty)
            {
                MessageBox.Show("Ooops. Please, enter a name!");
                return;
            }

            if (Requirements.Count == 0)
            {
                MessageBox.Show("Ooops. No requirеments entered!");
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void btnCnacelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
