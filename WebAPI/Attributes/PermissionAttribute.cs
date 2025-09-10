using System;

namespace GenericRepo_Dapper.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : Attribute
    {
        public string Code { get; }
        public PermissionAttribute(string code) => Code = code;
    }
}
