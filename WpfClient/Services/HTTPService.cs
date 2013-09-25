using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DataRepository.Models;
using Mc.Settings.Model.Concrete;
using WpfClient.Model;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;
using WpfClient.ViewModel;

namespace WpfClient.Services
{
    static class HTTPService
    {
        private const string FilePath = "../../../Mc.HTTPServer/www/index.html";
        private static List<List<ParameterVm>> parameters;
        public static void HTTPServiceInit()
        {
            parameters = new List<List<ParameterVm>>();
            for (var i = 1; i <= Config.Instance.FanObjectConfig.FanObjectCount; i++)
            {
                var fanObject = IoC.Resolve<DatabaseService>().GetFanObject(i);
                if (fanObject == null) continue;

                parameters.Add(new List<ParameterVm>());

                parameters[i - 1].Add(checkRemoteSignalState(i));
                parameters[i - 1].Add(getFanNumberParameter(fanObject));
                parameters[i - 1].Add(getFanStateParameter(fanObject));
                fanObject.Parameters.ForEach(p => parameters[i - 1].Add(new ParameterVm(p)));
            }    
        }
        public static void UpdateData(FanLog fanLog)
        {
            if (parameters.Count != Config.Instance.FanObjectConfig.FanObjectCount)
            {
                HTTPServiceInit();
                WriteDataToIndexFile();
                return;
            }
            var parameterList = new List<ParameterVm>();
            var fanObject = IoC.Resolve<DatabaseService>().GetFanObject(fanLog);
            parameterList.Add(checkRemoteSignalState(fanObject.FanObjectId));
            parameterList.Add(getFanNumberParameter(fanObject));
            parameterList.Add(getFanStateParameter(fanObject));
            fanObject.Parameters.ForEach(p => parameterList.Add(new ParameterVm(p)));
            parameters[fanObject.FanObjectId - 1] = parameterList;
            WriteDataToIndexFile();
        }
        public static void WriteDataToIndexFile()
        {
            if (!File.Exists(FilePath))
            {
                return;
            }
            FileStream FS=null;
            StreamWriter streamWriter = null;
            while (true)
            {
                try
                {
                    FS = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    streamWriter = new StreamWriter(FS);
                    streamWriter.WriteLine("<html>");
                    streamWriter.WriteLine("<head>");
                    streamWriter.WriteLine(@"<meta charset=""utf-8"">");
                    streamWriter.WriteLine("<title>ООО «Донбассуглеавтоматика»</title>");
                    streamWriter.WriteLine("<style>");
                    streamWriter.WriteLine("table { ");
                    streamWriter.WriteLine("border-collapse: separate; /* Способ отображения границы */ ");
                    streamWriter.WriteLine("width: 700px; /* Ширина таблицы */ ");
                    streamWriter.WriteLine("border-spacing: 7px 11px; /* Расстояние между ячейками */ ");
                    streamWriter.WriteLine("margin: auto;");
                    streamWriter.WriteLine(" }");
                    streamWriter.WriteLine(" td {");
                    streamWriter.WriteLine("padding: 5px; /* Поля вокруг текста */ ");
                    streamWriter.WriteLine("border: 1px solid #a52a2a; /* Граница вокруг ячеек */");
                    streamWriter.WriteLine("}");
                    streamWriter.WriteLine("a {");
                    streamWriter.WriteLine("text-decoration: none; /* Убирает подчеркивание для ссылок */");
                    streamWriter.WriteLine("color: black; /* цвет текста */");
                    streamWriter.WriteLine("}");
                    streamWriter.WriteLine("body {");
                    streamWriter.WriteLine("background-color: #efefef;");
                    streamWriter.WriteLine("}");
                    streamWriter.WriteLine("</style>");
                    streamWriter.WriteLine("</head>");
                    streamWriter.WriteLine("<body>");
                    streamWriter.WriteLine(@"<h1 align=""center"">" + Config.Instance.FanObjectConfig.MineName + "</h1>");
                    streamWriter.WriteLine("<table>");

                    streamWriter.WriteLine(@"<td align=""center"" bgcolor=""#efefef"">" + DateTime.Now + "</td>");
                    string[] fanNames = Config.Instance.FanObjectConfig.FansName.Split(new string[] { "!$!" }, StringSplitOptions.RemoveEmptyEntries);
                    int j = 1;
                    foreach (var fanName in fanNames)
                    {
                        try
                        {
                            streamWriter.WriteLine(@"<td align=""center"" bgcolor=""#fafafa"">
                                <a href=""control.html?Вентиляторная установка " + fanName + "&" + j.ToString() + "&" + parameters[j - 1][1].Value.Substring(1)
                                               + @""">
                                Вентиляторная установка " + fanName + "</a></td>");
                        }
                        catch (Exception)
                        {
                            streamWriter.WriteLine(@"<td align=""center"" bgcolor=""#fafafa"">
                                Вентиляторная установка " + fanName + "</td>");
                        }
                        
                        j++;
                    }

                    for (int i = 0; i < parameters[0].Count; i++)
                    {
                        streamWriter.WriteLine("<tr>");
                        streamWriter.WriteLine(@"<td align=""center"" bgcolor=""#fafafa"">" + parameters[0][i].Name + "</td>");
                        foreach (var parameter in parameters)
                            streamWriter.WriteLine(@"<td align=""center"" bgcolor=" + EnumToColor(parameter[i].State) + @""">" + parameter[i].Value + "</td>"); 
                        streamWriter.WriteLine("</tr>");
                    }




                    streamWriter.WriteLine("</table>");
                    streamWriter.WriteLine("<script>");
                    streamWriter.WriteLine("function fresh() {");
                    streamWriter.WriteLine("location.reload();");
                    streamWriter.WriteLine("}");
                    streamWriter.WriteLine(@"setInterval(""fresh()"",19000);");
                    streamWriter.WriteLine("</script>");
                    streamWriter.WriteLine("</body>");
                    streamWriter.WriteLine("</html>");
                    streamWriter.Close();
                    FS.Close();
                    break;
                }
                catch (Exception)
                {
                    if (streamWriter != null)
                       streamWriter.Close();
                    if(FS!=null)
                       FS.Close();
                    return;
                }
            }

        }
        private static string EnumToColor(StateEnum stateEnum)
        {
            switch (stateEnum)
            {
                    case StateEnum.Warning:
                    return "FFCC33";
                    case  StateEnum.Dangerous:
                    return "FF3300";
                    case StateEnum.Ok:
                    return "33FFCC";
            }
            return "";
        }
        private static ParameterVm checkRemoteSignalState(int fanObjectId)
        {
            ParameterVm SignalState = new ParameterVm();
            SignalState.Name = "Состояние сигнала";
            if (System.DateTime.Now - RemoteService.GetLastRecieve(fanObjectId) > new TimeSpan(0, 1, 0))
            {
                SignalState.Value = "отсутствует";
                SignalState.State = StateEnum.Dangerous;
            }
            else
            {
                SignalState.Value = "стабильный";
                SignalState.State = StateEnum.Ok;
            }
            return SignalState;
        }

        private static ParameterVm getFanStateParameter(FanObject fanObject)
        {
            return IoC.Resolve<FanService>().GetFanMode(fanObject.WorkingFanNumber, fanObject.Doors);
        }

        private static ParameterVm getFanNumberParameter(FanObject fanObject)
        {
            var parameter = new ParameterVm {Value = fanObject.WorkingFanNumber == 0 ? "АВАРИЯ" : string.Format("№{0}", fanObject.WorkingFanNumber), Name = "Вентилятор в работе"};
            parameter.State = SystemStateService.GetFanState(fanObject.WorkingFanNumber);

            return parameter;
        }
    }
}
