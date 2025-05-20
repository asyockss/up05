﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory
{
    public static class CurrentUser
    {
        public static int Id { get; set; }
        public static string Login { get; set; }
        public static string Role { get; set; }
        public static string FullName { get; set; }
        public static bool IsAdmin => Role?.ToLower() == "admin";
    }
}
