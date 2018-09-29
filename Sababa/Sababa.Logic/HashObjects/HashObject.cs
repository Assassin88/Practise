using System.Text;

namespace Sababa.Logic
{
    public class HashObject
    {
        private readonly int _hashField = 202;
        private readonly int _hashField2 = 205;

        public int SumBytes(string param)
        {
            if (string.IsNullOrEmpty(param)) return 0;

            var bytes = Encoding.ASCII.GetBytes(param);
            int sum = 0;
            foreach (var item in bytes)
            {
                for (int i = 0; i < 8; i++)
                {
                    sum += (item >> 1) & 1;
                }
            }

            return sum;
        }

        public override int GetHashCode()
        {
            var hashCode = _hashField.GetHashCode();
            return 18 * hashCode + _hashField2.GetHashCode();
        }
    }
}