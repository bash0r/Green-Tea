using System;

namespace GreenTea
{
    internal static class ValueExt
    {
        internal static bool IsNumber(this Value v)
        {
            return (v.Type == GTType.Float || v.Type == GTType.Integer);
        }

        public static bool IsTrue(this Value v)
        {
            if (v.Type == GTType.Bool)
                if (((GTBool)v).Value)
                    return true;
                else
                    return false;
            else if (v.Count == 0)
                return false;
            else
                throw new InvalidOperationException(v.ToString() + " is not a conditional, got " + v.Type);
        }

        internal static dynamic AsNumber(this Value v)
        {
            if (v is GTLazy)
                v = ((GTLazy)v).Val;

            switch (v.Type)
            {
                case GTType.Float:
                    return ((GTFloat)v).Value;

                case GTType.Integer:
                    return ((GTInt)v).Value;

                case GTType.Bool:
                    return ((GTBool)v).Value ? 1 : 0;

                default:
                    return v.Count;
            }
        }

        internal static bool Equal(this Value a, Value b)
        {
            if (a is GTLazy)
                a = ((GTLazy)a).Val;

            if (b is GTLazy)
                b = ((GTLazy)b).Val;

            if (a.IsNumber() && b.IsNumber())
                return a.AsNumber() == b.AsNumber();

            if (a.Type != b.Type)
                return false;

            switch (a.Type)
            {
                case GTType.Bool:
                    return ((GTBool)a).Value == ((GTBool)b).Value;

                case GTType.Function:
                    return a == b;

                case GTType.List:
                    if (a.Count != b.Count)
                        return false;

                    for (int i = 0; i < a.Count; i++)
                        if (a[i] != b[i])
                            return false;

                    return true;

                case GTType.String:
                    return ((GTString)a).Value == ((GTString)b).Value;

                case GTType.Void:
                    return true;
            }

            return false;
        }
    }
}
