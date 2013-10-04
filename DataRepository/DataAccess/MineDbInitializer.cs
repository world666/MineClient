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
                    new AnalogSignal {Type = "Расход Воздуха (м3/c)"},
                    new AnalogSignal {Type = "Депрессия (кПа)"},
                    new AnalogSignal {Type = "Температура подшипника №1 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №2 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №3 (град)"},
                    new AnalogSignal {Type = "Температура подшипника №4 (град)"},
                    new AnalogSignal {Type = "Вибрация подшипника №1 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №2 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №3 (мкм)"},
                    new AnalogSignal {Type = "Вибрация подшипника №4 (мкм)"},
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
                    new SettingsLog {Name = "MaxSignalQualityValue", DValue = 32},
                    new SettingsLog {Name = "TemperatureСoefficient", DValue = 1.2},
                    new SettingsLog {Name = "PillowСoefficient", DValue = 1},
                    new SettingsLog {Name = "AirFlowСoefficient", DValue = 1.97},
                    new SettingsLog {Name = "PressureСoefficient", DValue = 0.1},
                    new SettingsLog {Name = "RemotePassword", SValue = "2243"},
                    new SettingsLog {Name = "FanObjectCount", DValue = 2},
                    new SettingsLog {Name = "mineName", SValue = "Название шахты"},
                    new SettingsLog {Name = "fansName", SValue = "Один!$!Два"},
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
                            Name = "gprsQuality",
                            Warning = 21,
                            Danger = 17
                        }

                };
            settings.ForEach(s => context.SettingsLog.Add(s));

            context.SaveChanges();
        }
    }
}
