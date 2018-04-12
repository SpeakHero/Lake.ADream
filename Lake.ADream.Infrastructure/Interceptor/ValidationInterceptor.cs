//using Lake.ADream.Infrastructure.Attributes;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text;

//namespace Lake.ADream.Infrastructure.Interceptor
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class ValidationInterceptor : IInterceptor
//    {
//        public void Intercept(IInvocation invocation)
//        {
//            ParameterInfo[] parameters = invocation.Method.GetParameters();
//            for (int index = 0; index < parameters.Length; index++)
//            {
//                var paramInfo = parameters[index];
//                var attributes = paramInfo.GetCustomAttributes(typeof(ArgumentValidationAttribute), false);

//                if (attributes.Length == 0)
//                    continue;

//                foreach (ArgumentValidationAttribute attr in attributes)
//                {
//                    attr.Validate(invocation.Arguments[index], paramInfo.Name);
//                }
//            }

//            invocation.Proceed();
//        }
//    }
//}
