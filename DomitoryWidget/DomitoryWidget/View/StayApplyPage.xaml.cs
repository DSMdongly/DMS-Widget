﻿using System;
using System.Collections.Generic;
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

namespace DomitoryWidget.View
{
    /// <summary>
    /// StayApplyPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StayApplyPage : Page
    {
        private MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        public StayApplyPage()
        {
            InitializeComponent();
        }
        
        private void CancelApplyButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigatePage(new MainPage());
        }

        private void SubmitApplyButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}