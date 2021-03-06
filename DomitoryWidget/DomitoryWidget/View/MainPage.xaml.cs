﻿using DomitoryWidget.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        private MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        public MainPage()
        {
            InitializeComponent();
            LoadMyPage();
            LoadTodayMeals();
        }

        private void LoadMyPage()
        {
            var response = DMS.MyPage(mainWindow.AccesssToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("마이페이지 로딩 실패");
                mainWindow.NavigatePage(new LoginPage());

                return;
            }

            SetMyPageFromReponse(response);
        }
        
        private void LoadTodayMeals()
        {
            var now = DateTime.Now;
            var response = DMS.Meal(now.Year, now.Month, now.Day);

            TodayLabel.Content = $"{now.Year}년 " +
                                 $"{now.Month:00}월 " +
                                 $"{now.Day:00}일 " +
                                 ParseDayOfWeek(now.DayOfWeek);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("식단 로딩 실패");
                return;
            }

            SetTodayMealsFromResponse(response);
        }

        private void SetMyPageFromReponse(RestResponse response)
        {
            var content = JObject.Parse(response.Content);

            ElevenExtensionLabel.Content = ParseExtension(content.Value<JObject>("extension_11"));
            TwelveExtensionLabel.Content = ParseExtension(content.Value<JObject>("extension_12"));
            StayWeekendLabel.Content = ParseStayWeekend(content.Value<int?>("stay_value") ?? 0);

            SaturdayGoingoutLabel.Content = ParseGoingout(content["goingout"].Value<bool>("sat"));
            SundayGoingoutLabel.Content = ParseGoingout(content["goingout"].Value<bool>("sun"));
        }

        private void SetTodayMealsFromResponse(RestResponse response)
        {
            var content = JObject.Parse(response.Content);

            BreakfastText.Text = ParseMeal(content.Value<JArray>("breakfast"));
            LunchText.Text = ParseMeal(content.Value<JArray>("lunch"));
            DinnerText.Text = ParseMeal(content.Value<JArray>("dinner"));
        }

        private static string ParseDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday: return "일요일";
                case DayOfWeek.Monday: return "월요일";
                case DayOfWeek.Tuesday: return "화요일";
                case DayOfWeek.Wednesday: return "수요일";
                case DayOfWeek.Thursday: return "목요일";
                case DayOfWeek.Friday: return "금요일";
                case DayOfWeek.Saturday: return "토요일";
                default: return null;
            }
        }

        private static string ParseExtension(JObject extension)
        {
            if (extension == null) return "미신청";
            else return ParseExtension(extension.Value<int>("class_num"));
        }

        private static string ParseExtension(int extension)
        {
            switch (extension)
            {
                case 1: return "가온실";
                case 2: return "나온실";
                case 3: return "다온실";
                case 4: return "라온실";
                case 5: return "3층 독서실";
                case 6: return "4층 독서실";
                case 7: return "5층 열린교실";
                case 8: return "2층 여자 자습실";
                default: return "미신청";
            }
        }

        private static string ParseStayWeekend(int stay)
        {
            switch (stay)
            {
                case 1: return "금요귀가";
                case 2: return "토요귀가";
                case 3: return "토요귀사";
                case 4: return "잔류";
                default: return "미신청";
            }
        }

        private static string ParseGoingout(bool goingout)
        {
            return goingout ? "신청" : "미신청";
        }

        private static string ParseMeal(JArray meal)
        {
            return meal != null ? string.Join(",", meal) : "급식이 없습니다.";
        }

        private void StayApplyButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigatePage(new StayApplyPage());
        }

        private void GoingoutApplyButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigatePage(new GoingoutApplyPage());
        }
    }
}
