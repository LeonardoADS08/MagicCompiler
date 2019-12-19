namespace MagicCompiler.Matlab
{
    public class sas_Matrix<T>
    {
        public int Rows;
        public int Columns;
        public T[,] Values;

        public sas_Matrix()
        {
            Rows = Columns = 0;
        }

        public sas_Matrix(int rows, int columns, T[,] values)
        {
            Rows = rows;
            Columns = columns;
            Values = values;
        }
    }
}



