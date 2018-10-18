namespace Playtika.NETSummit.Problem3
{
    public class Dress
    {
        public int Size { get; }
        public int Growth { get; }
        public int Price { get; }

        public Dress(int size, int growth, int price)
        {
            Size = size;
            Growth = growth;
            Price = price;
        }

        public override int GetHashCode()
        {
            var hashCode = 352033997;
            hashCode *= 15213 + Size.GetHashCode();
            hashCode *= 15213 + Growth.GetHashCode();
            hashCode *= 15213 + Price.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;

            Dress dress = (Dress)obj;
            return this.Size == dress.Size &&
                   this.Growth == dress.Growth &&
                   this.Price == dress.Price;
        }
    }
}