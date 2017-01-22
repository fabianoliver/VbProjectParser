using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Compression
{
    public static class BitHelper
    {
        /// <summary>
        /// Takes count elements from the array, starting at ArrayIndex (and moves ArrayIndex forward)
        /// </summary>
        public static Byte[] ReadNBytes(this Byte[] Bytes, int count, ref int ArrayIndex)
        {
            Byte[] result = new Byte[count];
            Array.Copy(Bytes, ArrayIndex, result, 0, count);
            ArrayIndex += count;
            return result;
        }

        public static string ToBitString(this IEnumerable<Byte> Bytes, string separator = " ")
        {
            StringBuilder sb = new StringBuilder();
            bool start = true;

            foreach (var b in Bytes)
            {
                if (!start)
                    sb.Append(separator);

                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));

                start = false;
            }

            return sb.ToString();
        }

        public static string ToBitString(this UInt16 Number)
        {
            return Convert.ToString(Number, 2).PadLeft(sizeof(UInt16) * 8, '0');
        }

        public static string ToBitString(this int Number)
        {
            return Convert.ToString(Number, 2).PadLeft(sizeof(int) * 8, '0');
        }

        public static string ToBitString(this Byte @byte)
        {
            return new[] { @byte }.ToBitString();
        }
 

        /// <summary>
        /// True if the bit is 1, false if the bit is 0
        /// </summary>
        /// <param name="val"></param>
        /// <param name="position">Null-based</param>
        /// <returns></returns>
        public static bool ReadBitAtPosition(this byte val, int position)
        {
            if (position < 0 || position > 7)
                throw new ArgumentOutOfRangeException("val");

            return (val & (1 << position)) != 0x00;
        }

        public const bool DataFormatIsLittleEndianian = true;

        public static ushort ToUInt16(Byte[] vals)
        {
            if (vals.Length != 2)
                throw new ArgumentException();

            if (BitConverter.IsLittleEndian != DataFormatIsLittleEndianian)
                vals = vals.Reverse().ToArray();

            return BitConverter.ToUInt16(vals,0);
        }

        public static uint ToUInt32(Byte[] vals)
        {
            if (vals.Length != 4)
                throw new ArgumentException();

            if (BitConverter.IsLittleEndian != DataFormatIsLittleEndianian)
                vals = vals.Reverse().ToArray();

            return BitConverter.ToUInt32(vals, 0);
        }







        static readonly int[] Empty = new int[0];

        public static int[] Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return Empty;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }

        static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        static bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }
    }
}
