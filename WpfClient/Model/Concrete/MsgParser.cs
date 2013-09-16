using System;
using System.Linq;
using System.Collections.Generic;
using WpfClient.Model.Abstract;
using WpfClient.Services;

namespace WpfClient.Model.Concrete
{
    public class MsgParser : IMsgParser
    {
        private const int _expextedValueCount = 30;

        public Dictionary<string, int> Parse(string message)
        {
            var partedStr = message.Split(new string[] {"separ"}, StringSplitOptions.RemoveEmptyEntries);

            var separators = new char[] {' ', '='};

            var signalStr = partedStr[0].Trim().Split(separators);
            var doorStr = partedStr[1].Trim().Split(separators);
            var analogStr = partedStr[2].Trim().Split(separators);


            var paramTable = new Dictionary<string, int>(_expextedValueCount/2);
            //Parse signalStr
            for (int i = 0; i < signalStr.Length; i += 2)
            {
                paramTable[signalStr[i]] = Convert.ToInt32(signalStr[i+1]) + 2;
            }
            //Parse doorlStr
            for (int i = 0; i < doorStr.Length; i += 2)
            {
                paramTable[doorStr[i]] = Convert.ToInt32(doorStr[i + 1], 2) + 1;
            }

            //Parge analog signals
            for (int i = 0; i < analogStr.Length; i += 2) 
            {
                paramTable[analogStr[i]] = Convert.ToInt32(analogStr[i + 1]);
            }

            return paramTable;
        }
    }
}
