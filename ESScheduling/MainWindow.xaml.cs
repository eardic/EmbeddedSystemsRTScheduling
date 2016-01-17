using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ESScheduling.Jobs;
using ESScheduling.Schedulers;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ESScheduling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JobBuilder _jobBuilder;
        private IScheduler _scheduler;

        public int RunTimeOfScheduling { get; set; }

        public ObservableCollection<Job> Jobs { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitPlotView();
            _jobBuilder = new JobBuilder();
        }

        private void BtnRunSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (Jobs == null || !Jobs.Any())
            {
                MessageBox.Show("Couldn't find any job to schedule. Please select a job source.", "No Jobs",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var schedule = _scheduler.Run(RunTimeOfScheduling);
            LblSchedule.Text = _scheduler.Scheduled ? "Schedule : OK !" : " Schedule : Deadline MISS !";
            LblSchedule.Foreground = _scheduler.Scheduled ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            LblSchedule.InvalidateVisual();

            SchedulePlot.Model.Series.Clear();

            SchedulePlot.Model.Axes[1].MajorStep = 1;

            foreach (var job in Jobs)
            {
                var serie = new LineSeries { Title = string.Format("J{0}", job.Id) };
                serie.StrokeThickness = 7;
                serie.BrokenLineStyle = LineStyle.Dash;
                serie.BrokenLineThickness = 1;
                serie.BrokenLineColor = OxyColors.Black;
                serie.Points.Add(new DataPoint(0, job.Id));
                SchedulePlot.Model.Series.Add(serie);
            }
            for (var i = 0; i < schedule.Count; ++i)
            {
                var scheduledJob = schedule[i];

                foreach (LineSeries serie in SchedulePlot.Model.Series)
                {
                    // Deadlines
                    var j = Jobs.First(x => string.Format("J{0}", x.Id) == serie.Title);
                    if ((j.Type == JobType.Aperiodic && i == j.Deadline) ||
                        (j.Type == JobType.Periodic && (i != j.ArrivalTime) && (i - j.ArrivalTime) % j.Period == 0))
                    {
                        serie.Points.Add(new DataPoint(i, j.Id));
                        serie.Points.Add(new DataPoint(i, j.Id + 0.5));
                        serie.Points.Add(new DataPoint(i, j.Id));
                        serie.Points.Add(DataPoint.Undefined);
                    }

                    if (scheduledJob != null)
                    {
                        var jobTitle = string.Format("J{0}", scheduledJob.Id);
                        if (serie.Title == jobTitle)
                        {
                            serie.Points.Add(new DataPoint(i, scheduledJob.Id));
                            serie.Points.Add(new DataPoint(i + 1, scheduledJob.Id));
                            serie.Points.Add(DataPoint.Undefined);
                        }
                        else
                        {
                            serie.Points.Add(DataPoint.Undefined);
                        }
                    }
                    else
                    {
                        serie.Points.Add(DataPoint.Undefined);
                    }

                    if (i == RunTimeOfScheduling - 1)
                    {
                        serie.Points.Add(new DataPoint(i + 1, j.Id));
                    }
                }
                Debug.WriteLine(i + " => " + (scheduledJob != null ? scheduledJob.Id : -1));
            }

            SchedulePlot.InvalidatePlot();
            SchedulePlot.UpdateLayout();

            Message("Schedule chart is successfully drawn.", true);

        }

        private void InitPlotView()
        {
            var model = new PlotModel();
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0 });
            SchedulePlot.Model = model;
        }

     
        private int FindLeastCommonPeriod()
        {
            try
            {
                var periods = Jobs.Select(t => t.Period);
                return periods.Aggregate(MathHelpers.LCD);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void BtnLoadJobs_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Select Job File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Jobs = new ObservableCollection<Job>(_jobBuilder.GetJobsFromFile(openFileDialog.FileName));
                    jobTable.ItemsSource = Jobs;

                    if (Jobs.Any(j => j.Type == JobType.Aperiodic))
                    {
                        RunTimeOfScheduling = (Jobs.Max(j => j.Deadline) + 1);
                    }
                    else
                    {
                        RunTimeOfScheduling = FindLeastCommonPeriod();
                    }

                    TxtRunTime.Text = RunTimeOfScheduling.ToString();

                    var u = MathHelpers.U(Jobs);
                    var m = MathHelpers.M(Jobs.Count);
                    var rm = u <= m;

                    TxtSchedulabilityValue.Text = string.Format("{0:0.000} <= {1:0.000}", u, m);
                    TxtSchedulability.Text = rm ? "(Schedulable)" : "(Not Schedulable)";
                    TxtSchedulability.Foreground = rm ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

                    CbSchedules.IsEnabled = true;
                    CreateScheduler();

                    Message(string.Format("Loaded {0} jobs successfully.", Jobs.Count()), true);
                }
                catch (Exception ex)
                {
                    Message("Couldn't load jobs from source. Please validate source file.", false);
                }
            }
        }

        private void Message(string text, bool success)
        {
            Status.Text = text;
            Status.Foreground = !success ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
            Status.Visibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                Dispatcher.Invoke(new Action(() =>
                {
                    Status.Visibility = Visibility.Hidden;
                }));
            });
        }

        private void CreateScheduler()
        {
            if (Jobs != null && CbSchedules.SelectedItem != null)
            {
                switch (CbSchedules.SelectedItem.ToString())
                {
                    case "EDD":
                        _scheduler = new EDD(Jobs);
                        break;
                    case "EDF":
                        _scheduler = new EDF(Jobs);
                        break;
                    case "RM":
                        _scheduler = new RM(Jobs);
                        break;
                    case "LLF":
                        _scheduler = new LLF(Jobs);
                        break;
                }
                BtnRunSchedule.IsEnabled = Jobs.Any() && _scheduler != null;
            }
        }

        private void CbSchedules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateScheduler();
        }
    }
}
