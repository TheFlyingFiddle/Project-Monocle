using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using Game;

namespace EntityFramework
{
    class Program
    {
        public static void Main(string[] args)
        {
        }
    }

    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method, TargetMemberAttributes =  MulticastAttributes.Instance)]
    public class NullCheckAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            MonocleObject moObj = args.Instance as MonocleObject;
            if (moObj == null)
                throw new NullReferenceException("e");
        }



        public override bool CompileTimeValidate(System.Reflection.MethodBase method)
        {
            if (!typeof(MonocleObject).IsAssignableFrom(method.DeclaringType))
                return false;

            return base.CompileTimeValidate(method);
        }
    }
}
