using System;
using System.Collections.Generic;
using Mc.Settings.Model.Concrete;

namespace Mc.MvcWeb.Models.Index
{
    public class Fan
    {
        private List<Parameter> _parameters;

        public Fan(int fanObjectId)
        {
            FanObjectId = fanObjectId;
        }

        public int FanObjectId { get; private set; }

        public string FanName
        {
            get {
                try
                {
                    return string.Format("Вентиляторная установка {0}",
                                         Config.Instance.FanObjectConfig.FansName.Split(new string[] {"!$!"},
                                                                                        StringSplitOptions
                                                                                            .RemoveEmptyEntries)[
                                                                                                FanObjectId - 1]);
                }
                catch (Exception)
                {
                    return string.Format("Вентиляторная установка №{0}", FanObjectId);
                }
            }
        }


        public List<Parameter> Values
        {
            get { return _parameters ?? (_parameters = new List<Parameter>()); }
            set
            {
                if (value != null) _parameters = value;
            }
        }
    }
}