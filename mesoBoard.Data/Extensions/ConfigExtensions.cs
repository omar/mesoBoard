using System;

namespace mesoBoard.Data
{
    public partial class Config
    {
        public bool ToBool()
        {
            if (this.Type != "bool")
                throw new InvalidOperationException("The configuration type must be 'bool' in the database to run this method");

            return bool.Parse(this.Value);
        }

        public int IntValue()
        {
            return int.Parse(this.Value);
        }

        public bool BoolValue()
        {
            return bool.Parse(this.Value);
        }

        public string[] ValuesArray()
        {
            if (this.Type != "bool[]")
                throw new InvalidOperationException("The configuration type must be 'bool[]' in the database to run this method");

            return this.Value.Split(',');
        }

        public string[] OptionsArray()
        {
            if (this.Type != "string[]" || this.Type != "bool[]")
                throw new InvalidOperationException("The configuration type must be 'string[]' or 'bool[]' in the database to run this method");

            return this.Options.Split(',');
        }

        public int ToInt()
        {
            return int.Parse(this.Value);
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}