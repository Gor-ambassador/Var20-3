/*
 Массив данных о пассажирах теплохода:
 * ФИО пассажира,
 * номер каюты,
 * тип каюты (люкс, 1, 2, 3 классы),
 * порт назначения (сравнение по полям – номер каюты, порт назначения, ФИО)
 */

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hash.DBStructure
{
    /// <summary>
    /// Class with data about passenger
    /// </summary>
    public class BaseInfo
    {

        /// <value>
        /// Service field for interaction with the database
        /// </value>
        [Key]
        public int InfoId { get; set; }
        /// <value>
        /// Passenger name
        /// </value>
        public string Name { get; set; }
        /// <value>
        /// Destination port
        /// </value>
        public string Port { get; set; }
        /// <value>
        /// Passenger room number
        /// </value>
        public int RoomNumber { get; set; }
        /// <value>
        /// Passenger room type
        /// </value>
        public string RoomType { get; set; }

        public BaseInfo(int InfoId = 0, string Name = "", string Port = "", int RoomNumber = 0, string RoomType = "") {
            this.InfoId = InfoId;
            this.Name = Name;
            this.Port = Port;
            this.RoomNumber = RoomNumber;
            this.RoomType = RoomType;
        }

        public BaseInfo(BaseInfo bi)
        {
            this.InfoId = bi.InfoId;
            this.Name = bi.Name;
            this.Port = bi.Port;
            this.RoomNumber = bi.RoomNumber;
            this.RoomType = bi.RoomType;
        }

        /// <summary>
        /// Operator < overloading
        /// </summary>
        /// <returns>
        /// True if left element is less to the right, else false
        /// </returns>
        /// <param name="left">
        /// Left element
        /// </param>
        /// /// <param name="right">
        /// Right element
        /// </param>
        public static bool operator <(BaseInfo left, BaseInfo right)
        {
            if (left.RoomNumber == right.RoomNumber)
            {
                if (left.Port == right.Port)
                {
                    if (left.Name == right.Name)
                        return false;
                    else if (string.Compare(left.Port, right.Port) < 0)
                        return true;
                    return false;
                }
                else if (string.Compare(left.Port, right.Port) < 0)
                    return true;
                return false;
            }
            else if (left.RoomNumber < right.RoomNumber)
                return true;

            return false;
        }

        /// <summary>
        /// Operator > overloading
        /// </summary>
        /// <returns>
        /// True if left element is greater to the right, else false
        /// </returns>
        /// <param name="left">
        /// Left element
        /// </param>
        /// /// <param name="right">
        /// Right element
        /// </param>
        public static bool operator >(BaseInfo left, BaseInfo right)
        {
            if (left.RoomNumber == right.RoomNumber)
            {
                if (left.Port == right.Port)
                {
                    if (left.Name == right.Name)
                        return false;
                    else if (string.Compare(left.Name, right.Name) > 0)
                        return true;
                    return false;
                }
                else if (string.Compare(left.Port, right.Port) > 0)
                    return true;
                return false;
            }
            else if (left.RoomNumber > right.RoomNumber)
                return true;

            return false;
        }

        /// <summary>
        /// Operator <= overloading
        /// </summary>
        /// <returns>
        /// True if left element is less or equal to the right, else false
        /// </returns>
        /// <param name="left">
        /// Left element
        /// </param>
        /// /// <param name="right">
        /// Right element
        /// </param>
        public static bool operator <=(BaseInfo left, BaseInfo right)
        {

            if (left.RoomNumber == right.RoomNumber)
            {
                if (left.Port == right.Port)
                {
                    if (left.Name == right.Name)
                        return true;
                    else if (string.Compare(left.Name, right.Name) < 0)
                        return true;
                    return false;
                }
                else if (string.Compare(left.Port, right.Port) < 0)
                    return true;
                return false;
            }
            else if (left.RoomNumber < right.RoomNumber)
                return true;

            return false;

        }

        /// <summary>
        /// Operator >= overloading
        /// </summary>
        /// <returns>
        /// True if left element is greater or equal to the right, else false
        /// </returns>
        /// <param name="left">
        /// Left element
        /// </param>
        /// /// <param name="right">
        /// Right element
        /// </param>
        public static bool operator >=(BaseInfo left, BaseInfo right)
        {

            if (left.RoomNumber == right.RoomNumber)
            {
                if (left.Port == right.Port)
                {
                    if (left.Name == right.Name)
                        return true;
                    else if (string.Compare(left.Name, right.Name) > 0)
                        return true;
                    return false;
                }
                else if (string.Compare(left.Port, right.Port) > 0)
                    return true;
                return false;
            }
            else if (left.RoomNumber > right.RoomNumber)
                return true;

            return false;

            //return String.Compare(left.Port, right.Port) >= 0;
        }
    }

    public class InfoGood : BaseInfo
    {
        public UInt32 Hash;

        public InfoGood(int InfoId = 0, string Name = "", string Port = "", int RoomNumber = 0, string RoomType = "")
            : base(InfoId, Name, Port, RoomNumber, RoomType)
        { Hash = Program.GoodHash(Port); }

        public InfoGood(BaseInfo info) : base(info)
        { Hash = Program.GoodHash(Port); }
    }

    public class InfoBad : BaseInfo
    {
        public UInt32 Hash;

        public InfoBad(int InfoId = 0, string Name = "", string Port = "", int RoomNumber = 0, string RoomType = "")
            : base(InfoId, Name, Port, RoomNumber, RoomType)

        { Hash = Program.BadHash(Port); }
   

        public InfoBad(BaseInfo info) : base(info)
        { Hash = Program.BadHash(Port); }
    }
}
