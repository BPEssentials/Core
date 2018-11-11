﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using Newtonsoft.Json;

namespace BP_Essentials
{
    public class Group
    {
        public string Name { get; set; }
        public List<string> Usernames { get; set; }
        public string Message { get; set; }
    }

    class _RootObject
    {
        public List<Group> Groups { get; set; }
    }
    class ReadGroups
    {
        public static void Run()
        {
            try
            {
                Groups.Clear();
                _RootObject m = JsonConvert.DeserializeObject<_RootObject>(FilterComments.Run(CustomGroupsFile));
                foreach (var group in m.Groups)
                {
                    if (!Groups.ContainsKey(group.Name))
                        Groups.Add(group.Name, new _Group { Message = group.Message, Name = group.Name, Users = group.Usernames });
                    else
                        Debug.Log($"{SetTimeStamp.Run()}[ERROR] Cannot add group {group.Name} To dictionary because it already exists!");
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
