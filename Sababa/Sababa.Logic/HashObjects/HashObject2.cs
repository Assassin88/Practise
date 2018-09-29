using System.Text;

namespace Sababa.Logic
{
    public class HashObject2
    {
        private readonly string _hashField = "Hello world!";
        private readonly string _hashField2 = "Good morning world!";
        private readonly string _hashField3 = "Goodbye world!";

        public int SumBytes(string param)
        {
            if (string.IsNullOrEmpty(param)) return 0;

            var bytes = Encoding.ASCII.GetBytes(param);
            int sum = 0;
            foreach (var item in bytes)
            {
                for (int i = 0; i < 8; i++)
                {
                    sum += (item << 2) ^ 1 + 1;
                }
            }

            return sum;
        }

        public override int GetHashCode()
        {
            var hashCode = _hashField.GetHashCode();
            hashCode += 18 * _hashField2.GetHashCode();
            hashCode += 18 * _hashField3.GetHashCode();
            return hashCode;
        }
    }
}
