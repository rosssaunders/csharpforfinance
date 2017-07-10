using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace Library.Data
{
    

    //Binary operator delegate for types T
    public delegate TResult BinaryOperatorT<TLeft, TRight, TResult>(TLeft left, TRight right);


    public static class GenericOperatorFactory<TLeft, TRight, TResult, TOwner>
    {
        private static BinaryOperatorT<TLeft, TRight, TResult> add;
        private static BinaryOperatorT<TLeft, TRight, TResult> subtract;
        private static BinaryOperatorT<TLeft, TRight, TResult> multiply;
        private static BinaryOperatorT<TLeft, TRight, TResult> divide;

        #region Generic addition for type T
        //Addition
        public static BinaryOperatorT<TLeft, TRight, TResult> Add
        {
            get
            {
                if (add == null)
                {
                    //Create a dynamic method and an intermediate language generator
                    //DynamicMethod method = new DynamicMethod( "op_Addition", typeof( TLeft ), new Type[] { typeof( TLeft ), typeof( TRight ) }, typeof( TOwner ) );
                    DynamicMethod method = new DynamicMethod("op_Addition", typeof(TResult), new Type[] { typeof(TLeft), typeof(TRight) }, typeof(TOwner));
                    ILGenerator generator = method.GetILGenerator();

                    //Put arguments on the evaluation stack
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);

                    if (typeof(TLeft).GetTypeInfo().IsPrimitive)
                    {
                        //Add the 2 arguments and push the result on the evaluation stack
                        generator.Emit(OpCodes.Add);
                    }
                    else
                    {
                        MethodInfo info = typeof(TLeft).GetTypeInfo().GetMethod("op_Addition", new Type[] { typeof(TLeft), typeof(TRight) }, null);
                        generator.EmitCall(OpCodes.Call, info, null);
                    }

                    //Push return value on the callee's evaluation stack
                    generator.Emit(OpCodes.Ret);

                    //Finish dynamic method and create a delegate
                    add = (BinaryOperatorT<TLeft, TRight, TResult>)method.CreateDelegate(typeof(BinaryOperatorT<TLeft, TRight, TResult>));
                }

                return add;
            }
        }
        #endregion


        #region Generic substraction for type T
        //Substraction
        public static BinaryOperatorT<TLeft, TRight, TResult> Subtract
        {
            get
            {
                if (subtract == null)
                {
                    //Create a dynamic method and an intermediate language generator
                    //DynamicMethod method = new DynamicMethod( "op_Subtraction", typeof( TLeft ), new Type[] { typeof( TLeft ), typeof( TRight ) }, typeof( TOwner ) );
                    DynamicMethod method = new DynamicMethod("op_Subtraction", typeof(TResult), new Type[] { typeof(TLeft), typeof(TRight) }, typeof(TOwner));
                    ILGenerator generator = method.GetILGenerator();

                    //Put arguments on the evaluation stack
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);

                    if (typeof(TLeft).GetTypeInfo().IsPrimitive)
                    {
                        //Substract the 2 arguments and push the result on the evaluation stack
                        generator.Emit(OpCodes.Sub);
                    }
                    else
                    {
                        MethodInfo info = typeof(TLeft).GetTypeInfo().GetMethod("op_Subtraction", new Type[] { typeof(TLeft), typeof(TRight) }, null);
                        generator.EmitCall(OpCodes.Call, info, null);
                    }

                    //Push return value from the evaluation stack
                    generator.Emit(OpCodes.Ret);

                    //Finish dynamic method and create a delegate
                    subtract = (BinaryOperatorT<TLeft, TRight, TResult>)method.CreateDelegate(typeof(BinaryOperatorT<TLeft, TRight, TResult>));
                }

                return subtract;
            }
        }
        #endregion


        #region Generic multiplication for type T
        //Multiplication
        public static BinaryOperatorT<TLeft, TRight, TResult> Multiply
        {
            get
            {
                if (multiply == null)
                {
                    //Create a dynamic method and an intermediate language generator
                    //DynamicMethod method = new DynamicMethod( "op_Multiplication", typeof( TLeft ), new Type[] { typeof( TLeft ), typeof( TRight ) }, typeof( TOwner ) );
                    DynamicMethod method = new DynamicMethod("op_Multiplication", typeof(TResult), new Type[] { typeof(TLeft), typeof(TRight) }, typeof(TOwner));
                    ILGenerator generator = method.GetILGenerator();

                    //Put arguments on the evaluation stack
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);

                    if (typeof(TLeft).GetTypeInfo().IsPrimitive)
                    {
                        //Multiply the 2 arguments and push the result on the evaluation stack
                        generator.Emit(OpCodes.Mul);
                    }
                    else
                    {
                        MethodInfo info = typeof(TLeft).GetTypeInfo().GetMethod("op_Multiplication", new Type[] { typeof(TLeft), typeof(TRight) }, null);
                        generator.EmitCall(OpCodes.Call, info, null);
                    }

                    //Push return value from the evaluation stack
                    generator.Emit(OpCodes.Ret);

                    //Finish dynamic method and create a delegate
                    multiply = (BinaryOperatorT<TLeft, TRight, TResult>)method.CreateDelegate(typeof(BinaryOperatorT<TLeft, TRight, TResult>));
                }

                return multiply;
            }
        }
        #endregion

        #region Generic Division for type T
        //Multiplication
        public static BinaryOperatorT<TLeft, TRight, TResult> Divide
        {
            get
            {
                if (divide == null)
                {
                    //Create a dynamic method and an intermediate language generator
                    //DynamicMethod method = new DynamicMethod( "op_Multiplication", typeof( TLeft ), new Type[] { typeof( TLeft ), typeof( TRight ) }, typeof( TOwner ) );
                    DynamicMethod method = new DynamicMethod("op_Division", typeof(TResult), new Type[] { typeof(TLeft), typeof(TRight) }, typeof(TOwner));
                    ILGenerator generator = method.GetILGenerator();

                    //Put arguments on the evaluation stack
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_1);

                    if (typeof(TLeft).GetTypeInfo().IsPrimitive)
                    {
                        //Multiply the 2 arguments and push the result on the evaluation stack
                        generator.Emit(OpCodes.Div);
                    }
                    else
                    {
                        MethodInfo info = typeof(TLeft).GetTypeInfo().GetMethod("op_Division", new Type[] { typeof(TLeft), typeof(TRight) }, null);
                        generator.EmitCall(OpCodes.Call, info, null);
                    }

                    //Push return value from the evaluation stack
                    generator.Emit(OpCodes.Ret);

                    //Finish dynamic method and create a delegate
                    divide = (BinaryOperatorT<TLeft, TRight, TResult>)method.CreateDelegate(typeof(BinaryOperatorT<TLeft, TRight, TResult>));
                }

                return divide;
            }
        }
        #endregion
    }
}
