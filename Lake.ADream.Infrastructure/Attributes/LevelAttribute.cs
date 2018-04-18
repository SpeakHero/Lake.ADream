using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class LevelAttribute : Attribute
    {
        public int Level { get; set; } = 0;
        /// <summary>
        /// 0:page 1:button
        /// </summary>
        /// <param name="Level"></param>
        public LevelAttribute(int Level=0) { this.Level = Level; }
    }
  
}
