namespace Mc.MvcWeb.Models.Index
{
    public class Parameter
    {
        public string Value { get; set; }

        public string Maximum { get; set; }
        public string Name { get; set; }
        public StateEnum State;
        public string HtmlState 
        { 
            get { return EnumToColor(State); }
        }
        private string EnumToColor(StateEnum stateEnum)
        {
            switch (stateEnum)
            {
                case StateEnum.Warning:
                    return "#FFCC33";
                case StateEnum.Dangerous:
                    return "#FF3300";
                case StateEnum.Ok:
                    return "#33FFCC";
            }
            return "";
        }
    }
    public enum StateEnum
    {
        None, Ok, Warning, Dangerous
    }
          
}