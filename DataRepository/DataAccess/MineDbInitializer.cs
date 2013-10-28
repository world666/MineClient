using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using DataRepository.Models;

namespace DataRepository.DataAccess
{
    public class MineDbInitializer : DropCreateDatabaseIfModelChanges<MineContext>
    {
        protected override void Seed(MineContext context)
        {
            if (context == null)
                context = new MineContext();

            //Init DoorType table
            var doorType = new List<DoorType>
                {
                    new DoorType {Type = "Ляда 1 отсекающая"},
                    new DoorType {Type = "Ляда 1 переключающая"},
                    new DoorType {Type = "Ляда 2 отсекающая"},
                    new DoorType {Type = "Ляда 2 переключающая"},
                    new DoorType {Type = "Ляда атмосферная"},
                    new DoorType {Type = "Ляда дифузорная"},
                    new DoorType {Type = "Ляда подводящего канала"},
                    new DoorType {Type = "Направляющий аппарата вентилятора 1"},
                    new DoorType {Type = "Направляющий аппарата вентилятора 2"}
                };
            doorType.ForEach(d => context.DoorsType.Add(d));

            //Init DoorState table
            var doorState = new List<DoorState>
                {
                    new DoorState {State = "Неопределен"},
                    new DoorState {State = "Закрыт"},
                    new DoorState {State = "Открыт"},
                    new DoorState {State = "Неопределен"}
                };
            doorState.ForEach(d => context.DoorState.Add(d));

            //Init FanState table
            var fanState = new List<FanState>
                {
                    new FanState {State = "Выключен"},
                    new FanState {State = "Включен"},
                };
            fanState.ForEach(d => context.FanState.Add(d));

            //Init AnalogSignalType table
            var analogSignalType = new List<AnalogSignal>
                {
                    new AnalogSignal {Type = "Расход Воздуха В1 (м3/c)"},
                    new AnalogSignal {Type = "Депрессия В1 (кПа)"},
                    new AnalogSignal {Type = "Температура подшипника №1 В1 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №2 В1 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №3 В1 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №4 В1 (град)"},
                    new AnalogSignal {Type = "Вибрация подшипника №1 В1 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №2 В1 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №3 В1 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №4 В1 (мкм)"},
                    new AnalogSignal {Type = "Расход Воздуха В2 (м3/c)"},
                    new AnalogSignal {Type = "Депрессия В2 (кПа)"},
                    new AnalogSignal {Type = "Температура подшипника №1 В2 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №2 В2 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №3 В2 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №4 В2 (град)"},
                    new AnalogSignal {Type = "Вибрация подшипника №1 В2 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №2 В2 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №3 В2 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №4 В2 (мкм)"},
                    new AnalogSignal {Type = "Скорость двигателя В1 (об/мин)"},
                    new AnalogSignal {Type = "Ток статора В1 (А)"},
                    new AnalogSignal {Type = "Ток ротора В1 (А)"},
                    new AnalogSignal {Type = "Скорость двигателя В2 (об/мин)"},
                    new AnalogSignal {Type = "Ток статора В2 (А)"},
                    new AnalogSignal {Type = "Ток ротора В2 (А)"},
                    new AnalogSignal {Type = "Температура масла (град)"},
                    new AnalogSignal {Type = "Давление масла (кПа)"},
                    new AnalogSignal {Type = "Проток масла (л/мин)"},
                    new AnalogSignal {Type = "Температура ротора №1"},
                    new AnalogSignal {Type = "Температура ротора №2"},
                    new AnalogSignal {Type = "Температура ротора №3"},
                    new AnalogSignal {Type = "Температура ротора №4"},
                    new AnalogSignal {Type = "Температура ротора №5"},
                    new AnalogSignal {Type = "Температура ротора №6"},
                    new AnalogSignal {Type = "Температура ротора №7"},
                    new AnalogSignal {Type = "Температура ротора №8"},
                    new AnalogSignal {Type = "Качество сигнала"}
                };
            analogSignalType.ForEach(s => context.AnalogSignals.Add(s));

            //Init Settings дублирование настроек в базу для веб сервера mvc
            var settings = new List<SettingsLog>
            {
                new SettingsLog {Name = "MaxTemperature", DValue = 120},
                new SettingsLog {Name = "MaxPillowValue", DValue = 35},
                new SettingsLog {Name = "MaxAirFlowValue", DValue = 80},
                new SettingsLog {Name = "MaxPressureValue", DValue = 6},
                new SettingsLog {Name = "MaxSpeedValue", DValue = 300},
                new SettingsLog {Name = "MaxCurrentValue", DValue = 300},
                new SettingsLog {Name = "MaxOilFlowValue", DValue = 10},
                new SettingsLog {Name = "MaxSignalQualityValue", DValue = 32},
                new SettingsLog {Name = "TemperatureСoefficient", DValue = 1.2},
                new SettingsLog {Name = "PillowСoefficient", DValue = 1},
                new SettingsLog {Name = "AirFlowСoefficient", DValue = 1.97},
                new SettingsLog {Name = "PressureСoefficient", DValue = 0.1},
                new SettingsLog {Name = "RemotePassword", SValue = "2243"},
                new SettingsLog {Name = "FanObjectCount", DValue = 2},
                new SettingsLog {Name = "mineName", SValue = "Название шахты"},
                new SettingsLog {Name = "fansName", SValue = "Один!$!Два"},
                new SettingsLog {Name = "generalAnalogSignalsView", SValue = "0!$!1!$!2!$!3!$!4!$!5!$!6"},
                new SettingsLog
                {
                    Name = "pressure",
                    Warning = 1,
                    Danger = 0.5
                },
                new SettingsLog
                {
                    Name = "airConsumption",
                    Warning = 40,
                    Danger = 30
                },
                new SettingsLog
                {
                    Name = "pillowTemperature",
                    Warning = 60,
                    Danger = 80
                },
                new SettingsLog
                {
                    Name = "pillowVibration",
                    Warning = 20,
                    Danger = 30
                },
                new SettingsLog
                {
                    Name = "statorCurrent",
                    Warning = 100,
                    Danger = 150
                },
                new SettingsLog
                {
                    Name = "rotorCurrentLow",
                    Warning = 200,
                    Danger = 180
                },
                new SettingsLog
                {
                    Name = "rotorCurrentHigh",
                    Warning = 280,
                    Danger = 290
                },
                new SettingsLog
                {
                    Name = "oilFlow",
                    Warning = 6,
                    Danger = 2
                },
                new SettingsLog
                {
                    Name = "gprsQuality",
                    Warning = 21,
                    Danger = 17
                }

            };
            settings.ForEach(s => context.SettingsLog.Add(s));

            var remoteState = new List<RemoteState>
                {
                    new RemoteState {State = "Включить вентилятор №1"},
                    new RemoteState {State = "Включить вентилятор №2"},
                    new RemoteState {State = "Выключить"}
                };
            remoteState.ForEach(s => context.RemoteState.Add(s));

            context.SaveChanges();
        }
    }
}
