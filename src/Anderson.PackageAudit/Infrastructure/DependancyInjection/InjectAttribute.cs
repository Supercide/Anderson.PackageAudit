using System;
using Microsoft.Azure.WebJobs.Description;

namespace Anderson.PackageAudit
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
    }
}