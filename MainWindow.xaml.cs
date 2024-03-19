using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoListManager.DB;

namespace ToDoListManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDealsRepository _repository;
        public MainWindow()
        {
            InitializeComponent();
            _repository = new DBOperation();
            ShowList();

            if(ToDoListBox.SelectedIndex != -1)
            {
                CaseTextBox.Text = ((Deals)ToDoListBox.SelectedItem).Case;
                PriorityComboBox.Text = Convert.ToString(((Deals)ToDoListBox.SelectedItem).Priority);
                DeadlineCalendar.SelectedDate = ((Deals)ToDoListBox.SelectedItem).Execution;
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Deals deal = new Deals();
                deal.Case = CaseTextBox.Text;
                deal.Priority = Convert.ToInt32(PriorityComboBox.Text);
                deal.Execution = DeadlineCalendar.SelectedDate.Value;
                _repository.AddCase(deal);
                ShowList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add deal error: {ex.Message}", "Error");
            }
        }
        
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ToDoListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("No one row selected");
                    return;
                }
                int id = ((Deals)ToDoListBox.SelectedItem).Id;
                _repository.DeleteCase(id);
                ShowList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete deal error: {ex.Message}", "Error");
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(ToDoListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("No one row selected");
                    return;
                }
                Deals deal = new Deals();
                deal.Id = ((Deals)ToDoListBox.SelectedItem).Id;
                deal.Case = CaseTextBox.Text;
                deal.Priority = Convert.ToInt32(PriorityComboBox.Text);
                deal.Execution = DeadlineCalendar.SelectedDate.Value;
                _repository.EditCase(deal);
                ShowList();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Edit deal error: {ex.Message}", "Error");
            }
        }

        private void ShowList()
        {
            try
            {
                List<Deals> deals = _repository.GetAll();
                ToDoListBox.Items.Clear();
                foreach (Deals deal in deals)
                {
                    ToDoListBox.Items.Add(deal);
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ShowList error: {ex.Message}", "Error");
            }
        }

        private void orderByPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Deals> deals = _repository.OrderByPriority();
                ToDoListBox.Items.Clear();
                foreach (Deals deal in deals)
                {
                    ToDoListBox.Items.Add(deal);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ShowList error: {ex.Message}", "Error");
            }
        }

        private void deadlineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Deals> deals = _repository.OrderByDeadline();
                ToDoListBox.Items.Clear();
                foreach (Deals deal in deals)
                {
                    ToDoListBox.Items.Add(deal);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ShowList error: {ex.Message}", "Error");
            }
        }

    }
}
