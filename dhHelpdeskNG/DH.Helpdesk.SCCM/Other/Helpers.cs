using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Other
{
    public static class Helpers
    {

        public static string getRAM(long physicalMemory)
        {
            return (physicalMemory / (1024 * 1024 * 1024)) + " " + "GB";
        }


        public static string getChassisTypeName(int iChassisType)
        {

            switch (iChassisType)
            {
                case 1:
                    {
                        return "Other";
                    }

                case 2:
                    {
                        return "Unknown";
                    }

                case 3:
                    {
                        return "Desktop";
                    }

                case 4:
                    {
                        return "Low Profile Desktop";
                    }

                case 5:
                    {
                        return "Pizza Box";
                    }

                case 6:
                    {
                        return "Mini Tower";
                    }

                case 7:
                    {
                        return "Tower";
                    }

                case 8:
                    {
                        return "Portable";
                    }

                case 9:
                    {
                        return "Laptop";
                    }

                case 10:
                    {
                        return "Notebook";
                    }

                case 11:
                    {
                        return "Hand Held";
                    }

                case 12:
                    {
                        return "Docking Station";
                    }

                case 13:
                    {
                        return "All in One";
                    }

                case 14:
                    {
                        return "Sub Notebook";
                    }

                case 15:
                    {
                        return "Space-Saving";
                    }

                case 16:
                    {
                        return "Lunch Box";
                    }

                case 17:
                    {
                        return "Main System Chassis";
                    }

                case 18:
                    {
                        return "Expansion Chassis";
                    }

                case 19:
                    {
                        return "Sub Chassis";
                    }

                case 20:
                    {
                        return "Bus Expansion Chassis";
                    }

                case 21:
                    {
                        return "Peripheral Chassis";
                    }

                case 22:
                    {
                        return "Storage Chassis";
                    }

                case 23:
                    {
                        return "Rack Mount Chassis";
                    }

                case 24:
                    {
                        return "Sealed-Case PC";
                    }

                default:
                    {
                        return "";
                    }
            }
        }


    }
}
