﻿using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public abstract class LiquidValue : ExpressionDescription, ILiquidValue
    {
        public static readonly Option<ILiquidValue> None = new None<ILiquidValue>();

        protected LiquidValue()
        {
            MetaData = new Dictionary<string, object>();
        }


        public abstract object Value
        {
            get;
        }

        /// <summary>
        /// This should correspond to this:
        /// https://docs.shopify.com/themes/liquid-documentation/basics/true-and-false
        /// </summary>
        public abstract bool IsTrue
        {
            get;
        }

        public abstract string LiquidTypeName { get; }

        public Option<ILiquidValue> ToOption()
        {
            if (Value != null)
            {
                return new Some<ILiquidValue>(this);
            }
            else
            {
                return new None<ILiquidValue>();
            }
        }

        public IDictionary<string, object> MetaData { get; private set; }

        public static implicit operator Option<ILiquidValue>(LiquidValue t)
        {
            return Option<ILiquidValue>.Create(t);
        }

        public override bool Equals(object otherObj)
        {
            if (otherObj == null)
            {
                return false;
            }
            else
            {
                var exprConstant = otherObj as LiquidValue;
                if (exprConstant != null)
                {
                    return exprConstant.Value.Equals(Value);
                }
                else
                {
                    return Value == null;   
                }
            }
        }


        public override int GetHashCode()
        {
            return Value == null ? 0 : Value.GetHashCode();
        }

        public override LiquidExpressionResult Eval(
            ITemplateContext templateContext,
            IEnumerable<Option<ILiquidValue>> childresults)
        {
            return LiquidExpressionResult.Success(this);
        }

    }
}
