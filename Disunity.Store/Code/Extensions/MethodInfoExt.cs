using System.Linq;
using System.Reflection;


namespace Disunity.Store.Code.Extensions {

    public static class MethodInfoExt {

        public static string FormatNicely(this MethodInfo method) {
            var returnType = method.ReturnType.Name;
            var methodName = method.Name;
            var generic_args = method.GetGenericArguments().Select(a => a.Name);
            var generics = generic_args.Count() > 0 ? $"<{string.Join(',', generic_args)}>" : "";
            var parameters = string.Join(',', method.GetParameters().Select(p => p.ParameterType.Name));
            return $"{returnType} {methodName}{generics} ({parameters})";
        }

    }

}