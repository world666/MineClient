using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Model;
using WpfClient.Model.Concrete;
using WpfClient.ViewModel;

namespace WpfClient.Services
{
    class HTTPService
    {
        private const string FilePath = "../../../Mc.HTTPServer/www/index.html";
        public void WriteDataToIndexFile(List<List<ParameterVm>> parameters)
        {
            if (!File.Exists(FilePath))
            {
                return;
            }
            FileStream FS;
            StreamWriter streamWriter;
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
                    streamWriter.WriteLine("</style>");
                    streamWriter.WriteLine("</head>");
                    streamWriter.WriteLine("<body>");
                    streamWriter.WriteLine(@"<h1 align=""center"">" + Config.Instance.FanObjectConfig.MineName + "</h1>");
                    streamWriter.WriteLine("<table>");

                    streamWriter.WriteLine(@"<td align=""center"">" + DateTime.Now + "</td>");
                    string[] fanNames = Config.Instance.FanObjectConfig.FansName.Split(new string[] { "!$!" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var fanName in fanNames)
                        streamWriter.WriteLine(@"<td align=""center""><a href=""http://127.0.0.1:90/control.html?Вентиляторная установка " + fanName + @""">
                                                                                                                            Вентиляторная установка " + fanName + "</a></td>");
 
                    for (int i = 0; i < parameters[0].Count; i++)
                    {
                        streamWriter.WriteLine("<tr>");
                        streamWriter.WriteLine(@"<td align=""center"">" + parameters[0][i].Name + "</td>");
                        foreach (var parameter in parameters)
                            streamWriter.WriteLine(@"<td align=""center"" bgcolor=" + EnumToColor(parameter[i].State) + @""">" + parameter[i].Value + "</td>"); 
                        streamWriter.WriteLine("</tr>");
                    }




                    streamWriter.WriteLine("</table>");
                    streamWriter.WriteLine("</body>");
                    streamWriter.WriteLine("</html>");
                    streamWriter.Close();
                    FS.Close();
                    break;
                }
                catch (Exception)
                {
                    continue;
                }
            }

        }
        private string EnumToColor(StateEnum stateEnum)
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
    }
}
