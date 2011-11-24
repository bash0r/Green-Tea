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
            if (v is GTLazy)
                v = ((GTLazy)v).Val;

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

        internal static int CompareTo(this Value a, Value b)
        {
            if (a is GTLazy)
                a = ((GTLazy)a).Val;

            if (b is GTLazy)
                b = ((GTLazy)b).Val;

            if (a.IsNumber() && b.IsNumber())
                return a.AsNumber().CompareTo(b.AsNumber());

            if (a.Type != b.Type)
                return -2; // invalid

            switch (a.Type)
            {
                case GTType.Bool:
                    return ((GTBool)a).Value.CompareTo(((GTBool)b).Value);

                case GTType.Function:
                    return a == b ? 0 : -1;

                case GTType.List:
                    if (a.Count != b.Count)
                        return a.Count.CompareTo(b.Count);

                    for (int i = 0; i < a.Count; i++)
                    {
                        var comp = a[i].CompareTo(b[i]);

                        if (comp != 0)
                            return comp;
                    }

                    return 0;

                case GTType.String:
                    return ((GTString)a).Value.CompareTo(((GTString)b).Value);

                case GTType.Void:
                    return -1;
            }

            return -2;
        }
    }
}
