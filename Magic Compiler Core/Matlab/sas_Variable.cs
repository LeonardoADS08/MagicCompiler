namespace MagicCompiler.Matlab
{
    public class sas_Variable<T>
    {
        public string Name;
        public T Value;
        
        public sas_Variable()
        {

        }

        public sas_Variable(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}



