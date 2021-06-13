using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class KeyValue
    {

        public KeyValue()
        {

        }
        public KeyValue(string caption, int value)
        {
            Caption = caption;
            Value = value;
        }
        public string Caption { get; set; }


        public string DisplayMember
        {
            get
            {
                return $"{Caption} ({Value})";
            }
        }


        public List<string> CaptionData { get; set; }

        public int Value { get; set; }

        public int Id { get; set; }

        public override string ToString()
        {
            return $"{Caption} - {Value}";
        }

    }
}
