using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.DataAccess.GenericRepository;
using Ninject.Parameters;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using WpfClient.Model;
using WpfClient.Model.Plot;
using WpfClient.ViewModel;
using WpfClient.ViewModel.Plot;

namespace WpfClient.Services
{
    class PlotService
    {
        private readonly List<OxyColor> colors = new List<OxyColor> { OxyColors.Green, OxyColors.IndianRed, OxyColors.Coral, OxyColors.Chartreuse, OxyColors.Azure, OxyColors.Black };

        private readonly List<MarkerType> markerTypes = new List<MarkerType> { MarkerType.Plus, MarkerType.Star, MarkerType.Diamond, MarkerType.Triangle, MarkerType.Cross, MarkerType.Circle };
        public void SetUpModel(PlotModel PlotModel, int fanObjectId, string Title = "", string SubTitle = "")
        {
            PlotModel.Title = Title + fanObjectId;
            PlotModel.SubtitleColor = OxyColors.LightGreen;
            PlotModel.Subtitle = SubTitle;
            PlotModel.LegendTitle = "Легенда";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;
            PlotModel.MouseDown += (s, e) =>
            {
                try
                {
                    if (e.IsControlDown)
                    {
                        DataPoint date = (DataPoint)e.HitTestResult.Item;
                        IoC.Resolve<MainVm>().CurrentView =
                            IoC.Resolve<OnPlotClickVm>(new ConstructorArgument("fanObjectId", fanObjectId),
                                                       new ConstructorArgument("date",
                                                                               DateTimeAxis.ToDateTime(date.X)),
                                                       new ConstructorArgument("prevView",
                                                                               IoC.Resolve<MainVm>().CurrentView));
                    }
                }
                catch (Exception)
                {
                    return;
                }
            };
        }
        public void SetLinearDataAxis(PlotModel PlotModel)
        {
            var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Дата", "yyyy-MM-dd HH:mm:ss")
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 150
            };
            PlotModel.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis(AxisPosition.Left, -0.05)
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Значение"
            };
            PlotModel.Axes.Add(valueAxis);
        }
        public void ClearPlotData(PlotModel PlotModel)
        {
            PlotModel.Series.Clear();
        }
        private void LoadData(PlotModel PlotModel, IEnumerable<OxyPlotData> measurements)
        {
            if (!measurements.Any()) { return; }

            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = colors[PlotModel.Series.Count],
                MarkerType = markerTypes[PlotModel.Series.Count],
                CanTrackerInterpolatePoints = false,
                Title = measurements.First().ParamName,
                Smooth = false,
            };

            foreach (var data in measurements)
            {
                lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(data.DateTime), data.Value));
            }

            PlotModel.Series.Add(lineSerie);
        }
        public void ShowSignal(PlotModel plotModel, int fanObjectId, string paramName, DateTime from, DateTime to)
        {
            try
            {
                using (var repoUnit = new RepoUnit())
                {
                    var tmp = repoUnit.FanLog.Load().Where(f => f.FanNumber == fanObjectId && f.Date > from && f.Date < to)
                        .Select(f => new { Date = f.Date, AnalogSignal = f.AnalogSignalLogs.FirstOrDefault(a => a.SignalType.Type == paramName) })
                        .Select(s => new OxyPlotData { DateTime = s.Date, Value = s.AnalogSignal.SignalValue, ParamName = s.AnalogSignal.SignalType.Type });

                    LoadData(plotModel, tmp.ToList());
                }
            }
            catch(Exception ex)
            {}
        }
        public void DeleteSignal(PlotModel plotModel, string elemetName)
        {
            try
            {
                plotModel.Series.Remove(plotModel.Series.FirstOrDefault(f=>f.Title == elemetName));
            }
            catch (Exception)
            {  
            }
        }
        public void ShowDoor(PlotModel plotModel, int fanObjectId, string doorName, DateTime from, DateTime to)
        {
            try
            {
                using (var repoUnit = new RepoUnit())
                {
                    var tmp = repoUnit.FanLog.Load().Where(f => f.FanNumber == fanObjectId && f.Date > from && f.Date < to)
                        .Select(f => new { Date = f.Date, Door = f.DoorsLogs.FirstOrDefault(a => a.DoorType.Type == doorName) })
                        .Select(s => new OxyPlotData { DateTime = s.Date,
                                                       Value = s.Door.DoorState.State == "Открыт" ? 1 : s.Door.DoorState.State == "Закрыт" ? 0 : 0.5, 
                            ParamName = s.Door.DoorType.Type });

                    LoadData(plotModel, tmp.ToList());
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
