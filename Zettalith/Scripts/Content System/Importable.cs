﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    class ImportContentAttribute : Attribute { }

    /// <summary>
    ///  Applied to static object arrays to flag them as import requests. This is represented through a list of string initially
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field)]
    sealed class ImportAttribute : Attribute
    {
        public string[] input;

        public ImportAttribute(params string[] input)
        {
            this.input = input;
        }
    }
}
