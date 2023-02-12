﻿using LikeNtfsWalker.UI;
using System.Collections.Generic;

namespace LikeNtfsWalker.Model
{
    public class FileInfo : Notifier
    {
        private string title;
        
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> Properties { get; set; }

        public FileInfo(string title, Dictionary<string,string> properties)
        {
            this.title = title; 
            this.Properties = properties;


            // test
            /*datalist.Add("A", "123");
            datalist.Remove("A");
            datalist["A"] = "456";
            datalist["B"] = "789";*/
        }

        



    }
}
